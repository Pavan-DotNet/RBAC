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
using static MOCDIntegrations.Models.CDADetails;
using Newtonsoft.Json.Converters;
using RestSharp;
using static MOCDIntegrations.Models.OwnerDetails;

namespace MOCDIntegrations.Controllers
{
    public class CDANewController : Controller
    {
        // GET: CDA
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                var input = new JavaScriptSerializer().Deserialize<CDADetailsNew.CDARequest>(postdata);
                string soapResult = string.Empty;

                flag++;
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                bool IsActive = true;
                if (input.IsActive == "1")
                    IsActive = true;
                else IsActive = false;
                RestResponse response = CDAApiCall(input.BenficiaryEID, input.HoushouldEID, IsActive,input.MonthBatch);
                if (response != null && response.Content != null)
                {
                    if (response.Content.Contains("\"StatusCode\":200}"))
                    {
                        CDADetailsNew.Root objresp = JsonConvert.DeserializeObject<CDADetailsNew.Root>(response.Content);
                        if (objresp.Value != null)
                        {

                            flag = 1;
                            json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                            LogIntegrationDetails.LogSerilog(input.BenficiaryEID + input.HoushouldEID + input.IsActive, json, ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {
                            flag = 3;

                            //var resp = new StreamReader(ex.Message).ReadToEnd();
                            string ResponseDescription = "Record Not Found";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                    }
                }


            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }


        private RestResponse CDAApiCall(string BenficiaryEID, string HoushouldEID, bool IsActive = true,string MonthBatch="")
        {
            var body = "";
            string apiURL = ConfigurationManager.AppSettings["CDA_Url"].ToString();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["CDA_USERNAME"].ToString() + ":" + ConfigurationManager.AppSettings["CDA_PASSWORD"].ToString())));
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["CDA_GSB_KEY"].ToString());

            request.AddHeader("x-Gateway-APIKey", ConfigurationManager.AppSettings["CDA_API_KEY"].ToString());
            request.AddHeader("Content-Type", "application/json");

            body = @"{" + '\u0022' + "BenficiaryEID" + '\u0022' + ":" + '\u0022' + BenficiaryEID + '\u0022' + ","
                + '\u0022' + "HoushouldEID" + '\u0022' + ":" + '\u0022' + HoushouldEID + '\u0022' + ","
                + '\u0022' + "IsActive" + '\u0022' + ":" + '\u0022' + IsActive + '\u0022' + ","
                + '\u0022' + "MonthBatch" + '\u0022' + ":" + '\u0022' + MonthBatch + '\u0022' +
                                        @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            string jsonString = JsonConvert.SerializeObject(request);
            RestResponse response = client.Execute(request);
            return response;
        }
    }
}