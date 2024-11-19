using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static MOCDIntegrations.Controllers.ADJDAttestationController;
using static MOCDIntegrations.Models.ADKFDetails;
using oAuthTokenGeneration = MOCDIntegrations.Models.oAuthTokenGeneration;

namespace MOCDIntegrations.Controllers
{
    public class ADGController : Controller
    {
        // GET: ADG
        public ActionResult Index()
        {
            return View();
        }
        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["ADG_Uri"].ToString(), ConfigurationManager.AppSettings["ADG_grant_type"].ToString(), ConfigurationManager.AppSettings["ADG_client_id"].ToString(), ConfigurationManager.AppSettings["ADG_client_secret"].ToString(), ConfigurationManager.AppSettings["ADG_scope"].ToString());
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
            try
            {
                string key = "9085455432226654";
                var helper = new AESHelper(key, "ADG$$4321$$AJHRD");
                if (EmiratesId.Length == 15)
                {
                    RestResponse response = ADGAPICALL(EmiratesId);//("784198932621877");
                    if (response.Content.Contains("\"P_CODE\":\"Success\""))
                    {
                        ADGDetails.Root objresp = JsonConvert.DeserializeObject<ADGDetails.Root>(response.Content);
                        if (objresp != null)
                        {
                            objresp.OutputParameters.P_TOTAL_SLARY = helper.decrypt(objresp.OutputParameters.P_TOTAL_SLARY).ToString();
                            objresp.OutputParameters.P_SOCIAL_INSURANCE = helper.decrypt(objresp.OutputParameters.P_SOCIAL_INSURANCE).ToString();

                            flag = 1;
                            json = JsonConvert.SerializeObject(new { objresp.OutputParameters, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADGCode"].ToString(), ConfigurationManager.AppSettings["ADG"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADGCode"].ToString(), ConfigurationManager.AppSettings["ADG"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADGCode"].ToString(), ConfigurationManager.AppSettings["ADG"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }
            }
            catch (FaultException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADGCode"].ToString(), ConfigurationManager.AppSettings["ADG"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (WebException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADGCode"].ToString(), ConfigurationManager.AppSettings["ADG"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static RestResponse ADGAPICALL(string emirateID)
        {
            string apiURL = ConfigurationManager.AppSettings["ADG_URL"].ToString();
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["ADG_user"].ToString() + ":" + ConfigurationManager.AppSettings["ADG_pass"].ToString()));

            //string securityKey = GenerateToken();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("Authorization", "Basic " + vToken);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["ADG_key"].ToString());
            request.AddHeader("Content-Type", "application/json");
            var body = "";

            body = @"{" + '\u0022' + "eid" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' + @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response;
        }
    }
}