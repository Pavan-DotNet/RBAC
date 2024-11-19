using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

using System.Web.Mvc;
using System.Configuration;
using System.Web.Script.Serialization;
using static MOCDIntegrations.Models.ADAFSADetails.ADAFSAResponse;
using System.IO;
using System.Net;
using System.Text;

namespace MOCDIntegrations.Controllers
{
    public class PPCaseDetailsController : Controller
    {
        // GET: PPCaseDetails
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            try
            {
                List<PPCaseDetails.PPCaseDetailsRequest> lstPPCaseDetails = new List<PPCaseDetails.PPCaseDetailsRequest>();

                string soapResult = string.Empty;

                flag++;
                var input = new JavaScriptSerializer().Deserialize<ADHADetails.ADHADetailsRequestParams>(postdata);

                RestResponse response = PPCaseDetailsAPICALL(input.EmiratesID);

                PPCaseDetails.Root objresp = JsonConvert.DeserializeObject<PPCaseDetails.Root>(response.Content);
                if (objresp.statusCode.Contains("SUCCESS"))
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["PPCaseCode"].ToString(), ConfigurationManager.AppSettings["PPCase"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }


            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["PPCaseCode"].ToString(), ConfigurationManager.AppSettings["PPCase"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                // var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["PPCaseCode"].ToString(), ConfigurationManager.AppSettings["PPCase"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }

        private static RestResponse PPCaseDetailsAPICALL(string emirateID)
        {

            string apiURL = ConfigurationManager.AppSettings["pp_api_url"].ToString() + emirateID;
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Get);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["PP_key"].ToString());
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["PP_user"].ToString() + ":" + ConfigurationManager.AppSettings["PP_pass"].ToString())));
            RestResponse response = client.Execute(request);
            return response;
        }
        public static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["pp_uri"].ToString(), ConfigurationManager.AppSettings["pp_grant_type"].ToString(), ConfigurationManager.AppSettings["pp_client_id"].ToString(), ConfigurationManager.AppSettings["pp_client_secret"].ToString(), ConfigurationManager.AppSettings["pp_scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}