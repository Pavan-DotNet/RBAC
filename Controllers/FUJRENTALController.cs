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
    public class FUJRENTALController : Controller
    {
        // GET: FUJRENTAL
        public ActionResult Index()
        {
            return View("FUJRENTAL");
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

        public ActionResult Search(string postdata , string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<FUJRENTALDetails.FUJRENTALDetailsRequestParams>(postdata);
                FUJRentalService.FUJ_MUN_getRentalDetailsbyEIDPortTypeClient client = new FUJRentalService.FUJ_MUN_getRentalDetailsbyEIDPortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    FUJRENTALDetails.RentalDetails objRental = null;
                    List<FUJRENTALDetails.RentalDetails> lstResponseParams = new List<FUJRENTALDetails.RentalDetails>();

                    FUJRentalService.GetRentalDetails objInput = new FUJRentalService.GetRentalDetails();
                    objInput.EmiratesID = input.EmiratesID;

                    FUJRentalService.GetRentalDetailsResponse objGetRentalDetailsResponse = client.GetRentalDetails(objInput);
                    FUJRentalService.GetRentalDetailsResponseError objError = objGetRentalDetailsResponse.Error;

                    if (objError != null)
                        if (objError.ErrorCode == "FCSA-200")
                        {
                            foreach (FUJRentalService.GetRentalDetailsResponseGetRentalDetails objResponse in objGetRentalDetailsResponse.getRentalDetails)
                            {
                                objRental = new FUJRENTALDetails.RentalDetails();
                                objRental.EmiratesID = objResponse.EmiratesId;
                                objRental.RealEstateOwner = objResponse.RealEstateOwner;
                                objRental.OwnershipPercentage = objResponse.OwnershipPercentage;
                                objRental.RealEstateLocation = objResponse.RealEstateLocation;
                                objRental.RealEstateNumber = objResponse.RealEstateNumber;
                                objRental.OwnershipDate = objResponse.OwnershipDate;
                                objRental.RealEstateType = objResponse.RealEstateType;
                                objRental.RealEstateStatus = objResponse.RealEstateStatus;
                                objRental.RentAmount = objResponse.RentAmount;
                                objRental.RentalStartDate = objResponse.RentalStartDate;
                                objRental.RentalEndDate = objResponse.RentalEndDate;
                                objRental.LastRefreshDate = objResponse.LastRefreshDate;
                                lstResponseParams.Add(objRental);
                            }

                            flag = 1;
                            json = JsonConvert.SerializeObject(new { lstResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<FUJRENTALDetails.RentalDetails>>(lstResponseParams), ConfigurationManager.AppSettings["FUJRENTALCode"].ToString(), ConfigurationManager.AppSettings["FUJRENTAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = objError.ErrorDescription;
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJRENTALCode"].ToString(), ConfigurationManager.AppSettings["FUJRENTAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJRENTALCode"].ToString(), ConfigurationManager.AppSettings["FUJRENTAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    ResponseDescription = faultException.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJRENTALCode"].ToString(), ConfigurationManager.AppSettings["FUJRENTAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJRENTALCode"].ToString(), ConfigurationManager.AppSettings["FUJRENTAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FUJRENTALCode"].ToString(), ConfigurationManager.AppSettings["FUJRENTAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}