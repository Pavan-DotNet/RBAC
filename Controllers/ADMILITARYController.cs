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
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Controllers
{
    public class ADMILITARYController : Controller
    {
        // GET: ADMILITARY
        public ActionResult Index()
        {
            return View("");
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
        private string LoadTransId()
        {
            return "MOCD_ADMP_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }
        public JsonResult Search(string postdata, string UserAgent)
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
                var input = new JavaScriptSerializer().Deserialize<ADMILITARYDetails.ADMilitaryPensionRequestParams>(postdata);
                ADMilitaryPensionService.MocdWSClient client = new ADMilitaryPensionService.MocdWSClient();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    ADMilitaryPensionService.HREmployeeDetailsInp objADPension = null;
                    ADMilitaryPensionService.HREmployeeDetailsInp[] ADPensionRequest = new ADMilitaryPensionService.HREmployeeDetailsInp[1];

                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    objADPension = new ADMilitaryPensionService.HREmployeeDetailsInp();
                    objADPension.NationalID = input.EmiratesID;
                    ADPensionRequest[0] = objADPension;

                    ADMilitaryPensionService.SoapHeaderReq ADAuthreq = new ADMilitaryPensionService.SoapHeaderReq();
                    ADAuthreq.serviceProviderEntity = "MOCD";
                    ADAuthreq.transactionId = LoadTransId();

                    ADMilitaryPensionService.SoapHeaderRequest ADAuth = new ADMilitaryPensionService.SoapHeaderRequest();
                    ADAuth.CustomHeaderReq = ADAuthreq;
                    ADMilitaryPensionService.HREmployeeDetailsOut objOutput = client.GetHREmployeeDetails(ADAuth, ADPensionRequest);

                    if (objOutput.StatusHeader.StatusCode == "0")
                    {
                        if (objOutput != null)
                        {
                            ADMILITARYDetails.ADMilitaryPensionResponseParams objMilitaryPensionResponseParams = null;
                            List<ADMILITARYDetails.ADMilitaryPensionResponseParams> lstMilitaryPensionResponseParams = new List<ADMILITARYDetails.ADMilitaryPensionResponseParams>();
                            List<ADMILITARYDetails.ADMilitaryPensionRequestParams> lstADMilitaryRequestParamsFinal = new List<ADMILITARYDetails.ADMilitaryPensionRequestParams>();


                            foreach (ADMilitaryPensionService.HREmployeeInfo obj in objOutput.HREmployeeDetailsInfo)
                            {

                                if (!string.IsNullOrEmpty(obj.PensionerId) && obj.PensionerId != "0")
                                {
                                    flag = 1;
                                    objMilitaryPensionResponseParams = new ADMILITARYDetails.ADMilitaryPensionResponseParams();
                                    objMilitaryPensionResponseParams.PensionerId = obj.PensionerId;
                                    objMilitaryPensionResponseParams.BeneficiaryId = obj.BeneficiaryId;
                                    objMilitaryPensionResponseParams.FullNameArabic = obj.FullNameArabic;
                                    objMilitaryPensionResponseParams.FullNameEnglish = obj.FullNameEnglish;
                                    objMilitaryPensionResponseParams.PersonType = obj.PersonType;
                                    objMilitaryPensionResponseParams.TownNumber = obj.TownNumber;
                                    objMilitaryPensionResponseParams.FamilyNumber = obj.FamilyNumber;
                                    objMilitaryPensionResponseParams.NationalIdentifier = obj.NationalIdentifier;
                                    objMilitaryPensionResponseParams.Status = obj.Status;
                                    objMilitaryPensionResponseParams.MonthlyPension = obj.MonthlyPension;
                                    objMilitaryPensionResponseParams.PensionStartDate = obj.PensionStartDate;
                                    objMilitaryPensionResponseParams.AliveHeirFlag = obj.AliveHeirFlag;
                                    objMilitaryPensionResponseParams.BeneficiaryName = obj.BeneficiaryName;
                                    objMilitaryPensionResponseParams.BeneficiaryRelation = obj.BeneficiaryRelation;
                                    objMilitaryPensionResponseParams.BeneficiaryStartDate = obj.BeneficiaryStartDate;
                                    objMilitaryPensionResponseParams.EntityId = IEUtils.ToInt(obj.EntityId);
                                    objMilitaryPensionResponseParams.DOB = obj.DOB;
                                    lstMilitaryPensionResponseParams.Add(objMilitaryPensionResponseParams);

                                }
                              
                            }

                            if (lstMilitaryPensionResponseParams != null && lstMilitaryPensionResponseParams.Count > 0)
                            {
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { lstMilitaryPensionResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<ADMILITARYDetails.ADMilitaryPensionResponseParams>>(lstMilitaryPensionResponseParams), ConfigurationManager.AppSettings["ADMILITARYCode"].ToString(), ConfigurationManager.AppSettings["ADMILITARY"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = "No Matching Records available";
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADMILITARYCode"].ToString(), ConfigurationManager.AppSettings["ADMILITARY"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }

                        }

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = objOutput.StatusHeader.StatusDesc;
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADMILITARYCode"].ToString(), ConfigurationManager.AppSettings["ADMILITARY"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { resp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, resp, ConfigurationManager.AppSettings["ADMILITARYCode"].ToString(), ConfigurationManager.AppSettings["ADMILITARY"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["ADMILITARYCode"].ToString(), ConfigurationManager.AppSettings["ADMILITARY"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
    }
}