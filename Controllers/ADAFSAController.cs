using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json;
using MOCDIntegrations.Models;
using System.Web.Script.Serialization;
using static MOCDIntegrations.Models.ADAFSADetails;
using Newtonsoft.Json.Converters;



namespace MOCDIntegrations.Controllers
{
    public class ADAFSAController : Controller
    {
        // GET: ADAFSA
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            string DATA = "";
            DataTable dt = null;
            ADAFSAResponse.BeneficiaryResponse beneficiaryResponse = new ADAFSAResponse.BeneficiaryResponse();
            try
            {
                var input = new JavaScriptSerializer().Deserialize<ADAFSADetails.ADAFSADetailsRequestParams>(postdata);
                string soapResult = string.Empty;

                flag++;
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                DATA = input.EmiratesID.ToString(); // "784194025164941";//
                if (DATA.Length == 15)
                {
                    string respList = CallWebService(DATA, ConfigurationManager.AppSettings["ADAFSA_Url"].ToString(), "urn:ae:gov:abudhabi:RealEstateAndHousing:HousingTypes:1", "searchAppByEmirateId");
                    if (respList != "")
                    {
                        DataSet DataSetList = FillDataSet_FromXML(respList, "getAttestationList");
                        if (DataSetList.Tables.Count > 0)
                        {
                            if (DataSetList.Tables["beneficiaryResponse"] != null)
                            {
                                dt = DataSetList.Tables["beneficiaryResponse"];
                                foreach (DataRow row in dt.Rows)
                                {

                                    beneficiaryResponse.AgricultureCardNo = row["AgricultureCardNo"].ToString();
                                    beneficiaryResponse.OwnerNameAR = row["OwnerNameAR"].ToString();
                                    beneficiaryResponse.Status = row["Status"].ToString();
                                    beneficiaryResponse.PhoneNumber = row["PhoneNumber"].ToString();
                                    beneficiaryResponse.PhoneNumber2 = row["PhoneNumber2"].ToString();
                                    beneficiaryResponse.EmiratesId = row["EmiratesId"].ToString();
                                    beneficiaryResponse.EmiratesExpiryDate = row["EmiratesExpiryDate"].ToString();
                                    beneficiaryResponse.BankAccountName = row["BankAccountName"].ToString();
                                    beneficiaryResponse.BankAccountNumber = row["BankAccountNumber"].ToString();
                                    beneficiaryResponse.TransactionTypeAR = row["TransactionTypeAR"].ToString();
                                    beneficiaryResponse.TransferTypeAR = row["TransferTypeAR"].ToString();
                                    beneficiaryResponse.BankName = row["BankName"].ToString();
                                    beneficiaryResponse.BankBranch = row["BankBranch"].ToString();
                                    beneficiaryResponse.LastPaidMonth = row["LastPaidMonth"].ToString();
                                    beneficiaryResponse.LastPaidAmount = row["LastPaidAmount"].ToString();
                                }
                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = "No Matching Records available";
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADAFSACode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }

                            if (!String.IsNullOrEmpty(beneficiaryResponse.EmiratesId))
                            {
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { beneficiaryResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = "No Matching Records available";
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADAFSACode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                            }
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADAFSACode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }

                    }
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADAFSACode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADAFSACode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public static string CallWebService(string EmiratesId, string url, string actionname, string servicename, string Documentnumber = default)
        {
            string soapEnvelopeXml = "";
            soapEnvelopeXml = CreateSoapEnvelope(EmiratesId);

            HttpWebRequest webRequest = CreateWebRequest(url, actionname);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            string soapResult;
            WebResponse webResponse;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            try
            {
                using (webResponse = webRequest.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }
                    //Console.Write(soapResult);
                }
            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }

                throw ex;
            }

            return soapResult;
        }
        private static string CreateSoapEnvelope(string EmiratesId)
        {
            string soapEnvelopeDocument = "";
            try
            {
                soapEnvelopeDocument =
            @"<soapenv:Envelope xmlns:adaf=""http://www.oracle.com/AdafsaMocdService"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
               <soapenv:Body>
                  <adaf:GetPaymentsPerBeneficiary>
                     <adaf:EmiratesId>" + EmiratesId + @"</adaf:EmiratesId>
                     <adaf:BeneficiaryName></adaf:BeneficiaryName>
                  </adaf:GetPaymentsPerBeneficiary>
               </soapenv:Body>
            </soapenv:Envelope>";
                return soapEnvelopeDocument;
            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }

                throw ex;
            }

        }
        private static void InsertSoapEnvelopeIntoWebRequest(string soapEnvelopeXml, HttpWebRequest webRequest)
        {
            try
            {
                using (Stream webStream = webRequest.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
                {
                    requestWriter.Write(soapEnvelopeXml);
                }
                using (Stream webStream = webRequest.GetRequestStream())
                    try
                    {
                        using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
                        {
                            requestWriter.Write(soapEnvelopeXml);
                        }
                    }
                    catch (Exception)
                    {
                    }

            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }
            }

        }

        public static DataSet FillDataSet_FromXML(string xmlData, string servicename)
        {
            try
            {
                StringReader theReader = new StringReader(xmlData);
                DataSet theDataSet = new DataSet();
                if (theReader != null)
                {
                    theDataSet.ReadXml(theReader);
                    return theDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add(@"SOAPAction:" + action + "");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["ADAFSAclient_id"].ToString() + ":" + ConfigurationManager.AppSettings["ADAFSAclient_secret"].ToString()));
                webRequest.Headers["Authorization"] = "Basic " + authInfo;
                return webRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}