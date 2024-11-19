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
using System.Text;
using System.Web.Mvc;
using System.Xml;
using static MOCDIntegrations.Models.ADJDAttestation;

namespace MOCDIntegrations.Controllers
{
    public class ADJDAttestationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Search(string EmiratesId, string UserAgent)
        {
            string DATA = "";
            var json = "";
            var json1 = "";
            XmlDocument doc = null;

            List<Result> lstAJMResponse = new List<Result>();

            string soapResult = string.Empty;
            int flag = 0;
            try
            {
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                DATA = EmiratesId;
                if (DATA.Length == 15)
                {

                    string respList = CallWebService(DATA, ConfigurationManager.AppSettings["adjd_a_Url"].ToString(), "urn:ae:gov:abudhabi:DigitalDocuments:AttestationTypes:1", "getAttestationList");
                    if (respList != "")
                    {
                        DataSet DataSetList = FillDataSet_FromXML(respList, "getAttestationList");
                        if (DataSetList.Tables.Count > 0)
                        {

                            doc = new XmlDocument();
                            doc.LoadXml(respList);

                            string jsonNew = JsonConvert.SerializeXmlNode(doc);
                            if (jsonNew.Contains("\"ns2:Contents\":{"))
                            {
                                jsonNew = jsonNew.Replace("\"ns2:Contents\":{", "\"ns2:Contents\":[{");
                                jsonNew = jsonNew.Replace(":\"false\"}}}}}}", ":\"false\"}}]}}}}");
                            }
                            ADJDContent.Root respon = JsonConvert.DeserializeObject<ADJDContent.Root>(jsonNew);
                            if (respon?.soapenvEnvelope?.soapenvBody?.ns2AttestationListResponse?.ns2Contents != null && respon?.soapenvEnvelope?.soapenvBody?.ns2AttestationListResponse?.ns2Contents.Count > 0)
                            {
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { respon.soapenvEnvelope?.soapenvBody?.ns2AttestationListResponse.ns2Contents, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                json = json.Replace("ns0:", "");
                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = "No Matching Records available";
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADJDATCode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }

                        }

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADJDATCode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(json, json1, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ViewMoreDetails(string documentId, string UserAgent)
        {
            string DATA = "";
            var json = "";
            var json1 = "";
            XmlDocument doc = null;
            List<Result> lstAJMResponse = new List<Result>();

            string soapResult = string.Empty;
            int flag = 0;
            try
            {
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                DATA = documentId;

                if (DATA.Length > 0)
                {


                    string respDetails = CallWebService(DATA, ConfigurationManager.AppSettings["adjd_a_Url"].ToString(), "urn:ae:gov:abudhabi:DigitalDocuments:AttestationTypes:1", "");



                    if (respDetails != "")
                    {
                        DataSet DataSetDetails = FillDataSet_FromXML(respDetails, "getAttestationDetails");
                        if (DataSetDetails.Tables.Count > 0)
                        {
                            doc = new XmlDocument();
                            doc.LoadXml(respDetails);
                            string json11 = JsonConvert.SerializeXmlNode(doc);
                            if (json11.Contains("\"ns0:PartyDetails\":{"))
                            {
                                json11 = json11.Replace("\"ns0:PartyDetails\":{", "\"ns0:PartyDetails\":[{");
                                json11 = json11.Replace("}}}}}}}", "}}]}}}}}");
                            }
                            ADJDCompleteDetails.Root respon = JsonConvert.DeserializeObject<ADJDCompleteDetails.Root>(json11);

                            if (respon?.soapenvEnvelope?.soapenvBody?.ns2AttestationDetailsResponse?.ns2Contents != null)
                            {
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { respon.soapenvEnvelope?.soapenvBody?.ns2AttestationDetailsResponse.ns2Contents, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                json = json.Replace("ns2:", "");
                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = "No Matching Records available";
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(documentId, ResponseDescription, ConfigurationManager.AppSettings["ADJDATCode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(json, json1, JsonRequestBehavior.AllowGet);

        }

        public static string CallWebService(string EmiratesId, string url, string actionname, string servicename, string Documentnumber = default)
        {
            string soapEnvelopeXml = "";
            if (Documentnumber == null)
            {
                Documentnumber = "";
            }
            if (servicename == "getAttestationList")
                soapEnvelopeXml = CreateSoapEnvelope_getAttestationList(EmiratesId, Documentnumber);

            else
                soapEnvelopeXml = CreateSoapEnvelope_getAttestationDetails(EmiratesId);

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
        private static string CreateSoapEnvelope_getAttestationList(string EmiratesId, string Documentnumber = default)
        {
            string soapEnvelopeDocument = "";
            try
            {
                soapEnvelopeDocument =
              @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:ae:gov:abudhabi:DigitalDocuments:AttestationMessages:1"" xmlns:urn1=""urn:ae:gov:abudhabi:DigitalDocuments:AttestationTypes:1"">
   <soapenv:Header>
      <urn:RequestTrnHeader>
         <!--Optional:-->
         <urn1:TransactionId>9</urn1:TransactionId>
         <!--Optional:-->
         <urn1:EntityCode>MOCD</urn1:EntityCode>
      </urn:RequestTrnHeader>
   </soapenv:Header>
   <soapenv:Body>
      <urn:AttestationListRequest>
         <!--You have a CHOICE of the next 2 items at this level-->
         <urn:EmiratesIDNumber>" + EmiratesId + @"</urn:EmiratesIDNumber>
         <urn:DocumentNumber></urn:DocumentNumber>
      </urn:AttestationListRequest>
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

        private static string CreateSoapEnvelope_getAttestationDetails(string DocumentRefNumber)
        {
            string soapEnvelopeDocument = "";
            try
            {
                soapEnvelopeDocument =
                    @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:ae:gov:abudhabi:DigitalDocuments:AttestationMessages:1"" xmlns:urn1=""urn:ae:gov:abudhabi:DigitalDocuments:AttestationTypes:1"">
   <soapenv:Header>
      <urn:RequestTrnHeader>
         <!--Optional:-->
         <urn1:TransactionId>MOCD_ADJD_1212123</urn1:TransactionId>
         <!--Optional:-->
         <urn1:EntityCode>MOCD</urn1:EntityCode>
      </urn:RequestTrnHeader>
   </soapenv:Header>
   <soapenv:Body>
      <urn:AttestationDetailsRequest>
         <urn:DocumentReferenceNumber>" + DocumentRefNumber + @"</urn:DocumentReferenceNumber>
      </urn:AttestationDetailsRequest>
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
                webRequest.Headers.Add(@"GSB-APIKey:" + ConfigurationManager.AppSettings["Adjd_key"].ToString() + "");
                webRequest.Method = "POST";
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["Adjd_username"].ToString() + ":" + ConfigurationManager.AppSettings["Adjd_password"].ToString()));
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

        public class oAuthTokenGeneration
        {

            public TokenDetails GenerateToken(string uri, string grant_type, string client_id, string client_secret, string scope)
            {
                TokenDetails objToken = new TokenDetails();
                StringBuilder tokenUri = new StringBuilder();
                tokenUri.Append(uri);
                tokenUri.AppendFormat("?grant_type=" + grant_type + "&client_id=" + client_id + "&client_secret=" + client_secret + "&scope=" + scope);
                String responseBody;

                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUri.ToString());
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader responseStream = new StreamReader(response.GetResponseStream());
                    responseBody = responseStream.ReadToEnd();
                    objToken = JsonConvert.DeserializeObject<TokenDetails>(responseBody);
                    return objToken;
                }
                catch (WebException ex)
                {
                    StreamReader responseStream = new StreamReader(ex.Response.GetResponseStream());
                    responseBody = responseStream.ReadToEnd();
                    objToken = JsonConvert.DeserializeObject<TokenDetails>(responseBody);
                    return objToken;
                }
            }
        }
        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["adjd_a_uri"].ToString(), ConfigurationManager.AppSettings["adjd_a_grant_type"].ToString(), ConfigurationManager.AppSettings["adjd_a_client_id"].ToString(), ConfigurationManager.AppSettings["adjd_a_client_secret"].ToString(), ConfigurationManager.AppSettings["adjd_a_scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}