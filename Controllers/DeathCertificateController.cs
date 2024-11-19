using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using static MOCDIntegrations.Models.DeathCertificateDetails;
using System.Web.Http.Description;
using System.ServiceModel;
using System.Text;
using static MOCDIntegrations.Models.OwnerDetails;

namespace MOCDIntegrations.Controllers
{
    public class DeathCertificateController : Controller
    {
        // GET: DeathCertificate
        public ActionResult Index()
        {
            return View();
        }
        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["death_c_uri"].ToString(), ConfigurationManager.AppSettings["death_c_grant_type"].ToString(), ConfigurationManager.AppSettings["death_c_username"].ToString(), ConfigurationManager.AppSettings["death_c_password"].ToString(), "crm");
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;

            List<DeathCertificateDetails.data> listdata = new List<DeathCertificateDetails.data>();
            List<DeathCertificateDetailsRequestParams> listdata_Request = new List<DeathCertificateDetailsRequestParams>();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["DeathCertificateDetails_URL"].ToString());
                request.Method = "POST";
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                //request.ContentLength = requestBody.Length;
                string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["ADDED_Username"].ToString() + ":" + ConfigurationManager.AppSettings["ADDED_Password"].ToString()));

                request.Headers.Add(System.Net.HttpRequestHeader.Authorization, $"Basic {vToken}");
                request.Headers.Add("GSB-APIKey", ConfigurationManager.AppSettings["DeathCertificateDetails_API_KEY"].ToString());
//                request.Headers.Add("Authorization", "Bearer " + GenerateToken());
                request.Headers.Add("SOAPAction", "urn:ae:gov:abudhabi:DigitalDocuments:DeathCertificatePull:getDeathCertificatePullDetails");
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(@"
                                            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:ae:gov:abudhabi:DigitalDocuments:DeathCertificatePullMessages:1"" xmlns:urn1=""urn:ae:gov:abudhabi:DigitalDocuments:HealthTypes:1"">
                                               <soapenv:Header>
                                                  <urn:RequestTrnHeader>
                                                     <!--Optional:-->
                                                     <urn1:TransactionId>DoH_MOCD_123456789</urn1:TransactionId>
                                                     <!--Optional:-->
                                                     <urn1:EntityCode>MOCD</urn1:EntityCode>
                                                  </urn:RequestTrnHeader>
                                               </soapenv:Header>
                                               <soapenv:Body>
                                                  <urn:DeathCertificatePullDetailsRequest>
                                                     <urn:EmiratesID>" + EmiratesId + @"</urn:EmiratesID>
                                                  </urn:DeathCertificatePullDetailsRequest>
                                               </soapenv:Body>
                                            </soapenv:Envelope>");

                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
                string soapResult = string.Empty;
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        StringReader StringStream = new StringReader(soapResult);

                        DataSet ds = new DataSet();
                        ds.ReadXml(StringStream);
                        data deathResponse = new data();

                        if (ds.Tables["ResponseStatus"] != null && ds.Tables["ResponseStatus"].Columns.Count > 0)
                        {

                            DataColumnCollection columns = ds.Tables["ResponseStatus"].Columns;
                            if (columns.Contains("StatusCode"))
                            {
                                deathResponse.StatusCode = ds.Tables["ResponseStatus"].Rows[0]["StatusCode"].ToString();
                            }
                            else
                            {
                                deathResponse.StatusCode = "NULL";
                            }
                            if (columns.Contains("StatusDescriptionEnglish"))
                            {
                                deathResponse.StatusDescriptionEnglish = ds.Tables["ResponseStatus"].Rows[0]["StatusDescriptionEnglish"].ToString();
                            }
                            else { deathResponse.StatusDescriptionEnglish = "NULL"; }
                            if (columns.Contains("StatusDescriptionArabic"))
                            {
                                deathResponse.StatusDescriptionArabic = ds.Tables["ResponseStatus"].Rows[0]["StatusDescriptionArabic"].ToString();
                            }
                            else { deathResponse.StatusDescriptionArabic = "NULL"; }
                        }
                        if (ds.Tables["Contents"] != null && ds.Tables["Contents"].Columns.Count > 0)
                        {
                            DataColumnCollection columns = ds.Tables["Contents"].Columns;
                            if (columns.Contains("DeathReferenceNo"))
                            {
                                deathResponse.DeathReferenceNo = ds.Tables["Contents"].Rows[0]["DeathReferenceNo"].ToString();
                            }
                            if (columns.Contains("FullNameEnglish"))
                            {
                                deathResponse.FullNameEnglish = ds.Tables["Contents"].Rows[0]["FullNameEnglish"].ToString();
                            }
                            if (columns.Contains("FullNameArabic"))
                            {
                                deathResponse.FullNameArabic = ds.Tables["Contents"].Rows[0]["FullNameArabic"].ToString();
                            }
                            if (columns.Contains("DateOfBirth"))
                            {
                                deathResponse.DateOfBirth = ds.Tables["Contents"].Rows[0]["DateOfBirth"].ToString();
                            }
                            if (columns.Contains("DeathPlace"))
                            {
                                deathResponse.DeathPlace = ds.Tables["Contents"].Rows[0]["DeathPlace"].ToString();

                            }
                            if (columns.Contains("DateOfDeath"))
                            {
                                deathResponse.DateOfDeath = ds.Tables["Contents"].Rows[0]["DateOfDeath"].ToString();
                            }
                            if (columns.Contains("DateOfDeathInWordsEnglish"))
                            {
                                deathResponse.DateOfDeathInWordsEnglish = ds.Tables["Contents"].Rows[0]["DateOfDeathInWordsEnglish"].ToString();
                            }
                            if (columns.Contains("DateOfDeathHijri"))
                            {
                                deathResponse.DateOfDeathHijri = ds.Tables["Contents"].Rows[0]["DateOfDeathHijri"].ToString();
                            }
                            if (columns.Contains("Age"))
                            {
                                deathResponse.Age = ds.Tables["Contents"].Rows[0]["Age"].ToString();
                            }
                        }
                        if (ds.Tables["Gender"] != null && ds.Tables["Gender"].Columns.Count > 0)
                        {
                            DataColumnCollection columns = ds.Tables["Gender"].Columns;
                            if (columns.Contains("Code"))
                            {
                                deathResponse.GenderCode = ds.Tables["Gender"].Rows[0]["Code"].ToString();
                            }
                            if (columns.Contains("DescriptionEnglish"))
                            {
                                deathResponse.GenderDescriptionEnglish = ds.Tables["Gender"].Rows[0]["DescriptionEnglish"].ToString();
                            }
                            if (columns.Contains("DescriptionArabic"))
                            {
                                deathResponse.GenderDescriptionArabic = ds.Tables["Gender"].Rows[0]["DescriptionArabic"].ToString();
                            }
                        }

                        if (ds.Tables["Religion"] != null && ds.Tables["Religion"].Columns.Count > 0)
                        {
                            DataColumnCollection columns = ds.Tables["Religion"].Columns;
                            if (columns.Contains("Code"))
                            {
                                deathResponse.ReligonCode = ds.Tables["Religion"].Rows[0]["Code"].ToString();
                            }
                            if (columns.Contains("DescriptionEnglish"))
                            {
                                deathResponse.ReligonDescriptionEnglish = ds.Tables["Religion"].Rows[0]["DescriptionEnglish"].ToString();
                            }
                            if (columns.Contains("DescriptionArabic"))
                            {
                                deathResponse.ReligonDescriptionArabic = ds.Tables["Religion"].Rows[0]["DescriptionArabic"].ToString();
                            }
                        }
                        if (ds.Tables["Nationality"] != null && ds.Tables["Nationality"].Columns.Count > 0)
                        {
                            DataColumnCollection columns = ds.Tables["Nationality"].Columns;
                            if (columns.Contains("Code"))
                            {
                                deathResponse.NationalityCode = ds.Tables["Nationality"].Rows[0]["Code"].ToString();
                            }
                            if (columns.Contains("DescriptionEnglish"))
                            {
                                deathResponse.NationalityDescriptionEnglish = ds.Tables["Nationality"].Rows[0]["DescriptionEnglish"].ToString();
                            }
                            if (columns.Contains("DescriptionArabic"))
                            {
                                deathResponse.NationalityDescriptionArabic = ds.Tables["Nationality"].Rows[0]["DescriptionArabic"].ToString();
                            }
                        }
                        if (ds.Tables["MaritalStatus"] != null && ds.Tables["MaritalStatus"].Columns.Count > 0)
                        {
                            DataColumnCollection columns = ds.Tables["MaritalStatus"].Columns;
                            if (columns.Contains("Code"))
                            {
                                deathResponse.MaritalStatusCode = ds.Tables["MaritalStatus"].Rows[0]["Code"].ToString();
                            }
                            if (columns.Contains("DescriptionEnglish"))
                            {
                                deathResponse.MartialStatusDescriptionEnglish = ds.Tables["MaritalStatus"].Rows[0]["DescriptionEnglish"].ToString();
                            }
                            if (columns.Contains("DescriptionArabic"))
                            {
                                deathResponse.MartialStatusDescriptionArabic = ds.Tables["MaritalStatus"].Rows[0]["DescriptionArabic"].ToString();
                            }
                        }
                        if (ds.Tables["Occupation"] != null && ds.Tables["Occupation"].Columns.Count > 0)
                        {
                            DataColumnCollection columns = ds.Tables["Occupation"].Columns;
                            if (columns.Contains("Code"))
                            {
                                deathResponse.OccupationCode = ds.Tables["Occupation"].Rows[0]["Code"].ToString();
                            }
                            if (columns.Contains("DescriptionEnglish"))
                            {
                                deathResponse.OccupationDescriptionEnglish = ds.Tables["Occupation"].Rows[0]["DescriptionEnglish"].ToString();
                            }
                            if (columns.Contains("DescriptionArabic"))
                            {
                                deathResponse.OccupationDescriptionArabic = ds.Tables["Occupation"].Rows[0]["DescriptionArabic"].ToString();
                            }
                        }
                        if (ds.Tables["CertificatePlaceOfIssue"] != null && ds.Tables["CertificatePlaceOfIssue"].Columns.Count > 0)
                        {
                            DataColumnCollection columns = ds.Tables["CertificatePlaceOfIssue"].Columns;
                            if (columns.Contains("Code"))
                            {
                                deathResponse.CertificatePlaceOfIssueCode = ds.Tables["CertificatePlaceOfIssue"].Rows[0]["Code"].ToString();
                            }
                            if (columns.Contains("DescriptionEnglish"))
                            {
                                deathResponse.CertificatePlaceOfIssueDescriptionEnglish = ds.Tables["CertificatePlaceOfIssue"].Rows[0]["DescriptionEnglish"].ToString();
                            }
                            if (columns.Contains("DescriptionArabic"))
                            {
                                deathResponse.CertificatePlaceOfIssueDescriptionArabic = ds.Tables["CertificatePlaceOfIssue"].Rows[0]["DescriptionArabic"].ToString();
                            }
                        }
                        if (deathResponse.StatusCode == "0" && !String.IsNullOrEmpty(deathResponse.DateOfDeath))
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { deathResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["DeathCerCode"].ToString(), ConfigurationManager.AppSettings["DeathCer"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records Found";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["DeathCerCode"].ToString(), ConfigurationManager.AppSettings["DeathCer"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                }

            }
            catch (FaultException fex)
            {
                var fault = fex.CreateMessageFault();
                var doc = new XmlDocument();
                var innerdoc = new XmlDocument();
                var innersdoc = new XmlDocument();
                var nav = doc.CreateNavigator();
                string ResponseDescription = string.Empty;
                flag = 3;

                
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["DeathCerCode"].ToString(), ConfigurationManager.AppSettings["DeathCer"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["DeathCerCode"].ToString(), ConfigurationManager.AppSettings["DeathCer"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }


    }

}
