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
    public class MoEcController : Controller
    {
        // GET: MoEc
        public ActionResult Index()
        {
            return View("MoEc");
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

        public ActionResult Search(string index, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<MoEcDetails.MoEcRequestParams>(postdata);
                MoEcService.MOE_getTradeLicenseBasicByEIDPPPortTypeClient client = new MoEcService.MOE_getTradeLicenseBasicByEIDPPPortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    MoEcDetails.MoEcResponseParams objResponseParams = new MoEcDetails.MoEcResponseParams();

                    MoEcDetails.OwnerDetails objOwnerDetails = null;
                    MoEcDetails.BusinessLicenses objBusinessLicenses = null;
                    MoEcDetails.OwnerPartnership objOwnerPartnership = null;
                    List<MoEcDetails.OwnerDetails> lstOwnerDetails = new List<MoEcDetails.OwnerDetails>();
                    List<MoEcDetails.BusinessLicenses> lstBusinessLicenses = new List<MoEcDetails.BusinessLicenses>();
                    List<MoEcDetails.OwnerPartnership> lstOwnerPartnership = new List<MoEcDetails.OwnerPartnership>();

                    MoEcService.GetTradeLicenseBasicByEIDPP objInput = new MoEcService.GetTradeLicenseBasicByEIDPP();
                    if (index == "2")
                        objInput.EmiratesID = input.EmiratesID;
                    else if (index == "3")
                        objInput.Passport = input.Passport;

                    MoEcService.GetTradeLicenseBasicByEIDPPResponse objGetTradeLicenseBasicByEIDPPResponse = client.GetTradeLicenseBasicByEIDPP(objInput);
                    MoEcService.GetTradeLicenseBasicByEIDPPResponseError objError = objGetTradeLicenseBasicByEIDPPResponse.Error;

                    if (objError.ErrorCode == "FCSA-200")
                    {
                        foreach (MoEcService.GetTradeLicenseBasicByEIDPPResponseOwnerDetails objResponse in objGetTradeLicenseBasicByEIDPPResponse.OwnerDetails)
                        {
                            objOwnerDetails = new MoEcDetails.OwnerDetails();
                            objOwnerDetails.OwnerEmiratesID = objResponse.OwnerEmiratesID;
                            objOwnerDetails.OwnerNameAR = objResponse.OwnerNameAR;
                            objOwnerDetails.OwnerNameEN = objResponse.OwnerNameEN;
                            objOwnerDetails.OwnerPassport = objResponse.OwnerPassport;
                            lstOwnerDetails.Add(objOwnerDetails);

                            foreach (MoEcService.GetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses in objResponse.BusinessLicenses)
                            {
                                objBusinessLicenses = new MoEcDetails.BusinessLicenses();
                                objBusinessLicenses.LicenseNumber = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.LicenseNumber;
                                objBusinessLicenses.LicenseStatusAR = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.LicenseStatusAR;
                                objBusinessLicenses.LicenseStatusEN = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.LicenseStatusEN;
                                objBusinessLicenses.LicenseRegistrationDate = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.LicenseRegistrationDate;
                                objBusinessLicenses.LicenseExpiryDate = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.LicenseExpiryDate;
                                objBusinessLicenses.EmirateEN = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.EmirateEN;
                                objBusinessLicenses.EmirateAR = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.EmirateAR;
                                objBusinessLicenses.BusinessNameAr = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.BusinessNameAr;
                                objBusinessLicenses.BusinessNameEn = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.BusinessNameEn;
                                objBusinessLicenses.EconomicDepartmentAR = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.EconomicDepartmentAR;
                                objBusinessLicenses.EconomicDepartmentEN = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.EconomicDepartmentEN;
                                objBusinessLicenses.LicenseFCRNumber = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.LicenseFCRNumber;
                                lstBusinessLicenses.Add(objBusinessLicenses);

                                foreach (MoEcService.GetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicensesOwnerPartnership objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicensesOwnerPartnership in objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicenses.OwnerPartnership)
                                {
                                    objOwnerPartnership = new MoEcDetails.OwnerPartnership();
                                    objOwnerPartnership.OwnerPartnershipTypeAR = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicensesOwnerPartnership.OwnerPartnershipTypeAR;
                                    objOwnerPartnership.OwnerPartnershipTypeEN = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicensesOwnerPartnership.OwnerPartnershipTypeEN;
                                    objOwnerPartnership.OwnerSharePercentage = objGetTradeLicenseBasicByEIDPPResponseOwnerDetailsBusinessLicensesOwnerPartnership.OwnerSharePercentage;
                                    lstOwnerPartnership.Add(objOwnerPartnership);
                                }

                            }
                        }

                        objResponseParams.lstOwnerDetails = lstOwnerDetails;
                        objResponseParams.lstBusinessLicenses = lstBusinessLicenses;
                        objResponseParams.lstOwnerPartnership = lstOwnerPartnership;

                        flag = 1;
                        json = JsonConvert.SerializeObject(new { objResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<MoEcDetails.MoEcResponseParams>(objResponseParams), ConfigurationManager.AppSettings["MOECCode"].ToString(), ConfigurationManager.AppSettings["MOEC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = objError.ErrorDescription;
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOECCode"].ToString(), ConfigurationManager.AppSettings["MOEC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOECCode"].ToString(), ConfigurationManager.AppSettings["MOEC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    ResponseDescription = faultException.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOECCode"].ToString(), ConfigurationManager.AppSettings["MOEC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOECCode"].ToString(), ConfigurationManager.AppSettings["MOEC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOECCode"].ToString(), ConfigurationManager.AppSettings["MOEC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}