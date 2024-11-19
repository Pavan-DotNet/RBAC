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
using System.Xml;
using System.Configuration;
using static MOCDIntegrations.Models.AJHRD;
using System.Text;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Controllers
{
    public class SharjahCharityController : Controller
    {
        // GET: SharjahCharity
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            int flag = 0;
            var json = "";
            try
            {

                string AccessToken = GenerateAccessToken(GenerateTokenMOCD());

                flag++;
                //requestCharityInquiryData.EmiratesId="784199092862798";
                string requestBody = @"{" + "\n" + @"    ""emid"": " + EmiratesId + "" + "\n" + @"}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["CharityInquiry_URL"].ToString());
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = requestBody.Length;
                // request.Headers.Add("Authorization", "Bearer " + GenerateToken());
                request.Headers.Add("Authorization", "Bearer " + GenerateTokenMOCD());
                request.Headers.Add("AccessToken", AccessToken);
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
                    SharjahCharityDetails.Rooot objResponsee = null;
                    if (!response1.Contains("{\"success\":false,"))
                    {
                        objResponsee = JsonConvert.DeserializeObject<SharjahCharityDetails.Rooot>(response1);

                        if (objResponsee != null)
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { objResponsee, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["SHCharityCode"].ToString(), ConfigurationManager.AppSettings["SHCharity"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records Available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                             LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SHCharityCode"].ToString(), ConfigurationManager.AppSettings["SHCharity"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                         LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SHCharityCode"].ToString(), ConfigurationManager.AppSettings["SHCharity"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }


                }
            }

            catch (FaultException fex)
            {
                var fault = fex.CreateMessageFault();
                var doc = new XmlDocument();
                var innerdoc = new XmlDocument();
                var innersdoc = new XmlDocument();
                var nav = doc.CreateNavigator();

                flag = 3;
                string ResponseDescription = string.Empty;

                if (fault.HasDetail)
                {
                    if (nav != null)
                    {
                        using (var writer = nav.AppendChild())
                        {
                            fault.WriteTo(writer, EnvelopeVersion.Soap12);
                        }

                        string str = string.Empty; //do something with it
                        foreach (XmlNode child in doc.DocumentElement.ChildNodes)
                        {

                            if (child.Name == "Code")
                            {
                                innerdoc.LoadXml(child.InnerXml);
                                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                {
                                    str += "Contact GSB Support.";
                                }
                            }

                            if (child.Name == "Detail")
                            {
                                //innerdoc.LoadXml(child.InnerXml);
                                //foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                //{
                                //    if (chd.Name == "arDesc")
                                //    {
                                //        str += chd.InnerText + " - ";
                                //    }
                                //    if (chd.Name == "enDesc")
                                //    {
                                //        str += chd.InnerText;
                                //    }

                                //    if (chd.Name == "details")
                                //    {
                                //        innersdoc.LoadXml(chd.InnerXml);
                                //        foreach (XmlNode chds in innersdoc.DocumentElement.ChildNodes)
                                //        {
                                //            if (chds.Name == "message")
                                //            {
                                //                str += chd.InnerText;
                                //            }
                                //        }
                                //    }
                                //}
                                ResponseDescription = child.InnerXml;
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }); ;
                                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SHCharityCode"].ToString(), ConfigurationManager.AppSettings["SHCharity"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {

                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SHCharityCode"].ToString(), ConfigurationManager.AppSettings["SHCharity"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SHCharityCode"].ToString(), ConfigurationManager.AppSettings["SHCharity"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SHCharityCode"].ToString(), ConfigurationManager.AppSettings["SHCharity"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }

        private static string GenerateTokenMOCD()
        {
            try
            {
                string Result = GenerateAndGetToken(ConfigurationManager.AppSettings["SHJ_Uri"].ToString(), ConfigurationManager.AppSettings["SHJ_grant_type"].ToString(), ConfigurationManager.AppSettings["SHJ_client_id"].ToString(), ConfigurationManager.AppSettings["SHJ_client_secret"].ToString(), ConfigurationManager.AppSettings["SHJ_scope"].ToString()).access_token;
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static TokenDetails GenerateAndGetToken(string uri, string grant_type, string client_id, string client_secret, string scope)
        {
            TokenDetails objToken = new TokenDetails();
            StringBuilder tokenUri = new StringBuilder();
            tokenUri.Append(uri);
            tokenUri.AppendFormat("?grant_type=" + grant_type + "&client_id=" + client_id + "&client_secret=" + client_secret + "&scope=" + scope);
            String responseBody;

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUri.ToString());
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader responseStream = new StreamReader(response.GetResponseStream());
                responseBody = responseStream.ReadToEnd();
                objToken = JsonConvert.DeserializeObject<TokenDetails>(responseBody);
                return objToken;
            }
            catch (WebException ex)
            {
                StreamReader responseStream = new StreamReader(ex.Response.GetResponseStream());
                responseBody = responseStream.ReadToEnd();
                objToken = JsonConvert.DeserializeObject<TokenDetails>(responseBody);
                return objToken;
            }
        }
        public static TokenDetails GetTheAccessTokenwithBody(string vToken, string uri)
        {
            try
            {
                TokenDetails objResponsee = new TokenDetails();
                string Username = "user90216";
                string Password = "c%rcq0";
                string requestBody = "{" + "\"username\":\"" + Username + "\"," + "\"password\":\"" + Password + "\"}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri.ToString());
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = requestBody.Length;
                request.Headers.Add("Authorization", "Bearer " + vToken);
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

                    objResponsee = JsonConvert.DeserializeObject<TokenDetails>(response1);
                }
                return objResponsee;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static string GenerateAccessToken(string GToken)
        {
            try
            {
                string Result = GetTheAccessTokenwithBody(GToken.ToString(), ConfigurationManager.AppSettings["SHJ_AT_uri"].ToString()).access_token;
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}