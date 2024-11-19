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
using System.Xml;
using System.Text;

namespace MOCDIntegrations.Controllers
{
    public class AJMResidentialDetailsController : Controller
    {
        // GET: AJMResidentialDetails
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            int flag = 0;
            var json = "";
            try
            {
                var input = new JavaScriptSerializer().Deserialize<AJMResidentialDetails.AJMResidentialDetailsRequestParams>(postdata);

                RestResponse response = AJMShareAPICALL(input.EmiratesId);
                if (!response.Content.Contains("Message: \"Success\""))
                {
                    if (!response.Content.Contains("{\"Code\":\"EISB-SZHP-BRK-999\""))
                    {
                        AJMResidentialDetails.AJMResidentialDetail.Root objresp = JsonConvert.DeserializeObject<AJMResidentialDetails.AJMResidentialDetail.Root>(response.Content);

                        flag = 1;
                        json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["AJMResCode"].ToString(), ConfigurationManager.AppSettings["AJMShare"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Record Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMShareCode"].ToString(), ConfigurationManager.AppSettings["AJMShare"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


                    }

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Record Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMShareCode"].ToString(), ConfigurationManager.AppSettings["AJMShare"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMShareCode"].ToString(), ConfigurationManager.AppSettings["AJMShare"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMShareCode"].ToString(), ConfigurationManager.AppSettings["AJMShare"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static RestResponse AJMShareAPICALL(string emirateID)
        {

            string apiURL = ConfigurationManager.AppSettings["AJMResident_URL"].ToString() + "?emiratesId=" + emirateID;
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Get);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["AJMRes_Key"].ToString());
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["AJMRes_User"].ToString() + ":" + ConfigurationManager.AppSettings["AJMRes_Pass"].ToString()));
            request.AddHeader("Authorization", "Basic " + vToken);
            request.AddHeader("Accept", "application/json");

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