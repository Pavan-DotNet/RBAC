using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace MOCDIntegrations.Controllers
{
    public class SharjahMunicipalityController : Controller
    {
        // GET: SharjahMunicipality
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                flag++;
                if (EmiratesId != null)
                {
                    //EmiratesId = "784194421636906";
                    String responseBody;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["Sharjah_GSB_Mun_URL"].ToString() + "/" + EmiratesId + "/" + "null" + "/" + "null");//784197817139526 
                    request.Method = "GET";
                    request.Headers.Add("Authorization", "Bearer " + GenerateToken());
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader responseStream = new StreamReader(response.GetResponseStream());
                    responseBody = responseStream.ReadToEnd();
                    ShrajahMunciplalityDetails.Rooot objResponse = null;
                    if (responseBody.Contains("{\"code\":0,\""))
                    {
                        objResponse = JsonConvert.DeserializeObject<ShrajahMunciplalityDetails.Rooot>(responseBody);
                        if (objResponse != null)
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { objResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ShMunCode"].ToString(), ConfigurationManager.AppSettings["ShMun"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 3;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ShMunCode"].ToString(), ConfigurationManager.AppSettings["ShMun"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    else
                    {
                        flag = 3;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ShMunCode"].ToString(), ConfigurationManager.AppSettings["ShMun"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
            }
            catch (WebException ex)
            {
                flag = 4;

                // var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ShMunCode"].ToString(), ConfigurationManager.AppSettings["ShMun"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


            }
            catch (Exception ex)
            {
                flag = 4;
                //  var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ShMunCode"].ToString(), ConfigurationManager.AppSettings["ShMun"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["sh_mun_uri"].ToString(), ConfigurationManager.AppSettings["sh_mun_grant_type"].ToString(), ConfigurationManager.AppSettings["sh_mun_client_id"].ToString(), ConfigurationManager.AppSettings["sh_mun_client_secret"].ToString(), ConfigurationManager.AppSettings["sh_mun_scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
