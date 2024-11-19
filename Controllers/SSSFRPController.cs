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
    public class SSSFRPController : Controller
    {
        // GET: SSSFRP
        public ActionResult Index()
        {
            return View("SSSFRP");
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

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<SSSFRPDetails.SSSFRPRequestParams>(postdata);
                SSSFRPService.SHJ_SSSF_getRetirementPensionerbyEIDPortTypeClient client = new SSSFRPService.SHJ_SSSF_getRetirementPensionerbyEIDPortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    SSSFRPDetails.SSSFRPResponseParams objResponseParams = null;
                    List<SSSFRPDetails.SSSFRPResponseParams> lstResponseParams = new List<SSSFRPDetails.SSSFRPResponseParams>();

                    SSSFRPService.RetirementPensioner objInput = new SSSFRPService.RetirementPensioner();
                    objInput.EmiratesID = input.EmiratesID;

                    SSSFRPService.RetirementPensionerResponse objRetirementPensionerResponse = client.RetirementPensioner(objInput);
                    SSSFRPService.RetirementPensionerResponseError[] lstError = objRetirementPensionerResponse.Error;

                    if (lstError != null)
                        foreach (SSSFRPService.RetirementPensionerResponseError objError in lstError)
                        {
                            if (objError.ErrorCode == "FCSA-200")
                            {
                                foreach (SSSFRPService.RetirementPensionerResponseRetirementPensionerDetails objResponse in objRetirementPensionerResponse.RetirementPensionerDetails)
                                {
                                    objResponseParams = new SSSFRPDetails.SSSFRPResponseParams();
                                    objResponseParams.RetirementPensionersEmiratesID = objResponse.RetirementPensionersEmiratesID;
                                    objResponseParams.RetirementPensioners_AR = objResponse.RetirementPensioners_AR;
                                    objResponseParams.RetirementPensioners_EN = objResponse.RetirementPensioners_EN;
                                    objResponseParams.RetirementPensionersStartDate = objResponse.RetirementPensionersStartDate;
                                    objResponseParams.RetirementPensionersEndDate = objResponse.RetirementPensionersEndDate;
                                    lstResponseParams.Add(objResponseParams);
                                }

                                flag = 1;
                                json = JsonConvert.SerializeObject(new { lstResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<SSSFRPDetails.SSSFRPResponseParams>>(lstResponseParams), ConfigurationManager.AppSettings["SSSFRPCode"].ToString(), ConfigurationManager.AppSettings["SSSFRP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = objError.ErrorDescription;
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SSSFRPCode"].ToString(), ConfigurationManager.AppSettings["SSSFRP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }

                        }
                }
            }
            catch (FaultException faultException)
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
                                    // str += "Contact GSB Support.";
                                }
                            }

                            if (child.Name == "Detail")
                            {
                                //innerdoc.LoadXml(child.InnerXml);
                                //foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                //{
                                //    if (chd.Name == "errorMessageArField")
                                //    {
                                //        str += chd.InnerText + " - ";
                                //    }
                                //    if (chd.Name == "errorMessageEnField")
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

                                str += child.InnerXml;

                            }
                            ResponseDescription += str;
                        }

                    }

                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SSSFRPCode"].ToString(), ConfigurationManager.AppSettings["SSSFRP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    ResponseDescription = faultException.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SSSFRPCode"].ToString(), ConfigurationManager.AppSettings["SSSFRP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SSSFRPCode"].ToString(), ConfigurationManager.AppSettings["SSSFRP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SSSFRPCode"].ToString(), ConfigurationManager.AppSettings["SSSFRP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}