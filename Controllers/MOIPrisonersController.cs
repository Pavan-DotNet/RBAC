using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Controllers
{
    public class MOIPrisonersController : Controller
    {
        public ActionResult Index()
        {
            return View("");
        }
        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["MOI_uri"].ToString(), ConfigurationManager.AppSettings["MOI_grant_type"].ToString(), ConfigurationManager.AppSettings["MOI_client_id"].ToString(), ConfigurationManager.AppSettings["MOI_client_secret"].ToString(), ConfigurationManager.AppSettings["MOI_scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public ActionResult Search(string index, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<MOIPrisonersDetails.MIOFetailsRequestParams>(postdata);

                flag++;
                RestResponse response = MOIAPICALL(input.EmiratesId, index, input.UnifiedNumber);//MOIAPICALL("784197590940637");

                MOIPrisonersDetails.Root objresp = JsonConvert.DeserializeObject<MOIPrisonersDetails.Root>(response.Content);
                if (objresp?.content?.EmiratesId != null)
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<MOIPrisonersDetails.Root>(objresp), ConfigurationManager.AppSettings["MOIPRSCode"].ToString(), ConfigurationManager.AppSettings["MOIPRS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }

                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOIPRSCode"].ToString(), ConfigurationManager.AppSettings["MOIPRS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException ex)
            {
                flag = 3;
                // var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOIPRSCode"].ToString(), ConfigurationManager.AppSettings["MOIPRS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOIPRSCode"].ToString(), ConfigurationManager.AppSettings["MOIPRS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static RestResponse MOIAPICALL(string emirateID, string index, string unifiedNumber)
        {

            string apiURL = ConfigurationManager.AppSettings["MOI_URL"].ToString();
            string vToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MOIPris_Username"].ToString() + ":" + ConfigurationManager.AppSettings["MOIPris_Password"].ToString()));

            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["MOIPris_Key"].ToString());
            request.AddHeader("CallerSiteId", "38");

            request.AddHeader("Authorization", "Basic " + vToken);
            request.AddHeader("Content-Type", "application/json");
            var body = "";
            if (index == "2")
                body = @"{" + '\u0022' + "EmiratesId" + '\u0022' + ":" + '\u0022' + emirateID + '\u0022' + @"}";

            else
                body = @"{" + '\u0022' + "UnifiedNumber" + '\u0022' + ":" + '\u0022' + unifiedNumber + '\u0022' + @"}";


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response;
        }
    }


}