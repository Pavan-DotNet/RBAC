using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Text;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Controllers
{
    public class EWAController : Controller
    {
        // GET: EWA
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
                RestResponse response = EWAAPICALL(EmiratesId);
                if (response != null && response.Content != null)
                {
                    if (response.Content.Contains("\"StatusCode\":\"00\""))
                    {
                        EWAModel.Root objresp = JsonConvert.DeserializeObject<EWAModel.Root>(response.Content);
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["EWACode"].ToString(), ConfigurationManager.AppSettings["EWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
                else
                {
                    flag = 3;
                    string ResponseDescription = "No Matching Records available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["EWACode"].ToString(), ConfigurationManager.AppSettings["EWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["EWACode"].ToString(), ConfigurationManager.AppSettings["EWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["EWACode"].ToString(), ConfigurationManager.AppSettings["EWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static RestResponse EWAAPICALL(string emirateID)
        {
            var body = "";
            string apiURL = ConfigurationManager.AppSettings["EWA_URL"].ToString();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["EWA_Username"].ToString() + ":" + ConfigurationManager.AppSettings["EWA_Password"].ToString())));

            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["EWA_GSB_Key"].ToString());
            request.AddHeader("Content-Type", "application/json");

            body = @"{" + '\u0022' + "AccountNumber" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' +
                                        @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            string jsonString = JsonConvert.SerializeObject(request);
            RestResponse response = client.Execute(request);
            return response;
        }

    }
}
