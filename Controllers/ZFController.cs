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
using System.Xml;

namespace MOCDIntegrations.Controllers
{
    public class ZFController : Controller
    {
        // GET: ZF
        public ActionResult Index()
        {
            return View("ZF");
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
                var input = new JavaScriptSerializer().Deserialize<ZFDetails.ZFDetailsRequest>(postdata);
                ZFService.ZakatFundSrvClient client = new ZFService.ZakatFundSrvClient();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {

                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    ZFService.CredentialInput objInput = new ZFService.CredentialInput();
                    ZFService.CustomerInput objCInput = new ZFService.CustomerInput();
                    ZFService.CustomerCredential objCCInput = new ZFService.CustomerCredential();
                    objCCInput.SecurityToken = "";


                    objCInput.IdentityNumber = input.EmiratesID; //"922222202220222";
                    objInput.SearchCriteria = objCInput;
                    objInput.credential = objCCInput;

                    ZFService.AppData objAppData = null;

                    ZFService.Resultmsg result = client.GetData(objInput.SearchCriteria, objInput.credential, out objAppData);

                    List<ZFDetails.ZFDetailsResponse> lstResponse = new List<ZFDetails.ZFDetailsResponse>();
                    ZFDetails.ZFDetailsResponse objResponse = null;

                    if (result.Status == 0)
                    {
                        objResponse = new ZFDetails.ZFDetailsResponse();
                        objResponse.ApplicationDate = objAppData.ApplicationDateSpecified ? objAppData.ApplicationDate.ToString() : string.Empty;
                        objResponse.ApprovalDate = objAppData.ApprovalDateSpecified ? objAppData.ApplicationDate.ToString() : string.Empty;
                        objResponse.BeneficiaryName = objAppData.BeneficiaryName;
                        objResponse.CaseTypeAr = objAppData.CaseTypeAr;
                        objResponse.CaseTypeEn = objAppData.CaseTypeEn;
                        objResponse.ClassificationAr = objAppData.ClassificationAr;
                        objResponse.ClassificationEn = objAppData.ClassificationEn;
                        objResponse.ContainerAr = objAppData.ContainerAr;
                        objResponse.ContainerEn = objAppData.ContainerEn;
                        objResponse.FamilyNo = objAppData.FamilyNo.ToString();
                        objResponse.FileNo = objAppData.FileNo;
                        objResponse.FileStatusAr = objAppData.FileStatusAr;
                        objResponse.FileStatusEn = objAppData.FileStatusEn;
                        objResponse.HelpAmount = objAppData.HelpAmountSpecified ? objAppData.HelpAmount.ToString() : string.Empty;
                        objResponse.HelpTypeAr = objAppData.HelpTypeAr;
                        objResponse.HelpTypeEn = objAppData.HelpTypeEn;
                        objResponse.IdentityNumber = objAppData.IdentityNumber;
                        objResponse.MobileNo = objAppData.MobileNo;
                        objResponse.NameAr = objAppData.NameAr;
                        objResponse.NameEn = objAppData.NameEn;
                        objResponse.NoOfApplications = objAppData.NoOfApplicationsSpecified ? objAppData.NoOfApplications.ToString() : string.Empty;
                        objResponse.NoOfInstallment = objAppData.NoOfInstallmentSpecified ? objAppData.NoOfInstallment.ToString() : string.Empty;

                        lstResponse.Add(objResponse);

                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<ZFDetails.ZFDetailsResponse>>(lstResponse), ConfigurationManager.AppSettings["ZKFUNDCode"].ToString(), ConfigurationManager.AppSettings["ZKFUND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = result.Message;
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ZKFUNDCode"].ToString(), ConfigurationManager.AppSettings["ZKFUND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
            }
            catch (FaultException fex)
            {
                var fault = fex.CreateMessageFault();
                var doc = new XmlDocument();
                var innerdoc = new XmlDocument();
                var innersdoc = new XmlDocument();
                var nav = doc.CreateNavigator();
                flag = 3;
                string ResponseDescription = string.Empty;
                if (fault.HasDetail)
                {
                    if (nav != null)
                    {
                        using (var writer = nav.AppendChild())
                        {
                            fault.WriteTo(writer, EnvelopeVersion.Soap12);
                        }

                        string str = string.Empty; //do something with it
                        foreach (XmlNode child in doc.DocumentElement.ChildNodes)
                        {

                            if (child.Name == "Code")
                            {
                                innerdoc.LoadXml(child.InnerXml);
                                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                {
                                    str += "Contact GSB Support.";
                                }
                            }

                            if (child.Name == "Detail")
                            {
                                //innerdoc.LoadXml(child.InnerXml);
                                //foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                //{
                                //    if (chd.Name == "arDesc")
                                //    {
                                //        str += chd.InnerText + " - ";
                                //    }
                                //    if (chd.Name == "enDesc")
                                //    {
                                //        str += chd.InnerText;
                                //    }

                                //    if (chd.Name == "details")
                                //    {
                                //        innersdoc.LoadXml(chd.InnerXml);
                                //        foreach (XmlNode chds in innersdoc.DocumentElement.ChildNodes)
                                //        {
                                //            if (chds.Name == "message")
                                //            {
                                //                str += chd.InnerText;
                                //            }
                                //        }
                                //    }
                                //}
                                ResponseDescription = child.InnerXml;
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }); ;
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ZKFUNDCode"].ToString(), ConfigurationManager.AppSettings["ZKFUND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ZKFUNDCode"].ToString(), ConfigurationManager.AppSettings["ZKFUND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ZKFUNDCode"].ToString(), ConfigurationManager.AppSettings["ZKFUND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ZKFUNDCode"].ToString(), ConfigurationManager.AppSettings["ZKFUND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
    }
}
