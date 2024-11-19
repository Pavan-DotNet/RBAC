using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using RestSharp;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.Http.Description;
using System.Text;

namespace MOCDIntegrations.Controllers
{
    public class ADKFController : Controller
    {
        // GET: ADKF
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
                var input = new JavaScriptSerializer().Deserialize<ADKFDetails.ADKFetailsRequestParams>(postdata);

                RestResponse response = ADKFAPICALL(input.EmiratesId);
                if (!response.Content.Contains("{\"Code\":4,"))
                {
                    ADKFDetails.Root objresp = JsonConvert.DeserializeObject<ADKFDetails.Root>(response.Content);
                    if (objresp?.Content != null)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["ADKFCode"].ToString(), ConfigurationManager.AppSettings["ADKF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Record Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADKFCode"].ToString(), ConfigurationManager.AppSettings["ADKF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Record Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADKFCode"].ToString(), ConfigurationManager.AppSettings["ADKF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADKFCode"].ToString(), ConfigurationManager.AppSettings["ADKF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADKFCode"].ToString(), ConfigurationManager.AppSettings["ADKF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static RestResponse ADKFAPICALL(string emirateID)
        {
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["ADKF_username"].ToString() + ":" + ConfigurationManager.AppSettings["ADKF_password"].ToString()));

            string apiURL = ConfigurationManager.AppSettings["ADKF_URL"].ToString() + "?EmiratesId=" + emirateID;
            var options = new RestClientOptions(ConfigurationManager.AppSettings["ADKF_URL"].ToString())
            {
                MaxTimeout = -1,
            };
            string securityKey = ConfigurationManager.AppSettings["ADKF_Key"].ToString();

            var client = new RestClient(options);
            var request = new RestRequest(apiURL, Method.Get);
            request.AddHeader("GSB-APIKey", securityKey);
            request.AddHeader("Authorization", "Basic " + vToken);
            request.AddHeader("Cookie", "NSC_JOioyzjac3urmywddpftq0cwhn2xxe3=ffffffff09e7860345525d5f4f58455e445a4a4227fc; d1732416-dd6d-4856-96db-b6f830f7e8e0=d002772b420f7a1fc9a53b9d9f56c3d6");
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }


        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["ADKF_uri"].ToString(), ConfigurationManager.AppSettings["ADKF_grant_type"].ToString(), ConfigurationManager.AppSettings["ADKF_client_id"].ToString(), ConfigurationManager.AppSettings["ADKF_client_secret"].ToString(), ConfigurationManager.AppSettings["ADKF_scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}