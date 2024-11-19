using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MOCDIntegrations.Controllers
{
    public class GrantsController : Controller
    {
        public static string Title = "طلبات المنح / الأعراس الجماعية / رسائل لمن يهمه الأمر";
        // GET: Grants
        public ActionResult Index()
        {
            return View();
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
            string input = "{ \"EmiratesID\": \"" + postdata + "\" }";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;
                var client = new WebClient();
                client.Headers.Add("Content-type", "application/json");
                client.Headers.Add("Accept", "application/json");
                client.Headers.Add("Authorization", $"Bearer {GenerateToken()}");
                var uri = new Uri($"{ConfigurationManager.AppSettings["CRMApiUrl"]}/Services/GetMFApplicationsByEID?EID={postdata}");
                byte[] _response = client.DownloadData(uri);
                if (_response != null)
                {
                    var responseObj = JsonConvert.DeserializeObject<MOCDResponse<List<GrantRequest>>>(Encoding.UTF8.GetString(_response));
                    if (responseObj.Code == MOCDErroCode.Success)
                    {
                        if (responseObj.Content.Count > 0)
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { responseObj.Content, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(input, JsonConvert.SerializeObject(responseObj.Content), EntityCodes.SRA.ToString(), Title, DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {
                            flag = 2;
                            json = JsonConvert.SerializeObject(new { ResponseDescriptionAr = "لا يوجد سجلات", flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(input, "لا يوجد سجلات", EntityCodes.SRA.ToString(), Title, DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    else
                    {
                        flag = 3;
                        json = JsonConvert.SerializeObject(new { responseObj.ResponseDescriptionAr, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(input, responseObj.ResponseDescriptionAr, EntityCodes.SRA.ToString(), Title, DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                flag = 4;
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(input, ex.Message, EntityCodes.SRA.ToString(), Title, DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}