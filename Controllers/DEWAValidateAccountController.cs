using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using System.Configuration;
using static MOCDIntegrations.Models.DEWADetails;

namespace MOCDIntegrations.Controllers
{
    public class DEWAValidateAccountController : Controller
    {
        // GET: DEWAValidateAccount
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

                var dewaResponse = DEWAAPICALL(EmiratesId).Content;

                if (dewaResponse!="{}")
                {
                    if (!dewaResponse.Contains("\"success\\\": true"))
                    {
                        var dewaDetails = JsonConvert.DeserializeObject<DewaDetailsModel.Root>(dewaResponse);
                        if (dewaDetails != null && dewaDetails.GetPremiseDetailsResponse.Body.message != null && dewaDetails.GetPremiseDetailsResponse.Body.message.code == "00")
                        {
                            var dewaDetailsResponse = dewaDetails.GetPremiseDetailsResponse.Body.premiseDetailsResp.record[0];
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { dewaDetailsResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
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
        private RestResponse DEWAAPICALL(string accountNumber)
        {
            oAuthTokenGeneration oAuthTokenGeneration = new oAuthTokenGeneration();

            string apiURL = ConfigurationManager.AppSettings["DEWAURL"].ToString()+"?AccountNumber="+accountNumber;

            var options = new RestClientOptions(apiURL)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(apiURL, Method.Get);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("MOCD-APIKey", ConfigurationManager.AppSettings["DEWAAPIKey"].ToString());
            request.AddHeader("Authorization", "Bearer " + oAuthTokenGeneration.GenerateTokenWebMethods(ConfigurationManager.AppSettings["ICATokenUri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["DEWAClientId"].ToString(), ConfigurationManager.AppSettings["DEWAClientSecret"].ToString()).access_token);

            RestResponse response = client.Execute(request);
            return response;
        }

    }
}