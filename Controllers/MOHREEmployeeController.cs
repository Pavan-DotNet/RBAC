using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Controllers
{
    public class MOHREEmployeeController : Controller
    {
        // GET: MOHREEmployee
        public ActionResult Index()
        {
            return View("MOHREEmployee");
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
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<MOHREEmployeeDetails.MOHRERequest>(postdata);
                List<MOHREEmployeeDetails.MOHREResponse> lstMOHREResponse = new List<MOHREEmployeeDetails.MOHREResponse>();


                string DATA = @"{""eida"":" + input.EmiratesID + "}";


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["MOHREEmployeeUrl"].ToString());
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = DATA.Length;

                request.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["Mohre_username"].ToString() + ":" + ConfigurationManager.AppSettings["Mohre_password"].ToString()));
                request.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["Mohre_Key"].ToString();
                request.Headers.Add("Entity", "MOCD");

                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(DATA);
                }

                MOHREEmployeeDetails.MOHREResponse result = null;
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {

                    string response = responseReader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<MOHREEmployeeDetails.MOHREResponse>(response);

                    if (!string.IsNullOrEmpty(result.employeeNameArabic))
                    {
                        lstMOHREResponse.Add(result);
                    }

                }

                if (lstMOHREResponse != null && lstMOHREResponse.Count > 0)
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { lstMOHREResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<MOHREEmployeeDetails.MOHREResponse>>(lstMOHREResponse), ConfigurationManager.AppSettings["MOHREEMPCode"].ToString(), ConfigurationManager.AppSettings["MOHREEMP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = result.ErrorDescArabic + " - " + result.ErrorDescEnglish;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOHREEMPCode"].ToString(), ConfigurationManager.AppSettings["MOHREEMP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (FaultException ex)
            {

            }
            catch (WebException wex)
            {
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                flag = 3;
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new
                {
                    ResponseDescription,
                    flag
                }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOHREEMPCode"].ToString(), ConfigurationManager.AppSettings["MOHREEMP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOHREEMPCode"].ToString(), ConfigurationManager.AppSettings["MOHREEMP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}