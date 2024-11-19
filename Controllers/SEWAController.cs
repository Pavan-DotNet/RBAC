using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Controllers
{
    public class SEWAController : Controller
    {
        // GET: SEWA
        public ActionResult Index()
        {
            return View("SEWA");
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

        public JsonResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            var Eflag = "";

            SEWADetails.SEWAResponse objResponse = null;
            List<SEWADetails.SEWAResponse> lstSEWAResponse = new List<SEWADetails.SEWAResponse>();

            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                var input = new JavaScriptSerializer().Deserialize<SEWADetails.SEWARequest>(postdata);

                JsonHelper objHelper = new JsonHelper();
                
                string DATA = @"{""EmiratesId"":" + (input.EmiratesId == string.Empty ? "null" : input.EmiratesId) + @", ""LSocialSecurityNo"":" + (input.LSocialSecurityNo.Trim() == string.Empty ? "null" : input.LSocialSecurityNo.Trim() ) + @", ""FSocialSecurityNo"":" + (input.FSocialSecurityNo.Trim() == string.Empty ? "null" : input.FSocialSecurityNo.Trim()) + " }";


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["SEWA_Url"].ToString());
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = DATA.Length;

                request.Headers.Add("Authorization", "Bearer " + GenerateToken());
                request.ServicePoint.Expect100Continue = false;
                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(DATA);
                }

                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {

                    string response = responseReader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<List<SEWADetails.SEWAResponse>>(response);
                    Eflag = "0";
                    foreach (SEWADetails.SEWAResponse objResp in result)
                    {
                        if (objResp.errorCode == "0" && objResp.responseCode == "0")
                            Eflag = "1";

                        objResponse = new SEWADetails.SEWAResponse();
                        objResponse.accT_ID = objResp.accT_ID;
                        objResponse.addresS1 = objResp.addresS1;
                        objResponse.addresS2 = objResp.addresS2;
                        objResponse.addresS3 = objResp.addresS3;
                        objResponse.areA_DESC = objResp.areA_DESC;
                        objResponse.conS_NUM = objResp.conS_NUM;
                        objResponse.discounT_PERCENTAGE = objResp.discounT_PERCENTAGE;
                        objResponse.discounT_START_DT = objResp.discounT_START_DT;
                        objResponse.errorCode = objResp.errorCode;
                        objResponse.owneR_EMAIL = objResp.owneR_EMAIL;
                        objResponse.owneR_EMIRATES_ID = objResp.owneR_EMIRATES_ID;
                        objResponse.owneR_MOBILE = objResp.owneR_MOBILE;
                        objResponse.owneR_NAME = objResp.owneR_NAME;
                        objResponse.owneR_SHJ_SOCIAL_SEC_NUM = objResp.owneR_SHJ_SOCIAL_SEC_NUM;
                        objResponse.owneR_SOCIAL_SEC_NUM = objResp.owneR_SOCIAL_SEC_NUM;
                        objResponse.preM_TYPE = objResp.preM_TYPE;
                        objResponse.responseCode = objResp.responseCode;
                        objResponse.responseMessage = objResp.responseMessage;
                        objResponse.sitE_OFFICE = objResp.sitE_OFFICE;
                        objResponse.tenanT_EMAIL = objResp.tenanT_EMAIL;
                        objResponse.tenanT_EMIRATES_ID = objResp.tenanT_EMIRATES_ID;
                        objResponse.tenanT_MOBILE = objResp.tenanT_MOBILE;
                        objResponse.tenanT_NAME = objResp.tenanT_NAME;
                        objResponse.tenanT_SHJ_SOCIAL_SEC_NUM = objResp.tenanT_SHJ_SOCIAL_SEC_NUM;
                        objResponse.tenanT_SPL_NEED = objResp.tenanT_SPL_NEED;
                        lstSEWAResponse.Add(objResponse);

                    }

                    if(Eflag =="1")
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstSEWAResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<SEWADetails.SEWAResponse>>(lstSEWAResponse), ConfigurationManager.AppSettings["SEWACode"].ToString(), ConfigurationManager.AppSettings["SEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else if (Eflag == "0")
                    {
                        flag = 2;
                        string ResponseDescription = string.Empty;
                        foreach (var item in lstSEWAResponse)
                        {
                            if(item.errorCode != "0" && item.responseCode != "0")
                            ResponseDescription += item.errorCode + " - " + item.responseCode + "-" +  item.responseMessage;
                        }
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SEWACode"].ToString(), ConfigurationManager.AppSettings["SEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }

            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, resp, ConfigurationManager.AppSettings["SEWACode"].ToString(), ConfigurationManager.AppSettings["SEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["SEWACode"].ToString(), ConfigurationManager.AppSettings["SEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}