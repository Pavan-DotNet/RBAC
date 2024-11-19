using MOCDIntegrations.Models;
using MOCDIntegrations.Models.ContractDetails;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace MOCDIntegrations.Controllers
{
    public class ADPContractDetailsController : Controller
    {
        public ActionResult Index()
        {
            return View("");
        }

        private string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["gsb_adp_contractDetail_token_api_url"].ToString(), ConfigurationManager.AppSettings["gsb_adp_ClientId"].ToString(), ConfigurationManager.AppSettings["gsb_adp_SecreteKey"].ToString(), ConfigurationManager.AppSettings["client_secret"].ToString(), ConfigurationManager.AppSettings["gsb_adp_contractDetail_scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string LoadTransId()
        {
            return "MOCD_ICA_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }
        private RestResponse ContractDetailAPICall(string emirateID)
        {
            string userId = ConfigurationManager.AppSettings["gsb_adp_contractDetail_userId"].ToString();
            string appCode = ConfigurationManager.AppSettings["gsb_adp_contractDetail_appCode"].ToString();
            string apiURL = ConfigurationManager.AppSettings["gsb_adp_contractDetail_api_url"].ToString();
            string userName = ConfigurationManager.AppSettings["gsb_adp_ClientId"].ToString();
            string password = ConfigurationManager.AppSettings["gsb_adp_SecreteKey"].ToString();

            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            string securityKey = GenerateToken();
            var body = "";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(userName + ":" + password);
            string val = System.Convert.ToBase64String(plainTextBytes);
            request.AddHeader("Authorization", "Basic " + val);
            //request.AddHeader("Authorization", "Bearer " + securityKey);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Username", userName);
            request.AddHeader("Password", password);

            body = @"{" + '\u0022' + "NationalNo" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' + "," +

                                          '\u0022' + "UserId" + '\u0022' + ":" + '\u0022' + userId + '\u0022' + "," +

                                          '\u0022' + "AppCode" + '\u0022' + ":" + appCode +
                                      @"}";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response;
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
            MOCDIntegrations.Models.ContractDetails.Root root = null;
            MOCDIntegrations.Models.ContractDetails.RootError rootError = null;

            List<MOCDIntegrations.Models.ContractDetails.Root> lstResponse = null;
            RestResponse response = null;
            JsonHelper objHelper = new JsonHelper();
            try
            {
                response = ContractDetailAPICall(EmiratesId);
                if (!response.Content.Contains("\"errorCode\": \"MOCD-504\""))
                {
                    root = JsonConvert.DeserializeObject<MOCDIntegrations.Models.ContractDetails.Root>(response.Content);

                }
                else
                {
                    rootError = JsonConvert.DeserializeObject<MOCDIntegrations.Models.ContractDetails.RootError>(response.Content);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_ContractCode"].ToString(), ConfigurationManager.AppSettings["ADP_Contract"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_ContractCode"].ToString(), ConfigurationManager.AppSettings["ADP_Contract"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            if (root != null && root.OutParameters != null && !string.IsNullOrEmpty(root.OutParameters.NationalNo))
            {
                flag = 1;
                lstResponse = new List<Models.ContractDetails.Root>();
                lstResponse.Add(root);
                json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, objHelper.ConvertObjectToJSon<MOCDIntegrations.Models.ContractDetails.Root>(root), ConfigurationManager.AppSettings["ADP_ContractCode"].ToString(), ConfigurationManager.AppSettings["ADP_Contract"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else if (rootError.errorCode == "MOCD-504" || rootError != null)
            {
                flag = 2;
                string ResponseDescription = rootError.description;

                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, objHelper.ConvertObjectToJSon<MOCDIntegrations.Models.ContractDetails.Root>(root), ConfigurationManager.AppSettings["ADP_ContractCode"].ToString(), ConfigurationManager.AppSettings["ADP_Contract"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ResponseDescription = "No Matching Record Found";

                flag = 2;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, objHelper.ConvertObjectToJSon<MOCDIntegrations.Models.ContractDetails.Root>(root), ConfigurationManager.AppSettings["ADP_ContractCode"].ToString(), ConfigurationManager.AppSettings["ADP_Contract"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
    }
}