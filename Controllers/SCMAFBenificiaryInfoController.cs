using MOCDIntegrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.IO;
using MOCDIntegrations.SCMAFBeneficiary;

namespace MOCDIntegrations.Controllers
{
    public class SCMAFBenificiaryInfoController : Controller
    {
        // GET: SCMAF
        //public ActionResult SCMAFBenificiary()
        //{
        //    return View("SCMAFBenificiary");
        //}
        public ActionResult Index()
        {
            return View("SCMAFBenificiaryInfo");
        }

        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            SCAMFBeneficiariesInformation_Output objResp = null;
            JsonHelper objHelper = new JsonHelper();
            try
            {
                var input = new JavaScriptSerializer().Deserialize<OwnerProfileDetails.OwnerProfileDetailsRequest>(postdata);

                objResp = FetchBeneficiaryInfo(input.EmiratesId);
                if (!String.IsNullOrEmpty(objResp.BeneficiaryID))
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { objResp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["SCMAFBenCode"].ToString(), ConfigurationManager.AppSettings["SCMAFBen"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SCMAFBenCode"].ToString(), ConfigurationManager.AppSettings["SCMAFBen"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }
            }
            catch (WebException ex)
            {
                flag = 3;
                // var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SCMAFBenCode"].ToString(), ConfigurationManager.AppSettings["SCMAFBen"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SCMAFBenCode"].ToString(), ConfigurationManager.AppSettings["SCMAFBen"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);

        }

        private static SCAMFBeneficiariesInformation_Output FetchBeneficiaryInfo(string emirateID)
        {
            SCMAFBeneficiary.SCMAF_spcBenefiries_spcEligibility_spcCheckClient client = new SCMAFBeneficiary.SCMAF_spcBenefiries_spcEligibility_spcCheckClient();
            SCMAFBeneficiary.SCAMFBeneficiariesInformation_Input request = new SCMAFBeneficiary.SCAMFBeneficiariesInformation_Input();
            SCMAFBeneficiary.SCAMFBeneficiariesInformation_Output response = new SCMAFBeneficiary.SCAMFBeneficiariesInformation_Output();
            ConfigurationManager.AppSettings["Uri"].ToString();

            request.ServiceCode = ConfigurationManager.AppSettings["SCMAF_ServiceCode"].ToString();
            request.Channel = ConfigurationManager.AppSettings["SCMAF_Channel"].ToString();
            request.Journey = ConfigurationManager.AppSettings["SCMAF_Journey"].ToString();
            // request.EmiratesID = "784201326962079";

            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                //response = client.SCAMFBeneficiariesInformation(request);
                var newresponse = client.SCAMFBeneficiariesInformation(request);
                return newresponse;
            }
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

    }
}