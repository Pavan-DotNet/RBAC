using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static MOCDIntegrations.Models.SHJDHDetails;
using Root = MOCDIntegrations.Models.SHJDHDetails.Root;
using System.Configuration;
using static MOCDIntegrations.Models.SEDD;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Controllers
{
    public class SHJDHController : Controller
    {
        // GET: SHJDH
        public ActionResult SHJDH()
        {
            return View("SHJDH");
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            //StreamWriter sw = new StreamWriter(@"d:\AJMMUNServiceData\Errorlog.txt", true);
            int flag = 0;
            var json = "";

            try
            {
                var input = new JavaScriptSerializer().Deserialize<SHJDHDetails.SHJDHRequest>(postdata);
                Root objresp = null;

                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };


                RestResponse response = SHJDPICALL(input.EmiratesId);
                if (!response.Content.Contains("{\"success\":false"))
                {
                    objresp = JsonConvert.DeserializeObject<Root>(response.Content);

                    if (objresp != null)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["SHJDHCode"].ToString(), ConfigurationManager.AppSettings["SHJDH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                         LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SHJDHCode"].ToString(), ConfigurationManager.AppSettings["SHJDH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                }

                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                     LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SHJDHCode"].ToString(), ConfigurationManager.AppSettings["SHJDH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }

            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SHJDHCode"].ToString(), ConfigurationManager.AppSettings["SHJDH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SHJDHCode"].ToString(), ConfigurationManager.AppSettings["SHJDH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static RestResponse SHJDPICALL(string emirateID)
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();

                string userName = ConfigurationManager.AppSettings["sedd_userName"].ToString();
                string password = ConfigurationManager.AppSettings["sedd_password"].ToString();
                string accessTokenApiURL = ConfigurationManager.AppSettings["sedd_login_api_url"].ToString();
                string apiURL = ConfigurationManager.AppSettings["shjd_api_url"].ToString();
                string securityKey = GenerateToken();//IEUtils.GenerateToken();
                string accessToken = obj.AccessToken(accessTokenApiURL, securityKey, userName, password);
                var client = new RestClient(apiURL);
                var request = new RestRequest(apiURL, Method.Post);
                request.AddHeader("Authorization", "Bearer " + securityKey);
                request.AddHeader("AccessToken", accessToken);
                request.AddHeader("Content-Type", "application/json");
                var body = "";

                body = @"{" + '\u0022' + "emid" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' + @"}";


                request.AddParameter("application/json", body, ParameterType.RequestBody);
                RestResponse response = client.Execute(request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["sedd_uri"].ToString(), ConfigurationManager.AppSettings["sedd_grant_type"].ToString(), ConfigurationManager.AppSettings["sedd_client_id"].ToString(), ConfigurationManager.AppSettings["sedd_client_secret"].ToString(), ConfigurationManager.AppSettings["sedd_scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}