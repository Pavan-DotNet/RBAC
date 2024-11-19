using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Web.Http.Description;

using Newtonsoft.Json.Linq;
using System.Runtime.Remoting;
using MOCDIntegrations.Utils;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using System.Data.SqlClient;
using static MOCDIntegrations.Models.SEDD;
using Root = MOCDIntegrations.Models.SEDD.Root;

namespace MOCDIntegrations.Controllers
{
    public class SEDDController : Controller
    {
        // GET: SEDD
        public ActionResult SEDD()
        {
            return View("SEDD");
        }
        public ActionResult Search(string postdata, string UserAgent)
        {


            List<Object> lstAJMResponse = new List<Object>();
            var input = new JavaScriptSerializer().Deserialize<SEDDRequest>(postdata);
            Root objresp = null;
            var json = "";


            string soapResult = string.Empty;
            int flag = 0;

            flag++;
            try
            {
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };

                RestResponse response = SEDDAPICALL(input.EmiratesId);


                if (!response.Content.Contains("\"success\":false"))
                {
                    objresp = JsonConvert.DeserializeObject<Root>(response.Content);


                    if (objresp != null && objresp.data.licenses.Count == 0)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                       LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["SEDDCode"].ToString(), ConfigurationManager.AppSettings["SSSD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else if (objresp.data.licenses != null)
                    {
                        flag = 2;
                        json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                       LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["SEDDCode"].ToString(), ConfigurationManager.AppSettings["SSSD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 3;
                        string ResponseDescription = "No Matching Records Available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SEDDCode"].ToString(), ConfigurationManager.AppSettings["SSSD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                }
                else
                {
                    flag = 3;
                    string ResponseDescription = "No Matching Records Available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                   LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SEDDCode"].ToString(), ConfigurationManager.AppSettings["SSSD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }


            }
            catch (WebException ex)
            {
                flag = 4;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SEDDCode"].ToString(), ConfigurationManager.AppSettings["SSSD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


            }
            catch (Exception ex)
            {
                flag = 4;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SEDDCode"].ToString(), ConfigurationManager.AppSettings["SSSD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            //var json = JsonConvert.SerializeObject(new { json1, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
            return Json(json, JsonRequestBehavior.AllowGet);


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
        private static RestResponse SEDDAPICALL(string emirateID)
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();

                string userName = ConfigurationManager.AppSettings["sedd_userName"].ToString();
                string password = ConfigurationManager.AppSettings["sedd_password"].ToString();
                string accessTokenApiURL = ConfigurationManager.AppSettings["sedd_login_api_url"].ToString();
                string apiURL = ConfigurationManager.AppSettings["sedd_api_url"].ToString();
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


    }
}