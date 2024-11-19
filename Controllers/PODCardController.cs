using MOCDIntegrations.Models;
using MOCDIntegrations.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using static MOCDIntegrations.Models.POD;

namespace MOCDIntegrations.Controllers
{
    public class PODCardController : Controller
    {
        public ActionResult Index(string emiratesId)
        {
            return View("PODCard");
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 1;
            string ResponseDescription = "Data not found";
            PODDetails.GetDisabilityInfoResponse getDisabilityInfoResponse = new PODDetails.GetDisabilityInfoResponse(); 
            //PODCardResponse objResponse;
            try
            {
                DBManager db = new DBManager();
                JsonHelper objHelper = new JsonHelper();

                var input = new JavaScriptSerializer().Deserialize<PODCardRequest>(postdata);
                var response = PODAPICALL(input.EmiratesId);
                PODDetails.Root objResponse = JsonConvert.DeserializeObject<PODDetails.Root>(response.Content.ToString());
                getDisabilityInfoResponse.getDisabilityInfoResult = objResponse?.getDisabilityInfoResponse?.getDisabilityInfoResult;

                if (getDisabilityInfoResponse?.getDisabilityInfoResult?.ResponseStatus?.ResponseCode == "MOCD_0")
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objResponse, flag });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<PODDetails.GetDisabilityInfoResponse>(getDisabilityInfoResponse), ConfigurationManager.AppSettings["PODCardCode"].ToString(), ConfigurationManager.AppSettings["PODCardApp"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag });
                    LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["PODCardCode"].ToString(), ConfigurationManager.AppSettings["PODCardApp"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }

                //objResponse = new PODCardResponse();
                //objResponse = db.RetrievePODCardDetails(input.EmiratesId);

                //if (string.IsNullOrEmpty(objResponse.IdentificationNo))
                //    flag = 2;
                //if (flag == 1)
                //{
                //    json = JsonConvert.SerializeObject(new { objResponse, flag });
                //    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<PODCardResponse>(objResponse), ConfigurationManager.AppSettings["PODCardCode"].ToString(), ConfigurationManager.AppSettings["PODCardApp"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                //}
                //else
                //{
                //    json = JsonConvert.SerializeObject(new { ResponseDescription, flag });
                //    LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["PODCardCode"].ToString(), ConfigurationManager.AppSettings["PODCardApp"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                //}

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["PODCardCode"].ToString(), ConfigurationManager.AppSettings["PODCardApp"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static RestResponse PODAPICALL(string emirateID)
        {

            string apiURL = ConfigurationManager.AppSettings["PODURL"].ToString();

            var options = new RestClientOptions(apiURL)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("MOCD-APIKey", ConfigurationManager.AppSettings["PODAPIKey"].ToString());
            request.AddHeader("Authorization", "Bearer " + GenerateToken());
            request.AddHeader("Cookie", "BIGipServerTESTAPI.MOCD.GOV.AE_POOL_5566=929305866.48661.0000");
            var inquiryJson = new
            {

                EmiratesId = emirateID

            };
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(inquiryJson);


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            return response;
        }
        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateTokenWebMethods(ConfigurationManager.AppSettings["ICATokenUri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["PODClientId"].ToString(), ConfigurationManager.AppSettings["PODClientSecret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}

