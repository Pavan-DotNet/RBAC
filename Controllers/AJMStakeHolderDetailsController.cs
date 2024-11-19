using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Text;

namespace MOCDIntegrations.Controllers
{
    public class AJMStakeHolderDetailsController : Controller
    {
        // GET: AJMStakeHolderDetails
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            int flag = 0;
            var json = "";
            try
            {

                RestResponse response = AJMStakeAPICALL(EmiratesId);
                if (!response.Content.Contains("Message: \"Success\""))
                {

                    AJMStakeHoldersDetails.AJMStakeHolderDetailsModel.Root objresp = JsonConvert.DeserializeObject<AJMStakeHoldersDetails.AJMStakeHolderDetailsModel.Root>(response.Content);

                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Record Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static RestResponse AJMStakeAPICALL(string emirateID )
        {
            string apiURL = ConfigurationManager.AppSettings["AJMStake_URL"].ToString();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["AJMRes_Key"].ToString());
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["AJMRes_User"].ToString() + ":" + ConfigurationManager.AppSettings["AJMRes_Pass"].ToString()));
            request.AddHeader("Authorization", "Basic " + vToken);
            request.AddHeader("Accept", "application/json");
            var body = @"{" + '\u0022' + "IdentityNumber" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' + @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response;
        }



        public ActionResult ViewMoreDetails(string transactionId, string UserAgent)
        {
            int flag = 0;
            var json = "";
            try
            {

                RestResponse response = AJMLiscenceAPICALL(transactionId);
                if (!response.Content.Contains("Message: \"Success\""))
                {

                    AJMStakeHoldersDetails.AJMLiscenceDetails.Root objresp = JsonConvert.DeserializeObject<AJMStakeHoldersDetails.AJMLiscenceDetails.Root>(response.Content);

                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(transactionId, json, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Record Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(transactionId, ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(transactionId, ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(transactionId, ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static RestResponse AJMLiscenceAPICALL(string emirateID)
        {

            string apiURL = ConfigurationManager.AppSettings["AJMLiscence_URL"].ToString();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["AJMRes_Key"].ToString());
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["AJMRes_User"].ToString() + ":" + ConfigurationManager.AppSettings["AJMRes_Pass"].ToString()));
            request.AddHeader("Authorization", "Basic " + vToken);
            request.AddHeader("Accept", "application/json");
            var body = "";

            body = @"{" + '\u0022' + "LicenseNumber" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' + @"}";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response;
        }

        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["AJMShare_uri"].ToString(), ConfigurationManager.AppSettings["AJMShare_grant_type"].ToString(), ConfigurationManager.AppSettings["AJMShare_client_id"].ToString(), ConfigurationManager.AppSettings["AJMShare_client_secret"].ToString(), ConfigurationManager.AppSettings["AJMShare_scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}