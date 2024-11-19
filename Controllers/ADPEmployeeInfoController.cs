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
    public class ADPEmployeeInfoController : Controller
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
        private static RestResponse EmployeeInfoAPICall(string emirateID)
        {
            string userId = ConfigurationManager.AppSettings["adp_userId"].ToString();


            string appCode = ConfigurationManager.AppSettings["adp_appCode"].ToString();
            string apiURL = ConfigurationManager.AppSettings["adp_EmployeeInfo_api_url"].ToString();
            string userName = ConfigurationManager.AppSettings["adp_userName"].ToString();
            string password = ConfigurationManager.AppSettings["adp_password"].ToString();



            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            var body = "";
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["adp_userName"].ToString() + ":" + ConfigurationManager.AppSettings["adp_Pass"].ToString()));

            request.AddHeader("Authorization", "Basic " + vToken);

            request.AddHeader("Username", userId);
            request.AddHeader("Password", password);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["adp_gsbapikey"].ToString());
            request.AddHeader("AppCode", "0");

            request.AddHeader("Content-Type", "application/json");

            body = @"{" + '\u0022' + "NationalNo" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' +


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


        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            Models.ADPEmployeeInfo.Root root = null;
            List<Models.ADPEmployeeInfo.Root> lstResponse = null;
            RootError rootError = null;


            RestResponse response = null;
            JsonHelper objHelper = new JsonHelper();
            try
            {
                response = EmployeeInfoAPICall(EmiratesId);
                if (!response.Content.Contains("\"errorCode\": \"MOCD-504\""))
                {
                    root = JsonConvert.DeserializeObject<Models.ADPEmployeeInfo.Root>(response.Content);

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
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_EmployeeCode"].ToString(), ConfigurationManager.AppSettings["ADP_Employee"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_EmployeeCode"].ToString(), ConfigurationManager.AppSettings["ADP_Employee"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            if (root != null && root.OutParameters.Message == null)
            {
                flag = 1;
                lstResponse = new List<Models.ADPEmployeeInfo.Root>();
                lstResponse.Add(root);
                json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_EmployeeCode"].ToString(), ConfigurationManager.AppSettings["ADP_Employee"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet);
            }

            else
            {
                lstResponse = new List<Models.ADPEmployeeInfo.Root>();

                flag = 2;
                json = JsonConvert.SerializeObject(new { root.OutParameters, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADP_EmployeeCode"].ToString(), ConfigurationManager.AppSettings["ADP_Employee"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet);
            }

        }


    }
}