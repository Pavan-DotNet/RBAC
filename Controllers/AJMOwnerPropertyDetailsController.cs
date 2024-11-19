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
    public class AJMOwnerPropertyDetailsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string postdata, string UserAgent, string Index)
        {
            int flag = 0;
            var json = "";
            try
            {
                var input = new JavaScriptSerializer().Deserialize<AJMStakeHoldersDetails.AJMStakeHoldersRequestParams>(postdata);

                RestResponse response = AJMStakeAPICALL(input.EmiratesId, Index, input.Passport, input.Code);
                if (!response.Content.Contains("Message: \"Success\""))
                {
                    if (Index == "eid")
                    {
                        response.Content = response.Content.Replace("{\"EmiratesId\":[{", "{\"Emirates\":[{");
                    }
                    else if (Index == "pass")
                    {
                        response.Content = response.Content.Replace("{\"Passports\":[{", "{\"Emirates\":[{");

                    }
                    OwnerDetails.Root objresp = JsonConvert.DeserializeObject<OwnerDetails.Root>(response.Content);

                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Record Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FillNationality(string UserAgent)
        {
            int flag = 0;
            var json = "";
            try
            {

                RestResponse response = AJMStakeNationalityAPICALL();
                if (!response.Content.Contains("Message: \"Success\""))
                {
                    AJMOwnerPropertiesDetails.Root objresp = JsonConvert.DeserializeObject<AJMOwnerPropertiesDetails.Root>(response.Content);
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog("", json, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Record Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog("", ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog("", ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog("", ResponseDescription, ConfigurationManager.AppSettings["AJMStakeCode"].ToString(), ConfigurationManager.AppSettings["AJMStake"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        private static RestResponse AJMStakeAPICALL(string emirateID, string Keyvalue, string Passwport, string Code)
        {
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["AJMRes_User"].ToString() + ":" + ConfigurationManager.AppSettings["AJMRes_Pass"].ToString()));

            string apiURL = ConfigurationManager.AppSettings["AJMOwner_URL"].ToString();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["AJMRes_Key"].ToString());
            request.AddHeader("Authorization", "Basic " + vToken);

            request.AddHeader("Content-Type", "application/json");

            string jsonString = "";
            if (Keyvalue == "eid")
            {
                jsonString = "{\"EmiratesIds\":[\"" + emirateID + "\"],\"apiKey\":\"eff1952f13f3bbf3b8101d981acc714f4a4c595d9f252c6f3a0b9125f1d0c787\",\"PreviousProperties\":\"true\"}";
                request.AddHeader("KeyValue", "emiratesList");
            }
            else if (Keyvalue == "pass")
            {
                jsonString = "{\"PassportNumber\":[\"" + Passwport + "\"],\"apiKey\":\"eff1952f13f3bbf3b8101d981acc714f4a4c595d9f252c6f3a0b9125f1d0c787\",\"PreviousProperties\":\"true\"}";
                request.AddHeader("KeyValue", "passportList");
            }

            request.AddJsonBody(jsonString);
            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);
            return response;
        }
        private static RestResponse AJMStakeNationalityAPICALL()
        {

            string apiURL = ConfigurationManager.AppSettings["AJMOwner_URL"].ToString();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["AJMRes_Key"].ToString());
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["AJMRes_User"].ToString() + ":" + ConfigurationManager.AppSettings["AJMRes_Pass"].ToString()));

            request.AddHeader("Authorization", "Basic " + vToken);



            request.AddHeader("Content-Type", "application/json");
            var Passwport = "";


            string jsonString = "{\"PassportNumber\":[\"" + Passwport + "\"],\"apiKey\":\"eff1952f13f3bbf3b8101d981acc714f4a4c595d9f252c6f3a0b9125f1d0c787\",\"PreviousProperties\":\"true\"}";
            request.AddHeader("KeyValue", "nationalitiesList");


            request.AddJsonBody(jsonString);
            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response;
        }
    }
}