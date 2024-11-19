using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MOCDIntegrations.Models;
using MOCDstringegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static MOCDIntegrations.Models.OwnerDetails;

namespace MOCDIntegrations.Controllers
{
    public class MoHController : Controller
    {
        // GET: MoH
        public ActionResult Index()
        {
            return View("MoH");
        }

        private string LoadTransId()
        {
            return "MOCD_DQS_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }

        private string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["uri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["client_id"].ToString(), ConfigurationManager.AppSettings["client_secret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult Search(string index, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            DataTable dt = null; DataTable dt1 = null;

            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                JsonHelper objHelper = new JsonHelper();
                MOHDetails.DeathDetail mOHDetails = null;
                var input = new JavaScriptSerializer().Deserialize<DeathDetail.InputDetails>(postdata);

                string respList = CallWebService(input.EmiratesId, ConfigurationManager.AppSettings["moh_url"].ToString(), ConfigurationManager.AppSettings["moh_action"].ToString(), index, input.PassportNo);
                DataSet DataSetList = FillDataSet_FromXML(respList, "DeathDetailsList");

                if (DataSetList.Tables.Count > 0)
                {
                    if (DataSetList.Tables["DeathDetail"] != null)
                    {
                        dt = DataSetList.Tables["DeathDetail"];
                        if (DataSetList.Tables.Contains("DeathDetail"))
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                mOHDetails = new MOHDetails.DeathDetail();

                                mOHDetails.ApplicationStatusArabic = row["ApplicationStatusArabic"].ToString() != null ? row["ApplicationStatusArabic"].ToString() : string.Empty;


                                mOHDetails.ApplicationStatusCode = row["ApplicationStatusCode"].ToString() != null ? row["ApplicationStatusCode"].ToString() : string.Empty;
                                mOHDetails.ApplicationStatusEnglish = row["ApplicationStatusEnglish"].ToString() != null ? row["ApplicationStatusEnglish"].ToString() : string.Empty;
                                mOHDetails.City = row["City"].ToString() != null ? row["City"].ToString() : string.Empty;
                                mOHDetails.CityDescArabic = row["CityDescArabic"].ToString() != null ? row["CityDescArabic"].ToString() : string.Empty;
                                mOHDetails.CityDescEnglish = row["CityDescEnglish"].ToString() != null ? row["CityDescEnglish"].ToString() : string.Empty;
                                mOHDetails.DateOfBirth = row["DateOfBirth"].ToString() != null ? row["DateOfBirth"].ToString() : string.Empty;
                                mOHDetails.DateOfDeath = row["DateOfDeath"].ToString() != null ? row["DateOfDeath"].ToString() : string.Empty;
                                mOHDetails.Emirate = row["Emirate"].ToString() != null ? row["Emirate"].ToString() : string.Empty;
                                mOHDetails.EmirateDescArabic = row["EmirateDescArabic"].ToString() != null ? row["EmirateDescArabic"].ToString() : string.Empty;
                                mOHDetails.EmirateDescEnglish = row["EmirateDescEnglish"].ToString() != null ? row["EmirateDescEnglish"].ToString() : string.Empty;
                                mOHDetails.Gender = row["Gender"].ToString() != null ? row["Gender"].ToString() : string.Empty;
                                mOHDetails.HospitalNameArabic = row["HospitalNameArabic"].ToString() != null ? row["HospitalNameArabic"].ToString() : string.Empty;
                                mOHDetails.HospitalNameEnglish = row["HospitalNameEnglish"].ToString() != null ? row["HospitalNameEnglish"].ToString() : string.Empty;
                                mOHDetails.Nationality = row["Nationality"].ToString() != null ? row["Nationality"].ToString() : string.Empty;
                                mOHDetails.NationalityDescArabic = row["NationalityDescArabic"].ToString() != null ? row["NationalityDescArabic"].ToString() : string.Empty;
                                mOHDetails.NationalityDescEnglish = row["NationalityDescEnglish"].ToString() != null ? row["NationalityDescEnglish"].ToString() : string.Empty;
                                mOHDetails.PassportNumber = row["PassportNumber"].ToString() != null ? row["PassportNumber"].ToString() : string.Empty;
                                mOHDetails.PersonEmirateIDN = row["PersonEmirateIDN"].ToString() != null ? row["PersonEmirateIDN"].ToString() : string.Empty;
                                mOHDetails.PersonNameArabic = row["PersonNameArabic"].ToString() != null ? row["PersonNameArabic"].ToString() : string.Empty;
                                mOHDetails.PersonNameEnglish = row["PersonNameEnglish"].ToString() != null ? row["PersonNameEnglish"].ToString() : string.Empty;
                                mOHDetails.PlaceOfDeathArabic = row["PlaceOfDeathArabic"].ToString() != null ? row["PlaceOfDeathArabic"].ToString() : string.Empty;
                                mOHDetails.PlaceOfDeathEnglish = row["PlaceOfDeathEnglish"].ToString() != null ? row["PlaceOfDeathEnglish"].ToString() : string.Empty;
                                mOHDetails.QAIDNumber = row["QAIDNumber"].ToString() != null ? row["QAIDNumber"].ToString() : string.Empty;
                                mOHDetails.TransactionDate = row["TransactionDate"].ToString() != null ? row["TransactionDate"].ToString() : string.Empty;
                            }
                        }


                    }

                }


                if (!String.IsNullOrEmpty(mOHDetails.PersonEmirateIDN) || !String.IsNullOrEmpty(mOHDetails.PassportNumber))
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { mOHDetails, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                }
                else
                {
                    flag = 2;
                    json = JsonConvert.SerializeObject(new { flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                }

                //DeathQueryService.DeathQueryClient client = new DeathQueryService.DeathQueryClient();
                //using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                //{
                //    var httpRequestProperty = new HttpRequestMessageProperty();
                //    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                //    //Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MOH_Username"].ToString() + ":" + ConfigurationManager.AppSettings["MOH_Password"].ToString()));

                //    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                //    DeathQueryService.DeathQueryRequestModel mohSrv = new DeathQueryService.DeathQueryRequestModel();

                //    if (index == "2" || index == "3")
                //    {
                //        DeathQueryService.DocumentDetailsModel objDocDetails = new DeathQueryService.DocumentDetailsModel();
                //        objDocDetails.DocumentId = index == "2" ? input.EmiratesId : input.PassportNo;
                //        objDocDetails.DocumentType = IEUtils.ToInt(index);
                //        mohSrv.DocumentDetails = objDocDetails;
                //    }
                //    else if (index == "4")
                //    {
                //        DeathQueryService.NationalityDetailsModel objNatDetails = new DeathQueryService.NationalityDetailsModel();
                //        objNatDetails.DOB = input.DOB;
                //        objNatDetails.NationalityId = "234";// input.Nationality;
                //        objNatDetails.PersonName = input.Name;
                //        mohSrv.NationalityDetails = objNatDetails;
                //    }

                //    DeathQueryService.RequestHeader mohAuth = new DeathQueryService.RequestHeader();
                //    mohAuth.TransactionId = LoadTransId();
                //    mohAuth.ServiceProviderEntity = "MOCD";

                // DeathQueryService.DeathQueryResponseModel objResp = client.GetDeathDetails(ref mohAuth, mohSrv);
                //    flag = 1;
                //    if (objResp != null && objResp.ResponseCode == "MOH_SUC_001")
                //        json = JsonConvert.SerializeObject(new { objResp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                //    else if (objResp != null && objResp.ResponseCode == "MOH_SUC_002")
                //        json = JsonConvert.SerializeObject(new { objResp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                //    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<DeathQueryService.DeathQueryResponseModel>(objResp), ConfigurationManager.AppSettings["MOHCode"].ToString(), ConfigurationManager.AppSettings["MOH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                //    }
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                json = JsonConvert.SerializeObject(new { resp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, resp, ConfigurationManager.AppSettings["MOHCode"].ToString(), ConfigurationManager.AppSettings["MOH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["MOHCode"].ToString(), ConfigurationManager.AppSettings["MOH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }


            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private string CallWebService(string EmiratesId, string url, string actionname, string Documentnumber, string passwportNumber)
        {
            string soapEnvelopeXml = "";

            soapEnvelopeXml = CreateSoapEnvelope_getAttestationList(EmiratesId, Documentnumber, passwportNumber);


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
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return soapResult;
        }
        private string CreateSoapEnvelope_getAttestationList(string EmiratesId, string Documentnumber, string passwportNumber)
        {

            var Id = "";

            if (Documentnumber == "2")
            {
                Id = EmiratesId;
            }
            else if (Documentnumber == "3")
            {
                Id = passwportNumber;
            }
            string soapEnvelopeDocument = "";
            try
            {
                soapEnvelopeDocument =
              @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:nbb=""http://schemas.datacontract.org/2004/07/NBB.DeathQuery.Models"">
   <soapenv:Header>
      <tem:Header>
         <nbb:ServiceProviderEntity>MOCD</nbb:ServiceProviderEntity>
         <nbb:TransactionId>" + LoadTransId() + @"</nbb:TransactionId>
      </tem:Header>
   </soapenv:Header>
   <soapenv:Body>
      <tem:RequestMessage>
         <!--Optional:-->
         <tem:Body>
            <!--Optional:-->
            <nbb:DocumentDetails>
               <nbb:DocumentId>" + EmiratesId + @"</nbb:DocumentId>
               <nbb:DocumentType>" + Documentnumber + @"</nbb:DocumentType>
            </nbb:DocumentDetails>
           </tem:Body>
      </tem:RequestMessage>
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

                return "";
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
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
                {
                    requestWriter.Write(soapEnvelopeXml);
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
            catch (Exception ex)
            {
                throw ex;
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
                webRequest.Method = "POST";
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes("MOCDPrdConsumer3" + ":" + "mocdprduser$3"));
                webRequest.Headers["GSB-APIKey"] = "0c7950c0-4971-11e9-9d74-82b806c25279";

                webRequest.Headers["Authorization"] = "Basic " + authInfo;

                return webRequest;
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

                return null;
            }

        }
    }
}
