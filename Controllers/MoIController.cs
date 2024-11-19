using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace MOCDIntegrations.Controllers
{
    public class MoIController : Controller
    {
        // GET: MoI
        public ActionResult Index()
        {
            return View("MoI");
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
            return "MOCD_MOI_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
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
                var input = new JavaScriptSerializer().Deserialize<MoIDetails.MoIDetailsRequestParams>(postdata);

                MoIService.TrnHeaderType objHeader = new MoIService.TrnHeaderType();
                objHeader.transactionId = LoadTransId();
                objHeader.serviceProviderEntity = "MOCD";

                MoIService.InquiryCriteriaType objInquiry = null;
                MoIService.personCIInquiry_pttClient client = new MoIService.personCIInquiry_pttClient();

                MoIService.LookupType objlkp = null;


                List<MoIDetails.MoIDetailsResponseParams> lstResponseParams = new List<MoIDetails.MoIDetailsResponseParams>();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    MoIService.ErrorType objError = new MoIService.ErrorType();

                    objlkp = new MoIService.LookupType();
                    objInquiry = new MoIService.InquiryCriteriaType();
                    objInquiry.Item = input.EmiratesID;

                    MoIDetails.MoIDetailsResponseParams objResponse = new MoIDetails.MoIDetailsResponseParams();
                    MoIService.PersonCIDetailsType objCIDetails = client.getPersonCIDetails(ref objHeader, objInquiry);
                    if (objCIDetails != null)
                    {
                        objResponse.isPersonHasBeenImprisoned = objCIDetails.isPersonHasBeenImprisoned;
                        objlkp = objCIDetails.personCertificateStatus;
                        objResponse.personCertificateStatusId = objlkp.id;
                        objResponse.personCertificateStatusAr = objlkp.arDesc;
                        objResponse.personCertificateStatusEn = objlkp.enDesc;
                        objResponse.isPersonHasTreatment = objCIDetails.isPersonHasTreatment;
                        objResponse.isPersonExitCountry = objCIDetails.isPersonExitCountry;
                        objResponse.EmiratesId = input.EmiratesID;

                        lstResponseParams.Add(objResponse);
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<MoIDetails.MoIDetailsResponseParams>>(lstResponseParams), ConfigurationManager.AppSettings["MOICode"].ToString(), ConfigurationManager.AppSettings["MOI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOICode"].ToString(), ConfigurationManager.AppSettings["MOI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }


            }
            catch (System.ServiceModel.FaultException faultException)
            {
                var fault = faultException.CreateMessageFault();
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
                                innerdoc.LoadXml(child.InnerXml);
                                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                {
                                    if (chd.Name == "arDesc")
                                    {
                                        str += chd.InnerText + " - ";
                                    }
                                    if (chd.Name == "enDesc")
                                    {
                                        str += chd.InnerText;
                                    }

                                    if (chd.Name == "details")
                                    {
                                        innersdoc.LoadXml(chd.InnerXml);
                                        foreach (XmlNode chds in innersdoc.DocumentElement.ChildNodes)
                                        {
                                            if (chds.Name == "message")
                                            {
                                                str += chd.InnerText;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        ResponseDescription = str;
                    }

                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOICode"].ToString(), ConfigurationManager.AppSettings["MOI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOICode"].ToString(), ConfigurationManager.AppSettings["MOI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException wex)
            {
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                flag = 3;
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOICode"].ToString(), ConfigurationManager.AppSettings["MOI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOICode"].ToString(), ConfigurationManager.AppSettings["MOI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}