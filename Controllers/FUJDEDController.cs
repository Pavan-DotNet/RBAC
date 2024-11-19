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
    public class FUJDEDController : Controller
    {
        // GET: FUJDED
        public ActionResult Index()
        {
            return View("FUJDED");
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

        public ActionResult Search(string postdata ,string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<FUJDEDDetails.FUJDEDDetailsRequestParams>(postdata);
                FUJDEDService.FUJ_DED_getTradeLicenseDetailsbyEIDPortTypeClient client = new FUJDEDService.FUJ_DED_getTradeLicenseDetailsbyEIDPortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    FUJDEDDetails.LicenseDetails objLicense = null;
                    List<FUJDEDDetails.LicenseDetails> lstResponseParams = new List<FUJDEDDetails.LicenseDetails>();

                    FUJDEDService.GetTradeLicenseDetails objInput = new FUJDEDService.GetTradeLicenseDetails();
                    objInput.EmiratesID = input.EmiratesID;

                    FUJDEDService.GetTradeLicenseDetailsResponse objGetTradeLicenseDetailsResponse = client.GetTradeLicenseDetails(objInput);
                    FUJDEDService.GetTradeLicenseDetailsResponseError objError = objGetTradeLicenseDetailsResponse.Error;

                    if (objError != null)
                        if (objError.ErrorCode == "FCSA-200")
                        {
                            foreach (FUJDEDService.GetTradeLicenseDetailsResponseTradeLicenseDetails objResponse in objGetTradeLicenseDetailsResponse.TradeLicenseDetails)
                            {
                                objLicense = new FUJDEDDetails.LicenseDetails();
                                objLicense.EmiratesID = objResponse.EmiratesID;
                                objLicense.LicenseHolderName = objResponse.LicenseHolderName;
                                objLicense.LicenseHolderCategory = objResponse.LicenseHolderCategory;
                                objLicense.PartnershipRatio = objResponse.PartnershipRatio;
                                objLicense.LicenseIssuanceDate = objResponse.LicenseIssuanceDate;
                                objLicense.LicenseExpiryDate = objResponse.LicenseExpiryDate;
                                objLicense.LicenseNumber = objResponse.LicenseNumber;
                                objLicense.LicenseName = objResponse.LicenseName;
                                objLicense.LastRefreshDate = objResponse.LastRefreshDate;
                                lstResponseParams.Add(objLicense);
                            }

                            flag = 1;
                            json = JsonConvert.SerializeObject(new { lstResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<FUJDEDDetails.LicenseDetails>>(lstResponseParams), ConfigurationManager.AppSettings["FUJDEDCode"].ToString(), ConfigurationManager.AppSettings["FUJDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = objError.ErrorDescription;
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJDEDCode"].ToString(), ConfigurationManager.AppSettings["FUJDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJDEDCode"].ToString(), ConfigurationManager.AppSettings["FUJDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    ResponseDescription = faultException.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJDEDCode"].ToString(), ConfigurationManager.AppSettings["FUJDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJDEDCode"].ToString(), ConfigurationManager.AppSettings["FUJDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJDEDCode"].ToString(), ConfigurationManager.AppSettings["FUJDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}