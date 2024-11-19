using System;
using System.IO;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using MOCDIntegrations.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Xml;
using System.Data;
using DataSet = System.Data.DataSet;


namespace MOCDIntegrations.Controllers
{
    public class ADRPBF_NewController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            return View("ADPRBF");
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
                                memberInformationResponse.Address = row["Address"].ToString();
                                memberInformationResponse.City = row["City"].ToString();
                                memberInformationResponse.DOB = row["DOB"].ToString();
                                memberInformationResponse.Email = row["Email"].ToString();
                                memberInformationResponse.Emirate = row["Emirate"].ToString();
                                memberInformationResponse.FamilyBookNo = row["FamilyBookNo"].ToString();
                                memberInformationResponse.Fax = row["Fax"].ToString();
                                memberInformationResponse.MemberFullNameArabic = row["MemberFullNameArabic"].ToString();
                                memberInformationResponse.MemberFullNameEnglish = row["MemberFullNameEnglish"].ToString();
                                memberInformationResponse.MemberType = row["MemberType"].ToString();
                                memberInformationResponse.Mobile = row["Mobile"].ToString();
                                memberInformationResponse.NationalID = row["NationalID"].ToString();
                                memberInformationResponse.POBOX = row["POBOX"].ToString();
                                memberInformationResponse.PensionAmount = row["PensionAmount"].ToString();
                                memberInformationResponse.PensionNumber = row["PensionNumber"].ToString();
                                memberInformationResponse.PensionStartDate = row["PensionStartDate"].ToString();
                                memberInformationResponse.Phone = row["Phone"].ToString();
                                memberInformationResponse.BeneficiaryInformation = row["BeneficiaryInformation"].ToString();

                            }
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { memberInformationResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

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

                                aDRPBFDetailsEmiratedId.ActiveStartDate = row["ActiveStartDate"].ToString();
                                aDRPBFDetailsEmiratedId.ActiveemployerNameAr = row["ActiveemployerNameAr"].ToString();
                                aDRPBFDetailsEmiratedId.ActiveemployerNameEn = row["ActiveemployerNameEn"].ToString();
                                aDRPBFDetailsEmiratedId.ActivejobTitle = row["ActivejobTitle"].ToString();
                                aDRPBFDetailsEmiratedId.Activesalary = row["Activesalary"].ToString();
                                aDRPBFDetailsEmiratedId.Address = row["Address"].ToString();
                                aDRPBFDetailsEmiratedId.City = row["City"].ToString();
                                aDRPBFDetailsEmiratedId.DOB = row["DOB"].ToString();
                                aDRPBFDetailsEmiratedId.Email = row["Email"].ToString();
                                aDRPBFDetailsEmiratedId.Emirate = row["Emirate"].ToString();
                                aDRPBFDetailsEmiratedId.EmployerSector = row["EmployerSector"].ToString();
                                aDRPBFDetailsEmiratedId.FamilyBookCityID = row["FamilyBookCityID"].ToString();
                                aDRPBFDetailsEmiratedId.FamilyBookNo = row["FamilyBookNo"].ToString();
                                aDRPBFDetailsEmiratedId.Fax = row["Fax"].ToString();
                                aDRPBFDetailsEmiratedId.LastemployerName = row["LastemployerName"].ToString();
                                aDRPBFDetailsEmiratedId.LastemployerNameEn = row["LastemployerNameEn"].ToString();
                                aDRPBFDetailsEmiratedId.LastendDate = row["LastendDate"].ToString();
                                aDRPBFDetailsEmiratedId.LastjobTitle = row["LastjobTitle"].ToString();
                                aDRPBFDetailsEmiratedId.Lastsalary = row["Lastsalary"].ToString();
                                aDRPBFDetailsEmiratedId.LaststartDate = row["LaststartDate"].ToString();
                                aDRPBFDetailsEmiratedId.MemberFullNameArabic = row["MemberFullNameArabic"].ToString();
                                aDRPBFDetailsEmiratedId.MemberFullNameEnglish = row["MemberFullNameEnglish"].ToString();
                                aDRPBFDetailsEmiratedId.MemberType = row["MemberType"].ToString();
                                aDRPBFDetailsEmiratedId.Mobile = row["Mobile"].ToString();
                                aDRPBFDetailsEmiratedId.NationalID = row["NationalID"].ToString();
                                aDRPBFDetailsEmiratedId.POBOX = row["POBOX"].ToString();
                                aDRPBFDetailsEmiratedId.PensionAmount = row["PensionAmount"].ToString();
                                aDRPBFDetailsEmiratedId.PensionLastDate = row["PensionLastDate"].ToString();
                                aDRPBFDetailsEmiratedId.PensionNumber = row["PensionNumber"].ToString();
                                aDRPBFDetailsEmiratedId.PensionStartDate = row["PensionStartDate"].ToString();
                                aDRPBFDetailsEmiratedId.PensionStatus = row["PensionStatus"].ToString();
                                aDRPBFDetailsEmiratedId.Phone = row["Phone"].ToString();

                            }
                            flag = 2;
                            json = JsonConvert.SerializeObject(new { aDRPBFDetailsEmiratedId, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

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
                flag = 4;

                var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = resp.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                // LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["Index"].ToString(), ConfigurationManager.AppSettings["Index"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


            }
            catch (Exception ex)
            {
                flag = 4;
                var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = resp.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SEDD"].ToString(), ConfigurationManager.AppSettings["SSSD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }


            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            // JSONString = JSONConvert.SerializeObject(table);
            return JSONString;
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
                //  CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADRPBF", "FillDataSet_FromXML", "" + ex.Message.ToString() + "", "", "", xmlData);
                return null;
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
                    catch (Exception ex)
                    {
                        //  CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADED", "Insert Into Soap", "" + ex.Message.ToString() + "", null, null, null);

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
                // CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADED", "InsertSoapEnvelopeIntoWebRequest", "" + ex.Message.ToString() + "", "", soapEnvelopeXml, APIResponse);
            }

        }

        private static string CreateSoapEnvelope(string EmiratesId, string Family_no = default, string vFlag = default)
        {
            string soapEnvelopeDocument = "";
            try
            {
                if (vFlag == "3")
                {
                    Family_no = "16740-101";
                    EmiratesId = "784194374921099";
                    soapEnvelopeDocument =
                    @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ns=""http://schemas.datacontract.org/2004/07/"">
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
                          <soapenv:Body>
                              <tem:NationalID>" + EmiratesId + @"</tem:NationalID>
                          </soapenv:Body>
                        </soapenv:Envelope>";
                }
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
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADDEDCode"].ToString(), ConfigurationManager.AppSettings["ADDED"].ToString(), DateTime.Now.ToString(), string.Empty, null, null);
                //CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADRPBF", "CreateSoapEnvelope", "" + ex.Message.ToString() + "", EmiratesId.ToString(), soapEnvelopeDocument, APIResponse);
                return "";
            }

        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add(@"SOAPAction:" + action + "");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Headers.Add(@"GSB-APIKey:" + ConfigurationManager.AppSettings["ICA_ADRPBF_API_KEY"].ToString() + "");
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["ICAPersonalProfileUserName"].ToString() + ":" + ConfigurationManager.AppSettings["ICAPersonalProfilePassword"].ToString()));
                webRequest.Headers["Authorization"] = "Basic " + authInfo;
                return webRequest;
            }
            catch (Exception ex)
            {
                var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = resp;
                LogIntegrationDetails.LogSerilog(url, ResponseDescription, ConfigurationManager.AppSettings["ADDEDCode"].ToString(), ConfigurationManager.AppSettings["ADDED"].ToString(), DateTime.Now.ToString(), string.Empty, null, null);

                return null;
            }

        }
    }
}