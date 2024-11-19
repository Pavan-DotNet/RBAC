using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace MOCDIntegrations.Controllers
{
    public class ADDCAccountController : Controller
    {
        // GET: AADCAccount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            try
            {

                var aadcResponse = ADDCAPICALL(EmiratesId).Content;

                if (!aadcResponse.Contains("\"statusMessage\\\": Successful"))
                {
                    var aadcDetails = JsonConvert.DeserializeObject<AADCAccountDetails.Rooot>(aadcResponse);
                    if (aadcDetails != null &&  aadcDetails.statuscode == "200")
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { aadcDetails, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, null, ConfigurationManager.AppSettings["EWAValiCode"].ToString(), ConfigurationManager.AppSettings["EWAVali"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["EWAValiCode"].ToString(), ConfigurationManager.AppSettings["EWAVali"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["EWAValiCode"].ToString(), ConfigurationManager.AppSettings["EWAVali"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["DEWAValiCode"].ToString(), ConfigurationManager.AppSettings["DEWAVali"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["DEWAValiCode"].ToString(), ConfigurationManager.AppSettings["DEWAVali"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private RestResponse ADDCAPICALL(string accountNumber)
        {
            oAuthTokenGeneration oAuthTokenGeneration = new oAuthTokenGeneration();

            string apiURL = ConfigurationManager.AppSettings["ADDCURL"].ToString()+"?accID="+accountNumber;

            var options = new RestClientOptions(apiURL)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(apiURL, Method.Get);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("MOCD-APIKey", ConfigurationManager.AppSettings["ADDCAPIKey"].ToString());
            request.AddHeader("Authorization", "Bearer " + oAuthTokenGeneration.GenerateTokenWebMethods(ConfigurationManager.AppSettings["ICATokenUri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["ADDCClientId"].ToString(), ConfigurationManager.AppSettings["ADDCClientSecret"].ToString()).access_token);

            RestResponse response = client.Execute(request);
            return response;
        }


    }

}