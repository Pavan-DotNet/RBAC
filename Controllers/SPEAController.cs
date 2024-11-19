using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MOCDIntegrations.Controllers
{
    public class SPEAController : Controller
    {
        // GET: SPEA
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                List<string> lstStudents = new List<string>();

                flag++;

                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };

                RestResponse response = CallInfoAPICall(EmiratesId);
                if (response.Content != null)
                {
                    SPEADetails.Root root = JsonConvert.DeserializeObject<SPEADetails.Root>(response.Content);
                    if (root.success)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { root.data, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
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

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SCHSCode"].ToString(), ConfigurationManager.AppSettings["SCHS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                // var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SCHSCode"].ToString(), ConfigurationManager.AppSettings["SCHS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }



            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static RestResponse CallInfoAPICall(string emirateID)
        {

            string apiURL = ConfigurationManager.AppSettings["SPEA_URL"].ToString();

            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            var body = "";
            request.AddHeader("Authorization", "Bearer " + LoginApiCall());

            //request.AddHeader("AppCode", "0");
            request.AddHeader("Content-Type", "application/json");

            body = @"{" + '\u0022' + "emid" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' +
                                        @"}";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            string jsonString = JsonConvert.SerializeObject(request); //JsonConverter.Serialize(request);
            RestResponse response = client.Execute(request);


            return response;
        }


        private static string LoginApiCall()
        {

            string apiURL = ConfigurationManager.AppSettings["SH_Login_URL"].ToString();




            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            var body = "";

            //request.AddHeader("AppCode", "0");
            request.AddHeader("Content-Type", "application/json");

            body = @"{" + "\n" + @"""username"":" + '\u0022' + "user90216" + '\u0022' + "," + "\n" + @"    ""password"":" + '\u0022' + "c%rcq0" + '\u0022' + "\n" + @"}";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            string jsonString = JsonConvert.SerializeObject(request); //JsonConverter.Serialize(request);
            RestResponse response = client.Execute(request);
            ShrjahTokenDetails.Root root = JsonConvert.DeserializeObject<ShrjahTokenDetails.Root>(response.Content);

            return root.access_token;
        }
    }
}