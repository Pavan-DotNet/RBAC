using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json;
using MOCDIntegrations.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Converters;
using MOCDIntegrations.Models.NRC_Client;
using RestSharp;
using System.Net.Http;
using System.Net.Mime;

namespace MOCDIntegrations.Controllers
{
    public class NRCClientController : Controller
    {
        // GET: NRC_Client
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            string DATA = "";
            NRCClientDetails nRCClientDetails = new NRCClientDetails();
            try
            {
                string Result = string.Empty;
                EmiratesId = FormatEmiratesIdNumber(EmiratesId);
                flag++;
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                DATA = EmiratesId;
                if (DATA.Length == 15)
                {
                    try
                    {

                        Result = CallWebService(FormatEmiratesIdNumber(DATA), ConfigurationManager.AppSettings["NRC_Url"].ToString());
                        APIResponse response = JsonConvert.DeserializeObject<APIResponse>(Result);
                        if (response.StatusCode != "" && response.StatusCode == "200")
                        {
                            if (response.Data.uaeId.Length > 0)
                            {
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { response.Data, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["NRCCode"].ToString(), ConfigurationManager.AppSettings["NRCCode"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = "No Matching Records available";
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["NRCCode"].ToString(), ConfigurationManager.AppSettings["NRCCode"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }

                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["NRCCode"].ToString(), ConfigurationManager.AppSettings["NRCCode"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    catch (Exception ex)
                    {

                        flag = 2;
                        string ResponseDescription = ex.Message;
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["NRCCode"].ToString(), ConfigurationManager.AppSettings["NRCCode"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
            }
            catch (WebException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["NRCCode"].ToString(), ConfigurationManager.AppSettings["NRCCode"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["NRCCode"].ToString(), ConfigurationManager.AppSettings["NRCCode"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public static string FormatEmiratesIdNumber(string input)
        {
            if (input.Length != 15)
                throw new FormatException("Emirates Id  number. Must be 12 characters");

            return String.Format("{0}-{1}-{2}-{3}",
                             input.Substring(0, 3),
                             input.Substring(3, 4),
                             input.Substring(7, 7),
                             input.Substring(14, 1));


        }
        public static string CallWebService(string EmiratesId, string url)
        {
            
            string Result;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            try
            {
                var client = new RestClient(url+EmiratesId);
                var request = new RestRequest(url + EmiratesId, Method.Get);
                request.AddHeader("APIKey", ConfigurationManager.AppSettings["BulkServiceAPIKey"].ToString());
                RestResponse response = client.Execute(request);
                return Result = response.Content;

            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }

                throw ex;
            }
        }


    }
}