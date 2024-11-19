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
    public class UMMDEDController : Controller
    {
        // GET: UMMDED
        public ActionResult Index()
        {
            return View("UMMDED");
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
                var input = new JavaScriptSerializer().Deserialize<UMMDEDDetails.UMMDEDDetailsRequestParams>(postdata);
                UMMDEDService.UMM_DED_getLicenseDetailsbyEIDPortTypeClient client = new UMMDEDService.UMM_DED_getLicenseDetailsbyEIDPortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    UMMDEDService.LicenseDetails objLicenseDetails = new UMMDEDService.LicenseDetails();
                    objLicenseDetails.EmiratesID = input.EmiratesId;

                    UMMDEDService.LicenseDetails_Input licenseDetails_Input = new UMMDEDService.LicenseDetails_Input();
                    licenseDetails_Input.LicenseDetails = objLicenseDetails;

                    UMMDEDService.LicenseDetailsResponse objLicenseDetailsResponse = client.LicenseDetails(objLicenseDetails);

                    UMMDEDService.LicenseDetailsResponseLicenseDetails[] lstLicenseDetailsResponseLicenseDetails = null;

                    UMMDEDDetails.UMMDEDDetailsResponseParams objUMMDEDDetailsResponseParams = new UMMDEDDetails.UMMDEDDetailsResponseParams();

                    UMMDEDDetails.OwnerDetails objOwnerDetails = new UMMDEDDetails.OwnerDetails();
                    UMMDEDDetails.BusinessActivity objBusinessActivity = new UMMDEDDetails.BusinessActivity();

                    List<UMMDEDDetails.OwnerDetails> lstOwnerDetails = null;
                    List<UMMDEDDetails.BusinessActivity> lstBusinessActivity = null;


                    UMMDEDService.LicenseDetailsResponseError[] LstLicenseDetailsResponseError = objLicenseDetailsResponse.Error;

                    foreach (UMMDEDService.LicenseDetailsResponseError objLicenseDetailsResponseError in LstLicenseDetailsResponseError)
                    {
                        if (objLicenseDetailsResponseError.ErrorCode == "FCSA-200")
                        {
                            lstOwnerDetails = new List<UMMDEDDetails.OwnerDetails>();
                            lstBusinessActivity = new List<UMMDEDDetails.BusinessActivity>();

                            lstLicenseDetailsResponseLicenseDetails = objLicenseDetailsResponse.LicenseDetails;

                            foreach (UMMDEDService.LicenseDetailsResponseLicenseDetails objLicenseDetailsResponseLicenseDetails in lstLicenseDetailsResponseLicenseDetails)
                            {
                                objUMMDEDDetailsResponseParams.BusinessLicenseID = objLicenseDetailsResponseLicenseDetails.BusinessLicenseID;
                                objUMMDEDDetailsResponseParams.BusinessNameEN = objLicenseDetailsResponseLicenseDetails.BusinessNameEN;
                                objUMMDEDDetailsResponseParams.BusinessNameAR = objLicenseDetailsResponseLicenseDetails.BusinessNameAR;
                                objUMMDEDDetailsResponseParams.EstablishmentDate = objLicenseDetailsResponseLicenseDetails.EstablishmentDate;
                                objUMMDEDDetailsResponseParams.ExpiryDate = objLicenseDetailsResponseLicenseDetails.ExpiryDate;

                                foreach (UMMDEDService.LicenseDetailsResponseLicenseDetailsOwnerDetails objOwner in objLicenseDetailsResponseLicenseDetails.OwnerDetails)
                                {
                                    objOwnerDetails.OwnerEmirateID = objOwner.OwnerEmirateID;
                                    objOwnerDetails.OwnerFulNameAR = objOwner.OwnerFulNameAR;
                                    objOwnerDetails.OwnerFulNameEN = objOwner.OwnerFulNameEN;
                                    objOwnerDetails.OwnerRoleAR = objOwner.OwnerRoleAR;
                                    objOwnerDetails.OwnerRoleEN = objOwner.OwnerRoleEN;
                                    objOwnerDetails.OwnershipPercentage = objOwner.OwnershipPercentage;
                                    lstOwnerDetails.Add(objOwnerDetails);
                                }

                                foreach (UMMDEDService.LicenseDetailsResponseLicenseDetailsBusinessActivity objActivity in objLicenseDetailsResponseLicenseDetails.BusinessActivity)
                                {
                                    objBusinessActivity.BusinessActivityDescAR = objActivity.BusinessActivityDescAR;
                                    objBusinessActivity.BusinessActivityDescEN = objActivity.BusinessActivityDescEN;
                                    lstBusinessActivity.Add(objBusinessActivity);
                                }


                            }

                            objUMMDEDDetailsResponseParams.OwnerDetails = lstOwnerDetails;
                            objUMMDEDDetailsResponseParams.BusinessActivity = lstBusinessActivity;


                            flag = 1;
                            json = JsonConvert.SerializeObject(new { objUMMDEDDetailsResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<UMMDEDDetails.UMMDEDDetailsResponseParams>(objUMMDEDDetailsResponseParams), ConfigurationManager.AppSettings["UMMDEDCode"].ToString(), ConfigurationManager.AppSettings["UMMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = objLicenseDetailsResponseError.ErrorMessage;
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["UMMDEDCode"].ToString(), ConfigurationManager.AppSettings["UMMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["UMMDEDCode"].ToString(), ConfigurationManager.AppSettings["UMMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    ResponseDescription = faultException.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["UMMDEDCode"].ToString(), ConfigurationManager.AppSettings["UMMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["UMMDEDCode"].ToString(), ConfigurationManager.AppSettings["UMMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["UMMDEDCode"].ToString(), ConfigurationManager.AppSettings["UMMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}