using Newtonsoft.Json;
using RestSharp;
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
using MOCDIntegrations.Models;
using System.Configuration;
using Newtonsoft.Json.Converters;
using System.Data.Odbc;
using System.ServiceModel;
using MOCDIntegrations.Models.MOEcNERBL;
using static MOCDIntegrations.Models.DPPRISONSTATUSDetails;

namespace MOCDIntegrations.Controllers
{
    public class MOEcNERBLDController : Controller
    {
        // GET: ADDC
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string Type, string Data, string UserAgent )
        {
            var json = "";
            int flag = 0;
            string DATA = Data;
            ADDCDetails.Return objResponse = new ADDCDetails.Return();
            try
            {

                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                //DATA = EmiratesId; // "784194025164941";//
                if (Type == "eid" && DATA.Length == 15)
                {
                    RestResponse response = CallWebService(DATA, ConfigurationManager.AppSettings["MOEsNERBL_Url"].ToString(), Type);
                    if (response != null)
                    {
                        MOEcOwnerDetail root = JsonConvert.DeserializeObject<MOEcOwnerDetail>(response.Content);
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { root, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(DATA, json, ConfigurationManager.AppSettings["MOEsNERBLCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {

                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(DATA, ResponseDescription, ConfigurationManager.AppSettings["MOEsNERBLCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }
                else //if (Type == "eid" && DATA.Length == 15)
                {
                    RestResponse response = CallWebService(DATA, ConfigurationManager.AppSettings["MOEsNERBL_Url"].ToString(), Type);
                    MOEcOwnerDetail root = JsonConvert.DeserializeObject<MOEcOwnerDetail>(response.Content);
                    if (root.EmiratesId != null)
                    {
                        
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { root, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(DATA, json, ConfigurationManager.AppSettings["MOEsNERBLCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {

                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(DATA, ResponseDescription, ConfigurationManager.AppSettings["MOEsNERBLCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }

            }

            catch (FaultException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(DATA, ResponseDescription, ConfigurationManager.AppSettings["MOEsNERBLCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (WebException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(DATA, ResponseDescription, ConfigurationManager.AppSettings["MOEsNERBLCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public static RestResponse CallWebService(string Data, string url, string type)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                string securityKey = GenerateToken();

                string apiURL = "";
                if (type == "eid")
                {
                    apiURL = String.Format(url, "NA&", "NA&", "NA&", "NA&", Data);
                }
                else if (type == "pass")
                {
                    apiURL = String.Format(url, "NA&", "NA&", "NA&", Data+"&", "NA&");

                }
                else if (type == "uid")
                {
                    apiURL = String.Format(url, "NA&", "NA&", Data + "&", "NA&", "NA&");

                }
                else if (type == "pne")
                {
                    apiURL = String.Format(url, "NA&", Data + "&", "NA&", "NA&", "NA&");

                }
                else if (type == "pna")
                {
                    apiURL = String.Format(url, Data + "&", "NA&", "NA&", "NA&", "NA&");

                }
                var options = new RestClientOptions(apiURL)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(apiURL, Method.Get);
                string userName = ConfigurationManager.AppSettings["MOEcNER_client_id"].ToString();
                string password = ConfigurationManager.AppSettings["MOEcNER_client_secret"].ToString();
                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(userName + ":" + password)));
                RestResponse response = client.Execute(request);
                return response;
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
        public static TokenDetails GenerateAndGetToken(string uri, string grant_type, string client_id, string client_secret, string scope)
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
        private static string GenerateToken()
        {
            try
            {
                string Result = GenerateAndGetToken(ConfigurationManager.AppSettings["ShjUniversity_uri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["client_id"].ToString(), ConfigurationManager.AppSettings["client_secret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString()).access_token;
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}