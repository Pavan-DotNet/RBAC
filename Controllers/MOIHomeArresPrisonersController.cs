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
using System.Text;
using System.Reflection;

namespace MOCDIntegrations.Controllers
{
    public class MOIHomeArresPrisonersController : Controller
    {
        // GET: MOIHomeArresPrisoners
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string index, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<MOIPrisonersDetails.MIOFetailsRequestParams>(postdata);

                flag++;
                RestResponse response = MOIAPICALL(input.EmiratesId, index, input.UnifiedNumber);//MOIAPICALL("784197590940637");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MOIHomeArresPrisonersDetails.Root objresp = JsonConvert.DeserializeObject<MOIHomeArresPrisonersDetails.Root>(response.Content);
                    if (objresp != null)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["MOIHPRSCode"].ToString(), ConfigurationManager.AppSettings["MOIHPRS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOIHPRSCode"].ToString(), ConfigurationManager.AppSettings["MOIHPRS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOIHPRSCode"].ToString(), ConfigurationManager.AppSettings["MOIHPRS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException ex)
            {
                flag = 3;
                // var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOIHPRSCode"].ToString(), ConfigurationManager.AppSettings["MOIHPRS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOIHPRSCode"].ToString(), ConfigurationManager.AppSettings["MOIHPRS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static RestResponse MOIAPICALL(string emirateID, string index, string unifiedNumber)
        {
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MOIH_username"].ToString() + ":" + ConfigurationManager.AppSettings["MOIH_password"].ToString()));

            //emirateID = "784196635720681";
            string apiURL = ConfigurationManager.AppSettings["MOIH_URL"].ToString();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["MOIH_apikey"].ToString());
            request.AddHeader("CallerSiteId", "38");

            request.AddHeader("Authorization", "Basic " + vToken);
            request.AddHeader("Content-Type", "application/json");
            var body = "";
            if (index == "2")
                body = @"{" + '\u0022' + "EmiratesId" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' + @"}";

            else
                body = @"{" + '\u0022' + "UnifiedNumber" + '\u0022' + ":" + '\u0022' + unifiedNumber + '\u0022' + @"}";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response;
        }
    }
}