using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Configuration;
using static MOCDIntegrations.Models.NAFIS;
using RestSharp;
using System.Threading.Tasks;
using System.Text;

namespace MOCDIntegrations.Controllers
{
    public class NAFISController : Controller
    {
        // GET: NAFIS
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Search(string postdata, string UserAgent)
        {
            int flag = 0;
            var json = "";
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();

                var input = new JavaScriptSerializer().Deserialize<AJHRD.AJHRDDetailsRequestParams>(postdata);

                string EmirateID = input.EmiratesId;

                var client = new RestClient();

                var request = new RestRequest(ConfigurationManager.AppSettings["NAFIS_URL"].ToString() + EmirateID, Method.Get);
                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["NAFIS_USERNAME"].ToString() + ":" + ConfigurationManager.AppSettings["NAFIS_PASSWORD"].ToString())));

                request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["NAFIS_API_KEY"].ToString());
                request.AddHeader("clientId", ConfigurationManager.AppSettings["NAFIS_CLIENT_ID"].ToString());
                request.AddHeader("clientSecret", ConfigurationManager.AppSettings["NAFIS_CLIENT_SECRET"].ToString());
                request.AddHeader("userName", "mocd");
                request.AddHeader("password", "JEHiB^1m&$0a_U?8");
                RestResponse response = await client.ExecuteAsync(request);

                NAFISRoot objResponsee = null;

                objResponsee = JsonConvert.DeserializeObject<NAFISRoot>(response.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local });
                if (objResponsee != null && objResponsee.IsSucceeded && objResponsee.Data != null && objResponsee.Data.EmirateID != null)
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objResponsee, flag }, new IsoDateTimeConverter() { DateTimeFormat = "dd-MM-yyyy" });
                    LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["NAFISCode"].ToString(), ConfigurationManager.AppSettings["NAFIS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = objResponsee.Message?.ToString();
                    if (string.IsNullOrEmpty(ResponseDescription))
                    {
                        ResponseDescription = "Record not found";
                    }
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["NAFISCode"].ToString(), ConfigurationManager.AppSettings["NAFIS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }


            }
            catch (FaultException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["NAFISCode"].ToString(), ConfigurationManager.AppSettings["NAFIS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (WebException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["NAFISCode"].ToString(), ConfigurationManager.AppSettings["NAFIS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["sh_mun_uri"].ToString(), ConfigurationManager.AppSettings["sh_mun_grant_type"].ToString(), ConfigurationManager.AppSettings["sh_mun_client_id"].ToString(), ConfigurationManager.AppSettings["sh_mun_client_secret"].ToString(), ConfigurationManager.AppSettings["sh_mun_scope"].ToString());
                //TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["suri"].ToString(), ConfigurationManager.AppSettings["sgrant_type"].ToString(), ConfigurationManager.AppSettings["sclient_id"].ToString(), ConfigurationManager.AppSettings["sclient_secret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}