using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using static MOCDIntegrations.Models.SEDD;

namespace MOCDIntegrations.Controllers
{
    public class SSSDSewaController : Controller
    {
        // GET: Default
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
                if (!string.IsNullOrEmpty(EmiratesId))
                {

                    RestResponse response = SEWAAPICALL(EmiratesId);
                    if (response.Content.Contains("{\"success\":true"))
                    {
                        SSSDSewaDetails.Root result = JsonConvert.DeserializeObject<SSSDSewaDetails.Root>(response.Content);
                        result.data.emiratesId = EmiratesId;
                        if (result.success)
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { result, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SSSDSEWACode"].ToString(), ConfigurationManager.AppSettings["SSSDSEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SSSDSEWACode"].ToString(), ConfigurationManager.AppSettings["SSSDSEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

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
                                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SSSDSEWACode"].ToString(), ConfigurationManager.AppSettings["SSSDSEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {

                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SSSDSEWACode"].ToString(), ConfigurationManager.AppSettings["SSSDSEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SSSDSEWACode"].ToString(), ConfigurationManager.AppSettings["SSSDSEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["SSSDSEWACode"].ToString(), ConfigurationManager.AppSettings["SSSDSEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public static string AccessToken(string accessTokenAPIUrl, string securityKey, string userName, string password)
        {
            ResponseAccessToken accessTokenResponse = null;
            try
            {

                var client = new RestClient(accessTokenAPIUrl);
                //client.Timeout = -1;
                var request = new RestRequest(accessTokenAPIUrl, Method.Post);
                //request.AddHeader("Authorization", "Bearer b9cd567f-9806-460a-8383-c2c93c0c19fb");
                request.AddHeader("Authorization", "Bearer " + securityKey);
                request.AddHeader("Content-Type", "text/plain");
                var body = @"{" + "\n" + @"""username"":" + '\u0022' + userName + '\u0022' + "," + "\n" + @"    ""password"":" + '\u0022' + password + '\u0022' + "\n" + @"}";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                RestResponse response = client.Execute(request);

                accessTokenResponse = JsonConvert.DeserializeObject<ResponseAccessToken>(response.Content);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return accessTokenResponse != null ? accessTokenResponse.access_token : null;
        }

        public static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["Uri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["client_id"].ToString(), ConfigurationManager.AppSettings["client_secret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static RestResponse SEWAAPICALL(string emirateID)
        {
            bool isTestEnv = false;

            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["SSSD_Sewa_username"].ToString() + ":" + ConfigurationManager.AppSettings["SSSD_Sewa_Pass"].ToString()));
            string apiURL = ConfigurationManager.AppSettings["SSSD_Sewa_Url"].ToString();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("Authorization", "Basic " + vToken);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["SSSD_Sewa_Key"].ToString());
            request.AddHeader("Content-Type", "application/json");
            var body = "";
            if (isTestEnv)
            {
                body = @"{" + '\u0022' + "emid" + '\u0022' + ":" + '\u0022' + "784192250503981" + '\u0022' + @"}";
            }
            else
            {
                body = @"{" + '\u0022' + "emid" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' + @"}";
            }

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response;
        }

    }

}