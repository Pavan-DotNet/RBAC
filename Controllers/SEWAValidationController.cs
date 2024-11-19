using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static MOCDIntegrations.Models.OwnerDetails;
using System.Configuration;

namespace MOCDIntegrations.Controllers
{
    public class SEWAValidationController : Controller
    {
        // GET: SEWAValidation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;




            flag++;
            try
            {
                var sewaResponse = SEWAValidateAPICALL(EmiratesId).Content;
                if (sewaResponse.Contains("\"ResponseMessage\":\"Success\""))
                {
                    var sewaDetails = JsonConvert.DeserializeObject<SEWAAccountDetails.Roots>(sewaResponse);
                    if (sewaDetails != null)
                    {
                        sewaDetails.inputIdNumber=EmiratesId;
                        // Delete old recoreds 
                       
                    }

                    if (sewaDetails != null )
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { sewaDetails, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, null, ConfigurationManager.AppSettings["SCHSCode"].ToString(), ConfigurationManager.AppSettings["SCHS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SCHSCode"].ToString(), ConfigurationManager.AppSettings["SCHS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SCHSCode"].ToString(), ConfigurationManager.AppSettings["SCHS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SEWAValiCode"].ToString(), ConfigurationManager.AppSettings["SEWAVali"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SEWAValiCode"].ToString(), ConfigurationManager.AppSettings["SEWAVali"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }




        private RestResponse SEWAValidateAPICALL(string accountNumber)
        {
            oAuthTokenGeneration oAuthTokenGeneration = new oAuthTokenGeneration();

            string apiURL = ConfigurationManager.AppSettings["SEWAAURL"].ToString()+"?AccountNumber="+accountNumber;

            var options = new RestClientOptions(apiURL)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(apiURL, Method.Get);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("MOCD-APIKey", ConfigurationManager.AppSettings["SEWAAAPIKey"].ToString());
            request.AddHeader("Authorization", "Bearer " + oAuthTokenGeneration.GenerateTokenWebMethods(ConfigurationManager.AppSettings["ICATokenUri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["SEWAAClientId"].ToString(), ConfigurationManager.AppSettings["SEWAAClientSecret"].ToString()).access_token);

            RestResponse response = client.Execute(request);
            return response;
        }

    }
}