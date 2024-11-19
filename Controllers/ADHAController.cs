using MOCDIntegrations.Models;
using Newtonsoft.Json;
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
using System.Web.Script.Serialization;
using Newtonsoft.Json.Converters;
using static MOCDIntegrations.Models.ADHADetails.ADHAResponse;
using System.Collections;
using System.Web.Http.Description;
using static MOCDIntegrations.Models.ADAFSADetails.ADAFSAResponse;

namespace MOCDIntegrations.Controllers
{
    public class ADHAController : Controller
    {
        // GET: ADHA
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            string DATA = "";
            DataTable dt = null;
            try
            {
                DataTable DT_ADHA = new DataTable();

                List<Result> lstAJMResponse = new List<Result>();

                string soapResult = string.Empty;
                //  var input = new JavaScriptSerializer().Deserialize<ADHADetails.ADHADetailsRequestParams>(postdata);
                flag++;
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                DATA = EmiratesId; // "784199250659507";//
                if (DATA.Length == 15)
                {
                    string respList = CallWebService(DATA, ConfigurationManager.AppSettings["ADHA_Url"].ToString(), "urn:ae:gov:abudhabi:RealEstateAndHousing:HousingTypes:1", "searchAppByEmirateId");
                    if (respList != "")
                    {
                        if (!respList.Contains("<ns0:ResponseCode>10015"))
                        {
                            DataSet DataSetList = FillDataSet_FromXML(respList, "getAttestationList");
                            if (DataSetList.Tables.Count > 0)
                            {
                                var result = SaveADHADatat(DataSetList, DATA);

                                if (result != null)
                                {
                                    json = result;
                                    LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADHACode"].ToString(), ConfigurationManager.AppSettings["ADHA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                                }
                                else
                                {
                                    flag = 3;
                                    string ResponseDescription = "No Matching Records available";
                                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                    LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADHACode"].ToString(), ConfigurationManager.AppSettings["ADHA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                }
                            }
                        }
                        else
                        {
                            flag = 3;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADHACode"].ToString(), ConfigurationManager.AppSettings["ADHA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }


                    }
                    else
                    {
                        flag = 3;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADHACode"].ToString(), ConfigurationManager.AppSettings["ADHA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }

            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADHACode"].ToString(), ConfigurationManager.AppSettings["ADHA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADHACode"].ToString(), ConfigurationManager.AppSettings["ADHA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public static object dataSetToJSON(DataSet ds)
        {
            ArrayList root = new ArrayList();
            List<Dictionary<string, object>> table;
            Dictionary<string, object> data;

            foreach (DataTable dt in ds.Tables)
            {
                table = new List<Dictionary<string, object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    data = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        data.Add(col.ColumnName, dr[col]);
                    }
                    table.Add(data);
                }
                root.Add(table);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(root);
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

                // CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADHA", "CallWebService", "" + ex.Message.ToString() + "", EmiratesId.ToString(), soapEnvelopeXml, APIResponse);
                return "";
            }

            return soapResult;
        }
        private static string CreateSoapEnvelope(string EmiratesId)
        {
            string soapEnvelopeDocument = "";
            try
            {
                soapEnvelopeDocument =
                //@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:ae:gov:abudhabi:RealEstateAndHousing:HousingTypes:1"" xmlns:urn1=""urn:ae:gov:abudhabi:DigitalDocuments:AttestationTypes:1"">
                @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:ae:gov:abudhabi:RealEstateAndHousing:HousingTypes:1"" xmlns:urn1=""urn:ae:gov:abudhabi:RealEstateAndHousing:HousingTypes:1"">
                  <soapenv:Body>
                  <urn:SearchRequestByEmirate>
                     <urn:EmirateId>" + EmiratesId + @"</urn:EmirateId>
                     <urn:SystemCode>MOCD</urn:SystemCode>
                  </urn:SearchRequestByEmirate>
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

                //CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADHA", "CreateSoapEnvelope_getAttestationList", "" + ex.Message.ToString() + "", EmiratesId.ToString(), soapEnvelopeDocument, APIResponse);
                return "";
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
                // CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADHA", "InsertSoapEnvelopeIntoWebRequest", "" + ex.Message.ToString() + "", "", soapEnvelopeXml, APIResponse);
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
            catch (Exception ex)
            {
                // CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADHA", "FillDataSet_FromXML", "" + ex.Message.ToString() + "", "", "", "");
                return null;
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
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["ADHA_client_id"].ToString() + ":" + ConfigurationManager.AppSettings["ADHA_client_secret"].ToString()));
                webRequest.Headers["Authorization"] = "Basic " + authInfo;
                return webRequest;
            }
            catch (Exception ex)
            {
                //  CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADHA", "CreateWebRequest", "" + ex.Message.ToString() + "", "", "", "");
                return null;
            }

        }
        private static string SaveADHADatat(DataSet DRitem, string EmirateID)
        {
            var json = "";
            var flag = 1;
            ADHADetails.ADHAResponse.Item itemResponse = new ADHADetails.ADHAResponse.Item();
            ADHADetails.ADHAResponse.RequestedService requestServiceResponse = new ADHADetails.ADHAResponse.RequestedService();
            ADHADetails.ADHAResponse.ApprovedService approvedServiceResponse = new ADHADetails.ADHAResponse.ApprovedService();
            ADHADetails.ADHAResponse.ApplicationStage stageResponse = new ADHADetails.ADHAResponse.ApplicationStage();
            List<ADHADetails.ADHAResponse.ApplicantInfo> applicantInfoResponse = new List<ADHADetails.ADHAResponse.ApplicantInfo>();


            if (DRitem != null)
            {
                if (DRitem.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();

                    if (DRitem.Tables.Contains("Item"))
                    {
                        if (DRitem.Tables["Item"].Rows[0].Table.Columns.Contains("CbaNumber"))
                        {
                            itemResponse.CbaNumber = DRitem.Tables["Item"].Rows[0]["CbaNumber"].ToString();

                        }
                        itemResponse.ReferenceId = DRitem.Tables["Item"].Rows[0]["ReferenceId"].ToString();



                        if (DRitem.Tables["Item"].Rows[0].Table.Columns.Contains("SystemCode"))
                        {
                            itemResponse.SystemCode = DRitem.Tables["Item"].Rows[0]["SystemCode"].ToString();

                        }

                        if (DRitem.Tables["Item"].Rows[0].Table.Columns.Contains("DateOfCreation"))
                        {
                            itemResponse.DateOfCreation = DRitem.Tables["Item"].Rows[0]["DateOfCreation"].ToString();

                        }


                        if (DRitem.Tables["Item"].Rows[0].Table.Columns.Contains("item_Id"))
                        {
                            itemResponse.item_Id = DRitem.Tables["Item"].Rows[0]["item_Id"].ToString();

                        }
                        itemResponse.PrimaryApplicantFullNameArabic = DRitem.Tables["Item"].Rows[0]["PrimaryApplicantFullNameArabic"].ToString();


                        if (DRitem.Tables["Item"].Rows[0].Table.Columns.Contains("PrimaryApplicantFullNameEnglish"))
                        {
                            itemResponse.PrimaryApplicantFullNameEnglish = DRitem.Tables["Item"].Rows[0]["PrimaryApplicantFullNameEnglish"].ToString();

                        }

                        itemResponse.ResponseCode = DRitem.Tables["Item"].Rows[0]["ResponseCode"].ToString();
                        itemResponse.ResponseMessage = DRitem.Tables["Item"].Rows[0]["ResponseMessage"].ToString();
                        itemResponse.FGBDetails = DRitem.Tables["Item"].Rows[0]["FGBDetails"].ToString();
                        itemResponse.LastUpdateDate = DRitem.Tables["Item"].Rows[0]["LastUpdateDate"].ToString();
                        itemResponse.ServiceLocation = DRitem.Tables["Item"].Rows[0]["ServiceLocation"].ToString();
                        itemResponse.ApplicationStageStatus = DRitem.Tables["Item"].Rows[0]["ApplicationStageStatus"].ToString();
                        itemResponse.ApplicantType = DRitem.Tables["Item"].Rows[0]["ApplicantType"].ToString();
                        itemResponse.AppStageStatusTextEnglish = DRitem.Tables["Item"].Rows[0]["AppStageStatusTextEnglish"].ToString();
                        itemResponse.AppStageStatusTextArabic = DRitem.Tables["Item"].Rows[0]["AppStageStatusTextArabic"].ToString();
                        itemResponse.HasActiveInviteOnApp = Convert.ToBoolean(DRitem.Tables["Item"].Rows[0]["HasActiveInviteOnApp"]);
                        itemResponse.HasActiveWifeConsentOnApp = Convert.ToBoolean(DRitem.Tables["Item"].Rows[0]["HasActiveWifeConsentOnApp"]);
                        itemResponse.SearchResponseByEmirate_Id = DRitem.Tables["Item"].Rows[0]["SearchResponseByEmirate_Id"].ToString();


                        if (DRitem.Tables.Contains("RequestedService"))
                        {
                            requestServiceResponse.Name = DRitem.Tables["RequestedService"].Rows[0]["name"].ToString();
                            requestServiceResponse.RequestedService_Text = DRitem.Tables["RequestedService"].Rows[0]["RequestedService_Text"].ToString();

                        }
                        if (DRitem.Tables.Contains("ApprovedService"))
                        {
                            approvedServiceResponse.Name = DRitem.Tables["ApprovedService"].Rows[0]["name"].ToString();
                            approvedServiceResponse.ApprovedService_Text = DRitem.Tables["ApprovedService"].Rows[0]["ApprovedService_Text"].ToString();

                        }
                        if (DRitem.Tables.Contains("ApplicationStage"))
                        {
                            stageResponse.Name = DRitem.Tables["ApplicationStage"].Rows[0]["name"].ToString();
                            stageResponse.ApplicationStage_Text = DRitem.Tables["ApplicationStage"].Rows[0]["ApplicationStage_Text"].ToString();
                        }
                    }

                }
                if (DRitem.Tables.Contains("ApplicantsInfo"))
                {

                    foreach (DataRow DRApplicantInfo in DRitem.Tables["ApplicantInfo"].Rows)
                    {
                        applicantInfoResponse.Add(new ApplicantInfo
                        {
                            ApplicantNameAr = DRApplicantInfo["ApplicantNameEn"].ToString(),
                            ApplicantNameEn = DRApplicantInfo["ApplicantNameEn"].ToString(),
                            ApplicantType = DRApplicantInfo["ApplicantType"].ToString(),
                            EmiratesID = EmirateID,
                            PendingActions = DRApplicantInfo["PendingActions"].ToString()
                        });
                    }

                }
                json = JsonConvert.SerializeObject(new { itemResponse, requestServiceResponse, approvedServiceResponse, stageResponse, applicantInfoResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                return json;
            }
            return null;

        }
    }


}
