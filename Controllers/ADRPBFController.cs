using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace MOCDIntegrations.Controllers
{
    public class ADRPBFController : Controller
    {
        // GET: ADRPBF
        public ActionResult Index()
        {
            return View("");
        }

        public ActionResult SalaryDetails()
        {
            return View("");
        }

        public ActionResult Search(string index, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            DataSet DataSetList = null;
            string soapEnvelopeXml = "";
            DataTable dt = null;
            ADRPBFDetailsEmiratedId.MemberProfileInformation aDRPBFDetailsEmiratedId = new ADRPBFDetailsEmiratedId.MemberProfileInformation();
            ADRPBFDetailsFamily.MemberInformationResponse memberInformationResponse = new ADRPBFDetailsFamily.MemberInformationResponse();

            var input = new JavaScriptSerializer().Deserialize<ADRPBFDetails_New.ADRPBFDetailsRequestParams>(postdata);

            soapEnvelopeXml = CreateSoapEnvelope(input.EmiratesID, input.FNo + "-" + input.TNo, index);

            HttpWebRequest webRequest = CreateWebRequest(ConfigurationManager.AppSettings["ADRPBF_Url_GSB"].ToString(), "");
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
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(soapResult);
                    string jsonNew = JsonConvert.SerializeXmlNode(xDoc);
                    XmlNodeList result = xDoc.DocumentElement.ChildNodes;

                    if (index == "3")
                    {
                        DataSetList = FillDataSet_FromXML(soapResult, "GetSalaryDetails");
                        DataTable dt1 = DataSetList.Tables["Header"];

                        if (dt1.Rows[0]["ResponseCode"].ToString().Contains("ADRPBF_001"))
                        {
                            dt = DataSetList.Tables["MemberInformationResponse"];
                            foreach (DataRow row in dt.Rows)
                            {

                                if (row.Table.Columns.Contains("Address"))

                                {
                                    memberInformationResponse.Address = row["Address"].ToString();

                                }
                                if (row.Table.Columns.Contains("City"))
                                {
                                    memberInformationResponse.City = row["City"].ToString();

                                }
                                if (row.Table.Columns.Contains("City"))

                                {
                                    memberInformationResponse.DOB = row["City"].ToString();


                                }
                                if (row.Table.Columns.Contains("Email"))
                                {
                                    memberInformationResponse.Email = row["Email"].ToString();

                                }
                                if (row.Table.Columns.Contains("Emirate"))
                                {
                                    memberInformationResponse.Emirate = row["Emirate"].ToString();

                                }
                                if (row.Table.Columns.Contains("FamilyBookNo"))
                                {

                                    memberInformationResponse.FamilyBookNo = row["FamilyBookNo"].ToString();
                                }
                                if (row.Table.Columns.Contains("Fax"))
                                {

                                    memberInformationResponse.Fax = row["Fax"].ToString();
                                }
                                if (row.Table.Columns.Contains("MemberFullNameArabic"))
                                {
                                    memberInformationResponse.MemberFullNameArabic = row["MemberFullNameArabic"].ToString();
                                }
                                if (row.Table.Columns.Contains("MemberFullNameEnglish"))
                                {
                                    memberInformationResponse.MemberFullNameEnglish = row["MemberFullNameEnglish"].ToString();
                                }
                                if (row.Table.Columns.Contains("MemberType"))
                                {

                                    memberInformationResponse.MemberType = row["MemberType"].ToString();
                                }
                                if (row.Table.Columns.Contains("Mobile"))
                                {
                                    memberInformationResponse.Mobile = row["Mobile"].ToString();
                                }
                                if (row.Table.Columns.Contains("NationalID"))
                                {
                                    memberInformationResponse.NationalID = row["NationalID"].ToString();
                                }
                                if (row.Table.Columns.Contains("POBOX"))
                                {
                                    memberInformationResponse.POBOX = row["POBOX"].ToString();
                                }
                                if (row.Table.Columns.Contains("PensionAmount"))
                                {
                                    memberInformationResponse.PensionAmount = row["PensionAmount"].ToString();


                                }
                                if (row.Table.Columns.Contains("PensionNumber"))
                                {

                                    memberInformationResponse.PensionNumber = row["PensionNumber"].ToString();
                                }
                                if (row.Table.Columns.Contains("PensionStartDate"))
                                {

                                    memberInformationResponse.PensionStartDate = row["PensionStartDate"].ToString();
                                }
                                if (row.Table.Columns.Contains("Phone"))
                                {
                                    memberInformationResponse.Phone = row["Phone"].ToString();
                                }
                                if (row.Table.Columns.Contains("BeneficiaryInformation"))
                                {
                                    memberInformationResponse.BeneficiaryInformation = row["BeneficiaryInformation"].ToString();
                                }

                            }
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { memberInformationResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["ADRPBFCode"].ToString(), ConfigurationManager.AppSettings["ADRPBF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 3;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADRPBFCode"].ToString(), ConfigurationManager.AppSettings["ADRPBF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }

                    }
                    else
                    {
                        DataSetList = FillDataSet_FromXML(soapResult, "GetMemberProfile");
                        DataTable dt1 = DataSetList.Tables["Header"];

                        if (dt1.Rows[0]["ResponseCode"].ToString().Contains("ADRPBF_001"))
                        {
                            dt = DataSetList.Tables["memberProfileInformation"];

                            foreach (DataRow row in dt.Rows)
                            {

                                if (row.Table.Columns.Contains("ActiveStartDate"))
                                {
                                    aDRPBFDetailsEmiratedId.ActiveStartDate = row["ActiveStartDate"].ToString();

                                }
                                if (row.Table.Columns.Contains("ActiveemployerNameAr"))
                                {
                                    aDRPBFDetailsEmiratedId.ActiveemployerNameAr = row["ActiveemployerNameAr"].ToString();

                                }
                                if (row.Table.Columns.Contains("ActiveemployerNameEn"))
                                {
                                    aDRPBFDetailsEmiratedId.ActiveemployerNameEn = row["ActiveemployerNameEn"].ToString();

                                }
                                if (row.Table.Columns.Contains("ActivejobTitle"))
                                {
                                    aDRPBFDetailsEmiratedId.ActivejobTitle = row["ActivejobTitle"].ToString();

                                }
                                if (row.Table.Columns.Contains("Activesalary"))
                                {
                                    aDRPBFDetailsEmiratedId.Activesalary = row["Activesalary"].ToString();

                                }
                                if (row.Table.Columns.Contains("Address"))
                                {
                                    aDRPBFDetailsEmiratedId.Address = row["Address"].ToString();

                                }
                                if (row.Table.Columns.Contains("City"))
                                {
                                    aDRPBFDetailsEmiratedId.City = row["City"].ToString();

                                }
                                if (row.Table.Columns.Contains("DOB"))
                                {
                                    aDRPBFDetailsEmiratedId.DOB = row["DOB"].ToString();

                                }
                                if (row.Table.Columns.Contains("Email"))
                                {
                                    aDRPBFDetailsEmiratedId.Email = row["Email"].ToString();

                                }
                                if (row.Table.Columns.Contains("Emirate"))
                                {
                                    aDRPBFDetailsEmiratedId.Emirate = row["Emirate"].ToString();

                                }
                                if (row.Table.Columns.Contains("EmployerSector"))
                                {
                                    aDRPBFDetailsEmiratedId.EmployerSector = row["EmployerSector"].ToString();

                                }
                                if (row.Table.Columns.Contains("FamilyBookCityID"))
                                {
                                    aDRPBFDetailsEmiratedId.FamilyBookCityID = row["FamilyBookCityID"].ToString();

                                }
                                if (row.Table.Columns.Contains("FamilyBookNo"))
                                {
                                    aDRPBFDetailsEmiratedId.FamilyBookNo = row["FamilyBookNo"].ToString();

                                }
                                if (row.Table.Columns.Contains("Fax"))
                                {
                                    aDRPBFDetailsEmiratedId.Fax = row["Fax"].ToString();

                                }
                                if (row.Table.Columns.Contains("LastemployerName"))
                                {
                                    aDRPBFDetailsEmiratedId.LastemployerName = row["LastemployerName"].ToString();

                                }
                                if (row.Table.Columns.Contains("LastemployerNameEn"))
                                {
                                    aDRPBFDetailsEmiratedId.LastemployerNameEn = row["LastemployerNameEn"].ToString();

                                }
                                if (row.Table.Columns.Contains("LastendDate"))
                                {
                                    aDRPBFDetailsEmiratedId.LastendDate = row["LastendDate"].ToString();

                                }
                                if (row.Table.Columns.Contains("LastjobTitle"))
                                {
                                    aDRPBFDetailsEmiratedId.LastjobTitle = row["LastjobTitle"].ToString();

                                }
                                if (row.Table.Columns.Contains("Lastsalary"))
                                {
                                    aDRPBFDetailsEmiratedId.Lastsalary = row["Lastsalary"].ToString();

                                }
                                if (row.Table.Columns.Contains("LaststartDate"))
                                {
                                    aDRPBFDetailsEmiratedId.LaststartDate = row["LaststartDate"].ToString();

                                }

                                if (row.Table.Columns.Contains("MemberFullNameArabic"))
                                {
                                    aDRPBFDetailsEmiratedId.MemberFullNameArabic = row["MemberFullNameArabic"].ToString();

                                }
                                if (row.Table.Columns.Contains("MemberFullNameEnglish"))
                                {
                                    aDRPBFDetailsEmiratedId.MemberFullNameEnglish = row["MemberFullNameEnglish"].ToString();

                                }

                                if (row.Table.Columns.Contains("MemberType"))
                                {
                                    aDRPBFDetailsEmiratedId.MemberType = row["MemberType"].ToString();

                                }
                                if (row.Table.Columns.Contains("Mobile"))
                                {
                                    aDRPBFDetailsEmiratedId.Mobile = row["Mobile"].ToString();

                                }
                                if (row.Table.Columns.Contains("NationalID"))
                                {
                                    aDRPBFDetailsEmiratedId.NationalID = row["NationalID"].ToString();

                                }
                                if (row.Table.Columns.Contains("POBOX"))
                                {
                                    aDRPBFDetailsEmiratedId.POBOX = row["POBOX"].ToString();

                                }
                                if (row.Table.Columns.Contains("PensionAmount"))
                                {
                                    aDRPBFDetailsEmiratedId.PensionAmount = row["PensionAmount"].ToString();

                                }
                                if (row.Table.Columns.Contains("PensionLastDate"))
                                {
                                    aDRPBFDetailsEmiratedId.PensionLastDate = row["PensionLastDate"].ToString();

                                }
                                if (row.Table.Columns.Contains("PensionNumber"))
                                {
                                    aDRPBFDetailsEmiratedId.PensionNumber = row["PensionNumber"].ToString();

                                }
                                if (row.Table.Columns.Contains("PensionStartDate"))
                                {
                                    aDRPBFDetailsEmiratedId.PensionStartDate = row["PensionStartDate"].ToString();

                                }
                                if (row.Table.Columns.Contains("PensionStatus"))
                                {
                                    aDRPBFDetailsEmiratedId.PensionStatus = row["PensionStatus"].ToString();

                                }
                                if (row.Table.Columns.Contains("Phone"))
                                {
                                    aDRPBFDetailsEmiratedId.Phone = row["Phone"].ToString();

                                }
                            }
                            flag = 2;
                            json = JsonConvert.SerializeObject(new { aDRPBFDetailsEmiratedId, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["ADRPBFCode"].ToString(), ConfigurationManager.AppSettings["ADRPBF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 3;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADRPBFCode"].ToString(), ConfigurationManager.AppSettings["ADRPBF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADRPBFCode"].ToString(), ConfigurationManager.AppSettings["ADRPBF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


            }
            catch (Exception ex)
            {
                flag = 4;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADRPBFCode"].ToString(), ConfigurationManager.AppSettings["ADRPBF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }


            return Json(json, JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {
                throw ex;

            }

        }

        private static void InsertSoapEnvelopeIntoWebRequest(string soapEnvelopeXml, HttpWebRequest webRequest)
        {
            try
            {
                //using (Stream webStream = webRequest.GetRequestStream())
                //using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
                //{
                //    requestWriter.Write(soapEnvelopeXml);
                //}
                using (Stream webStream = webRequest.GetRequestStream())
                    try
                    {
                        using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
                        {
                            requestWriter.Write(soapEnvelopeXml);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;

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

        private static string CreateSoapEnvelope(string EmiratesId, string Family_no = default, string vFlag = default)
        {
            string soapEnvelopeDocument = "";
            try
            {
                if (vFlag == "3")
                {
                    //Family_no = "16740-101";
                    //EmiratesId = "784194374921099";
                    soapEnvelopeDocument = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ns=""http://schemas.datacontract.org/2004/07/"">
                                              <soapenv:Header>
                                              <tem:UserName>MOCD System Generated</tem:UserName>
                                              <tem:TransactionRefNo>MOCD_ADRPBF_20230526</tem:TransactionRefNo>
                                              <tem:ServiceKey>1000:LpFCm6LJpSXHoRYHx60Pi8zs6ZBjzb53:sf6vuiQXSE1QKccLoz+JJBbDpLXCplKM</tem:ServiceKey>
                                              <tem:ConsumerEntity>MOCD</tem:ConsumerEntity>
                                           </soapenv:Header>

                                            <soapenv:Body>
                                              <tem:MemberInformationReq>
                                                 <!--Optional:-->
                                                 <ns:FamilyBookNo>" + Family_no + @"</ns:FamilyBookNo>
                                                 <!--Optional:-->
                                                 <ns:NationalID>" + EmiratesId + @"</ns:NationalID>
                                              </tem:MemberInformationReq>
                                           </soapenv:Body>
                                        </soapenv:Envelope>";
                }
                else
                {
                    //EmiratesId = "784198402527323";
                    soapEnvelopeDocument =
                           @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                                                  <soapenv:Header>
                                                      <tem:UserName>MOCD System Generated</tem:UserName>
                                                      <tem:TransactionRefNo>MOCD_ADRPBF_20230526</tem:TransactionRefNo>
                                                      <tem:ServiceKey>1000:LpFCm6LJpSXHoRYHx60Pi8zs6ZBjzb53:sf6vuiQXSE1QKccLoz+JJBbDpLXCplKM</tem:ServiceKey>
                                                      <tem:ConsumerEntity>MOCD</tem:ConsumerEntity>
                                                  </soapenv:Header>
                                                  <soapenv:Body>
                                                      <tem:NationalID>" + EmiratesId + @"</tem:NationalID>
                                                  </soapenv:Body>
                                                </soapenv:Envelope>";
                }
                return soapEnvelopeDocument;
            }
            catch (WebException ex)
            {
                throw ex;
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
                webRequest.Headers.Add(@"GSB-APIKey:" + ConfigurationManager.AppSettings["ICA_ADRPBF_API_KEY"].ToString() + "");
                webRequest.Method = "POST";
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["ICAPersonalProfileUserName"].ToString() + ":" + ConfigurationManager.AppSettings["ICAPersonalProfilePassword"].ToString()));
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
