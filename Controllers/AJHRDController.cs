using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static MOCDIntegrations.Models.AJHRD;

namespace MOCDIntegrations.Controllers
{
    public class AJHRDController : Controller
    {
        // GET: AJHRD
        public ActionResult Index()
        {
            return View("");
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            int flag = 0;
            var json = "";
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();

                var input = new JavaScriptSerializer().Deserialize<AJHRD.AJHRDDetailsRequestParams>(postdata);

                //string json = @"{" + "\n" + @"    ""P_NATIONAL_IDENTIFIER"": " + DRAJHRD["NATIONAL_ID"].ToString() + "" + "\n" + @"}";
                string EmirateID = input.EmiratesId;
                if (EmirateID.Length == 15)
                {
                    string requestBody = "{" + "\"InputParameters\": {" + "\"P_NATIONAL_IDENTIFIER\":\"" + EmirateID + "\"}}";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["AJHRD_URL"].ToString());
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.ContentLength = requestBody.Length;
                    request.Headers.Add("Authorization", "Bearer " + obj.GenerateToken(ConfigurationManager.AppSettings["AJHRD_uri"].ToString(), ConfigurationManager.AppSettings["AJHRD_grant_type"].ToString(), ConfigurationManager.AppSettings["AJHRD_client_id"].ToString(), ConfigurationManager.AppSettings["AJHRD_client_secret"].ToString(), ConfigurationManager.AppSettings["AJHRD_scope"].ToString()).access_token);
                    using (Stream webStream = request.GetRequestStream())
                    using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                    {
                        requestWriter.Write(requestBody);
                    }

                    WebResponse webResponse = request.GetResponse();
                    using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response1 = responseReader.ReadToEnd();
                        Rooot objResponsee = null;
                        objResponsee = JsonConvert.DeserializeObject<Rooot>(response1);
                        if (objResponsee != null && objResponsee.OutputParameters.P_CODE == "Success")
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { objResponsee, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["AJMHRCode"].ToString(), ConfigurationManager.AppSettings["AJMHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = objResponsee.OutputParameters.P_OUT_MSG.ToString();
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMHRCode"].ToString(), ConfigurationManager.AppSettings["AJMHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                    }
                }
            }
            catch (FaultException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMHRCode"].ToString(), ConfigurationManager.AppSettings["AJMHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (WebException ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMHRCode"].ToString(), ConfigurationManager.AppSettings["AJMHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            //catch (Exception ex)
            //{
            //    flag = 3;
            //    //var resp = new StreamReader(ex.Message).ReadToEnd();
            //    string ResponseDescription = ex.Message.ToString();
            //    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            //    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMHRCode"].ToString(), ConfigurationManager.AppSettings["AJMHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            //}
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}