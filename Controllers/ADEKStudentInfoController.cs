using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Configuration;

namespace MOCDIntegrations.Controllers
{
    public class ADEKStudentInfoController : Controller
    {
        // GET: ADEKStudentInfo
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

                RestResponse response = StudentInfoAPICall(EmiratesId);
                Models.StudentInfo.Root root = JsonConvert.DeserializeObject<Models.StudentInfo.Root>(response.Content);
                if (root.ResponseStatus.Code == "DAT0001")
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                     LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADEKCode"].ToString(), ConfigurationManager.AppSettings["ADEK"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { root.StudentData, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId,null, ConfigurationManager.AppSettings["ADEKCode"].ToString(), ConfigurationManager.AppSettings["ADEK"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADEKCode"].ToString(), ConfigurationManager.AppSettings["ADEK"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
               // var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADEKCode"].ToString(), ConfigurationManager.AppSettings["ADEK"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }



            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static RestResponse StudentInfoAPICall(string emirateID)
        {
            string apiURL = "";
            apiURL = ConfigurationManager.AppSettings["api_url_studentInfo"].ToString() + "?EmiratesId=" + emirateID;
            var client = new RestClient(apiURL);
            string GSBAPIKey = ConfigurationManager.AppSettings["AdekStd_GSB_APIKey"].ToString();
            var request = new RestRequest(apiURL, Method.Get);
            request.AddHeader("GSB-APIKey", GSBAPIKey);
            string securityKey = GenerateToken();
            request.AddHeader("Authorization", "Bearer " + securityKey);
            RestResponse response = client.Execute(request);
            return response;
        }
        public static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["AdekStd_Uri"].ToString(), ConfigurationManager.AppSettings["AdekStd_grant_type"].ToString(), ConfigurationManager.AppSettings["AdekStd_client_id"].ToString(), ConfigurationManager.AppSettings["AdekStd_client_secret"].ToString(), ConfigurationManager.AppSettings["AdekStd_scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
