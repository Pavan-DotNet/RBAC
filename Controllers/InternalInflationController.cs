using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static MOCDIntegrations.Models.ADAFSADetails;
using System.Web.Script.Serialization;
using RestSharp;
using System.Configuration;

namespace MOCDIntegrations.Controllers
{
    public class InternalInflationController : Controller
    {
        // GET: InternalInflation
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Search(string postdata, string stype, string UserAgent)
        {
            var json = "";
            int flag = 0;
            string DATA = "";

            ADAFSAResponse.BeneficiaryResponse beneficiaryResponse = new ADAFSAResponse.BeneficiaryResponse();
            try
            {
                var input = new JavaScriptSerializer().Deserialize<ADAFSADetails.ADAFSADetailsRequestParams>(postdata);
                string soapResult = string.Empty;

                flag++;
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                DATA = input.EmiratesID.ToString(); // "784194025164941";//

                RestResponse response = InflationAPICALL(input.EmiratesID, stype);

                InternalInflationDetails.Root objresp = JsonConvert.DeserializeObject<InternalInflationDetails.Root>(response.Content);
                if (objresp.code == 200)
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["PPCaseCode"].ToString(), ConfigurationManager.AppSettings["PPCase"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADAFSACode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADAFSACode"].ToString(), ConfigurationManager.AppSettings["ADAFSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static RestResponse InflationAPICALL(string emirateID, string stype)
        {
            string apiURL = "";
            if (stype == "1")
            {
                apiURL = ConfigurationManager.AppSettings["inflation_api_url"].ToString() + "?nationalId=" + emirateID;


            }
            else if (stype == "2")
            {
                apiURL = ConfigurationManager.AppSettings["inflation_api_url"].ToString() + "?mobileNumber=" + emirateID;

            }
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Get);

            RestResponse response = client.Execute(request);
            return response;
        }


    }
}