using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Root = MOCDIntegrations.Models.Root;

namespace MOCDIntegrations.Controllers
{
    public class ADSSAController : Controller
    {
        // GET: ADSSA
        public ActionResult Index()
        {
            return View("");
        }

        private string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["uri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["client_id"].ToString(), ConfigurationManager.AppSettings["client_secret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<ADSSADetails.ADSSADetailsRequest>(postdata);
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["ADSSA_USERNAME"].ToString() + ":" + ConfigurationManager.AppSettings["ADSSA_PASSWORD"].ToString()));

                string body = @"{""EmiratesId"":" + input.EmiratesId + "}";
                string apiURL = ConfigurationManager.AppSettings["ADSSA_URL"].ToString();
                var client = new RestClient(apiURL);
                var request = new RestRequest(apiURL, Method.Post);
                request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["ADSSA_API_KEY"].ToString());
                request.AddHeader("Username", ConfigurationManager.AppSettings["ADSSA_USERNAME"].ToString());
                request.AddHeader("Password", "D#J@1w6)");

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Basic TU9DRFByZENvbnN1bWVyMzptb2NkcHJkdXNlciQz");

                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = client.Execute(request);

                Root objresp = null;
                objresp = JsonConvert.DeserializeObject<Root>(response.Content);

                if (objresp != null && objresp.Result != null)
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<Root>(objresp), ConfigurationManager.AppSettings["ADSSACode"].ToString(), ConfigurationManager.AppSettings["ADSSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADSSACode"].ToString(), ConfigurationManager.AppSettings["ADSSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }


            }
            catch (WebException ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADSSACode"].ToString(), ConfigurationManager.AppSettings["ADSSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADSSACode"].ToString(), ConfigurationManager.AppSettings["ADSSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);

        }
    }
}