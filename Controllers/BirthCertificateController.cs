using MOCDIntegrations.BirthCertificate;
using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static MOCDIntegrations.Models.ADAFSADetails.ADAFSAResponse;
using System.IO;
using System.Net;

namespace MOCDIntegrations.Controllers
{
    public class BirthCertificateController : Controller
    {
        // GET: BirthCertificate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            try
            {
                if (!string.IsNullOrEmpty(EmiratesId))
                {
                    //string emirateIDD = "784198016080701";
                    //EmiratesId = "784198016080701";
                    BirthCertificatePullListResponseType[] familyDetail = FetchFamilyDetails(EmiratesId);
                    if (familyDetail != null && familyDetail.Length > 0)
                    {
                        var result = SaveDependentInformation(familyDetail, EmiratesId);
                        if (result != null && result.Count > 0)
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { result, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["BirthCertificateCode"].ToString(), ConfigurationManager.AppSettings["BirthCertificate"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["BirthCertificateCode"].ToString(), ConfigurationManager.AppSettings["BirthCertificate"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }

                }

            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["BirthCertificateCode"].ToString(), ConfigurationManager.AppSettings["BirthCertificate"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
               // var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["BirthCertificateCode"].ToString(), ConfigurationManager.AppSettings["BirthCertificate"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);


        }

        private static BirthCertificatePullListResponseType[] FetchFamilyDetails(string emiratesId)
        {
            try
            {

                BirthCertificate.BirthCertificatePullPortClient client = new BirthCertificate.BirthCertificatePullPortClient();
                BirthCertificate.BirthCertificatePullListRequestType pullListRequestType = new BirthCertificate.BirthCertificatePullListRequestType { EmiratesID = emiratesId };
                BirthCertificate.BirthCertificatePullListResponse pullListResponse = new BirthCertificate.BirthCertificatePullListResponse();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateTokenForbirth();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    BirthCertificate.RequestTrnHeaderType headerType = new BirthCertificate.RequestTrnHeaderType();
                    headerType.TransactionId = LoadTransId();
                    headerType.EntityCode = "MOCD";
                    client.getBirthCertificatePullList(headerType, pullListRequestType, out pullListResponse);
                }
                return pullListResponse.Contents;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<MOCDIntegrations.BirthCertificate.BirthCertificatePullDetailsResponse> SaveDependentInformation(BirthCertificatePullListResponseType[] familyDetail, string emiratesID)
        {
            try
            {
                List<MOCDIntegrations.BirthCertificate.BirthCertificatePullDetailsResponse> list = new List<BirthCertificatePullDetailsResponse>();
                BirthCertificate.BirthCertificatePullPortClient client = new BirthCertificate.BirthCertificatePullPortClient();
                BirthCertificate.BirthCertificatePullDetailsRequestType pullRequestType = new BirthCertificate.BirthCertificatePullDetailsRequestType();
                BirthCertificate.BirthCertificatePullDetailsResponse pullResponse = new BirthCertificate.BirthCertificatePullDetailsResponse();


                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {

                    foreach (BirthCertificatePullListResponseType item in familyDetail)
                    {
                        var httpRequestProperty = new HttpRequestMessageProperty();
                        httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateTokenForbirth();
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                        BirthCertificate.RequestTrnHeaderType headerType = new BirthCertificate.RequestTrnHeaderType();
                        headerType.TransactionId = LoadTransId();
                        headerType.EntityCode = "MOCD";
                        pullRequestType.BirthCertificateID = item.BirthCertificateID;
                        pullRequestType.EmiratesID = emiratesID;
                        client.getBirthCertificatePullDetails(headerType, pullRequestType, out pullResponse);
                        list.Add(pullResponse);
                        return list;
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;

        }
        public static string LoadTransId()
        {
            return "MOCD_ICA_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }
        public static string GenerateTokenForbirth()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["birthUri"].ToString(), ConfigurationManager.AppSettings["birthgrant_type"].ToString(), ConfigurationManager.AppSettings["birthclient_id"].ToString(), ConfigurationManager.AppSettings["birthclient_secret"].ToString(), ConfigurationManager.AppSettings["birthscope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
