using MOCDIntegrations.Models;
using MOCDIntegrations.Models.ContractDetails;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace MOCDIntegrations.Controllers
{
    public class ADPVehicleCountController : Controller
    {
        public ActionResult Index()
        {
            return View("");
        }
        public static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["gsb_adp_VehicleCount_token_api_url"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["adp_userName"].ToString(), ConfigurationManager.AppSettings["adp_password"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static RestResponse VehicleCountAPICall(string emirateID)
        {
            string userId = ConfigurationManager.AppSettings["adp_userId"].ToString();
            string appCode = ConfigurationManager.AppSettings["adp_appCode"].ToString();
            string apiURL = ConfigurationManager.AppSettings["adp_api_url"].ToString();
            string userName = ConfigurationManager.AppSettings["adp_userName"].ToString();
            string password = ConfigurationManager.AppSettings["adp_password"].ToString();



            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            string securityKey = GenerateToken();
            var body = "";
            request.AddHeader("Authorization", "Bearer " + securityKey);
            request.AddHeader("Username", userName);
            request.AddHeader("Password", password);
            request.AddHeader("AppCode", "0");
            request.AddHeader("Content-Type", "application/json");

            body = @"{" + '\u0022' + "NationalNo" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' + "," +

                                            '\u0022' + "UserId" + '\u0022' + ":" + '\u0022' + userId + '\u0022' + "," +

                                            '\u0022' + "AppCode" + '\u0022' + ":" + appCode +
                                        @"}";



            request.AddParameter("application/json", body, ParameterType.RequestBody);
            string jsonString = JsonConvert.SerializeObject(request); //JsonConverter.Serialize(request);
            RestResponse response = client.Execute(request);
            return response;
        }

        private string LoadTransId()
        {
            return "MOCD_ICA_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }

        public HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["PP_URL"].ToString());
            webRequest.Headers.Add("SOAPAction", "http://id.gov.ae/ws/PersonInquiryService/personInquiry");
            webRequest.ContentType = "text/xml;charset=\"UTF-8\"";
            webRequest.Accept = "gzip,deflate";
            webRequest.Method = "POST";
            webRequest.UserAgent = "Apache-HttpClient/4.1.1 (java 1.5)";

            return webRequest;
        }

        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            Models.VehicleCount.Root root = null;
            List<Models.VehicleCount.Root> lstResponse = null;
            MOCDIntegrations.Models.VehicleCount.RootError rootError = null;

            RestResponse response = null;
            JsonHelper objHelper = new JsonHelper();
            try
            {
                response = VehicleCountAPICall(EmiratesId);
                if (!response.Content.Contains("\"errorCode\": \"MOCD-500\","))
                {
                    root = JsonConvert.DeserializeObject<Models.VehicleCount.Root>(response.Content);
                    root.OutParameters.NationalNo = EmiratesId;
                }
                else
                {
                    rootError = JsonConvert.DeserializeObject<MOCDIntegrations.Models.VehicleCount.RootError>(response.Content);

                }

            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_VehicleCode"].ToString(), ConfigurationManager.AppSettings["ADP_Vehicle"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_VehicleCode"].ToString(), ConfigurationManager.AppSettings["ADP_Vehicle"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            if (root != null && root.OutParameters != null)
            {
                flag = 1;
                lstResponse = new List<Models.VehicleCount.Root>();
                lstResponse.Add(root);
                json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_VehicleCode"].ToString(), ConfigurationManager.AppSettings["ADP_Vehicle"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else if (rootError != null  || rootError.errorCode.Contains("\"errorCode\": \"MOCD-500\","))
            {
                flag = 2;
                string ResponseDescription = rootError.errorMsg;

                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_VehicleCode"].ToString(), ConfigurationManager.AppSettings["ADP_Vehicle"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else
            {
                flag = 2;
                string ResponseDescription = "No Matching Record Found";
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_VehicleCode"].ToString(), ConfigurationManager.AppSettings["ADP_Vehicle"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet);
            }

        }


    }
}