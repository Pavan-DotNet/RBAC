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
    public class AJMDEDController : Controller
    {
        // GET: AJMDED
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
        public ActionResult Search(string postdata,string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<AJMDEDDetails.AJMDEDDetailsRequest>(postdata);

                AJMDED.AJM_DED_getLicenseDetailsbyEIDPortTypeClient client = new AJMDED.AJM_DED_getLicenseDetailsbyEIDPortTypeClient();
                AJMDEDDetails.AJMDEDDetailsResponse objLicenses = null; ;
                List<AJMDEDDetails.AJMDEDDetailsResponse> lstResponse = new List<AJMDEDDetails.AJMDEDDetailsResponse>();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;


                    AJMDED.GetLicenseDetails_Input objInput = new AJMDED.GetLicenseDetails_Input();
                    AJMDED.GetLicenseDetails_Output objOutput = new AJMDED.GetLicenseDetails_Output();
                    AJMDED.GetLicenseDetails obj = new AJMDED.GetLicenseDetails();
                    obj.EmiratesId = input.EmiratesId.Trim();  // "Z3672727" 

                    objInput.GetLicenseDetails = obj;

                    AJMDED.GetLicenseDetailsResponse objResponse = client.GetLicenseDetails(objInput.GetLicenseDetails);
                    AJMDED.GetLicenseDetailsResponseGetLicenseDetails[] objResult = objResponse.GetLicenseDetails;

                    AJMDED.GetLicenseDetailsResponseError objError = objResponse.Error;

                    if (objError.ErrorCode == "FCSA-200")
                    {
                        if (objResult != null && objResult.Length > 0)
                        {
                            List<AJMDEDDetails.Licenses> lstLic = new List<AJMDEDDetails.Licenses>();
                            AJMDEDDetails.Licenses objLic = null;

                            foreach (AJMDED.GetLicenseDetailsResponseGetLicenseDetails objRes in objResult)
                            {
                                objLicenses = new AJMDEDDetails.AJMDEDDetailsResponse();
                                objLicenses.EmiratesID = objRes.EmiratesID;
                                objLicenses.PassportNumber = objRes.PassportNumber;
                                objLicenses.NameEN = objRes.NameEN;
                                objLicenses.NameAR = objRes.NameAR;
                                objLicenses.Gender = objRes.Gender;
                                objLicenses.DateOfBirth = objRes.DateOfBirth;
                                objLicenses.Nationality = objRes.Nationality;
                                objLicenses.PhoneNumber = objRes.PhoneNumber;
                                objLicenses.MobileNumber = objRes.MobileNumber;
                                objLicenses.BanningReason = objRes.BanningReason;
                                objLicenses.BanningStatus = objRes.BanningStatus;
                                objLicenses.StakeHolderId = objRes.StakeHolderId;
                                foreach (AJMDED.GetLicenseDetailsResponseGetLicenseDetailsLicenses objLicense in objRes.Licenses)
                                {
                                    objLic = new AJMDEDDetails.Licenses();
                                    objLic.LicenseId = objLicense.LicenseId;
                                    objLic.LicenseNumber = objLicense.LicenseNumber;
                                    lstLic.Add(objLic);
                                }
                                objLicenses.Licenses = lstLic;
                            }

                            AJMDEDDetails.LicenseOwnershipDetails objLicenseOwnershipDetails = null;
                            List<AJMDEDDetails.LicenseOwnershipDetails> lstLicenseOwnershipDetails = new List<AJMDEDDetails.LicenseOwnershipDetails>();

                            AJMDEDDetails.LicenseOwnerDetails objLicenseOwnerDetails = null;
                            List<AJMDEDDetails.LicenseOwnerDetails> lstLicenseOwnerDetails = new List<AJMDEDDetails.LicenseOwnerDetails>();

                            AJMDEDDetails.LicenseActivityDetails objLicenseActivityDetails = null;
                            List<AJMDEDDetails.LicenseActivityDetails> lstLicenseActivityDetails = new List<AJMDEDDetails.LicenseActivityDetails>();


                            foreach (AJMDEDDetails.Licenses objLicense in lstLic)
                            {
                                if (objLicense.LicenseNumber != string.Empty)
                                {
                                    AJMDEDLICENSE.AJM_DED_getLicenseOwnershipDetailsPortTypeClient clientL = new AJMDEDLICENSE.AJM_DED_getLicenseOwnershipDetailsPortTypeClient();
                                    using (OperationContextScope scopeL = new OperationContextScope(clientL.InnerChannel))
                                    {
                                        var httpRequestPropertyL = new HttpRequestMessageProperty();
                                        httpRequestPropertyL.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestPropertyL;

                                        AJMDEDLICENSE.GetLicenseOwnershipDetails objGetLicenseOwnershipDetails = new AJMDEDLICENSE.GetLicenseOwnershipDetails();
                                        objGetLicenseOwnershipDetails.LicenseNumber = objLicense.LicenseNumber;

                                        AJMDEDLICENSE.GetLicenseOwnershipDetailsResponse objGetLicenseOwnershipDetailsResponse = clientL.GetLicenseOwnershipDetails(objGetLicenseOwnershipDetails);
                                        AJMDEDLICENSE.GetLicenseOwnershipDetailsResponseLicenseOwnershipDetails objResultL = objGetLicenseOwnershipDetailsResponse.LicenseOwnershipDetails;

                                        if (objResultL != null)
                                        {
                                            objLicenseOwnershipDetails = new AJMDEDDetails.LicenseOwnershipDetails();
                                            objLicenseOwnershipDetails.LicenseNumber = objResultL.LicenseNumber;
                                            objLicenseOwnershipDetails.ApplicationNumber = objResultL.ApplicationNumber;
                                            objLicenseOwnershipDetails.LicenseNameAR = objResultL.LicenseNameAR;
                                            objLicenseOwnershipDetails.LicenseNameEN = objResultL.LicenseNameEN;
                                            objLicenseOwnershipDetails.LicenseTypeAR = objResultL.LicenseTypeAR;
                                            objLicenseOwnershipDetails.LicenseTypeEN = objResultL.LicenseTypeEN;
                                            objLicenseOwnershipDetails.LegalFormAR = objResultL.LegalFormAR;
                                            objLicenseOwnershipDetails.LegalFormEN = objResultL.LegalFormEN;
                                            objLicenseOwnershipDetails.LicenseStatusAR = objResultL.LicenseStatusAR;
                                            objLicenseOwnershipDetails.LicenseStatusEN = objResultL.LicenseStatusEN;
                                            objLicenseOwnershipDetails.LicenseStartDate = objResultL.LicenseStartDate;
                                            objLicenseOwnershipDetails.LicenseEndDate = objResultL.LicenseEndDate;
                                            objLicenseOwnershipDetails.POBoxNo = objResultL.POBoxNo;
                                            objLicenseOwnershipDetails.TelephoneNumber = objResultL.TelephoneNumber;
                                            objLicenseOwnershipDetails.MobileNumber = objResultL.MobileNumber;
                                            objLicenseOwnershipDetails.EmailAddress = objResultL.EmailAddress;
                                            objLicenseOwnershipDetails.BanningStatus = objResultL.BanningStatus;
                                            objLicenseOwnershipDetails.BanningReason = objResultL.BanningReason;
                                            objLicenseOwnershipDetails.LicenseClassification = objResultL.LicenseClassification;
                                            objLicenseOwnershipDetails.isTaziz = objResultL.isTaziz;
                                            objLicenseOwnershipDetails.LicenseID = objResultL.LicenseID;
                                            lstLicenseOwnershipDetails.Add(objLicenseOwnershipDetails);

                                            objLicenseOwnerDetails = new AJMDEDDetails.LicenseOwnerDetails();
                                            foreach (AJMDEDLICENSE.GetLicenseOwnershipDetailsResponseLicenseOwnershipDetailsLicenseOwnerDetails objOwner in objResultL.LicenseOwnerDetails)
                                            {
                                                objLicenseOwnerDetails.OwnerEmiratesID = objOwner.OwnerEmiratesID;
                                                objLicenseOwnerDetails.OwnerPassportNumber = objOwner.OwnerPassportNumber;
                                                objLicenseOwnerDetails.OwnerNameAR = objOwner.OwnerNameAR;
                                                objLicenseOwnerDetails.OwnerNameEN = objOwner.OwnerNameEN;
                                                objLicenseOwnerDetails.OwnerGenderAR = objOwner.OwnerGenderAR;
                                                objLicenseOwnerDetails.OwnerGenderEN = objOwner.OwnerGenderEN;
                                                objLicenseOwnerDetails.OwnerNationalityAR = objOwner.OwnerNationalityAR;
                                                objLicenseOwnerDetails.OwnerNationalityEN = objOwner.OwnerNationalityEN;
                                                objLicenseOwnerDetails.OwnerPhoneNumber = objOwner.OwnerPhoneNumber;
                                                objLicenseOwnerDetails.OwnerMobileNumber = objOwner.OwnerMobileNumber;
                                                objLicenseOwnerDetails.OwnerTypeAR = objOwner.OwnerTypeAR;
                                                objLicenseOwnerDetails.OwnerTypeEN = objOwner.OwnerTypeEN;
                                                objLicenseOwnerDetails.LeasorTypeAR = objOwner.LeasorTypeAR;
                                                objLicenseOwnerDetails.LeasorTypeEN = objOwner.LeasorTypeEN;
                                                objLicenseOwnerDetails.Notes = objOwner.Notes;
                                                objLicenseOwnerDetails.LicenseSource = objOwner.LicenseSource;
                                                objLicenseOwnerDetails.SharePercent = objOwner.SharePercent;
                                                objLicenseOwnerDetails.OwnerSerialID = objOwner.OwnerSerialID;
                                                lstLicenseOwnerDetails.Add(objLicenseOwnerDetails);
                                            }

                                            objLicenseActivityDetails = new AJMDEDDetails.LicenseActivityDetails();
                                            foreach (AJMDEDLICENSE.GetLicenseOwnershipDetailsResponseLicenseOwnershipDetailsLicenseActivityDetails objActivity in objResultL.LicenseActivityDetails)
                                            {
                                                objLicenseActivityDetails.SerialNo = objActivity.SerialNo;
                                                objLicenseActivityDetails.LicenseActivityCode = objActivity.LicenseActivityCode;
                                                objLicenseActivityDetails.LicenseActivityDescAR = objActivity.LicenseActivityDescAR;
                                                objLicenseActivityDetails.LicenseActivityDescEN = objActivity.LicenseActivityDescEN;
                                                lstLicenseActivityDetails.Add(objLicenseActivityDetails);
                                            }

                                            objLicenses.lstLicenseOwnershipDetails = lstLicenseOwnershipDetails;
                                            objLicenses.lstLicenseOwnerDetails = lstLicenseOwnerDetails;
                                            objLicenses.lstLicenseActivityDetails = lstLicenseActivityDetails;
                                        }


                                    }
                                }
                            }

                            lstResponse.Add(objLicenses);
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<AJMDEDDetails.AJMDEDDetailsResponse>>(lstResponse), ConfigurationManager.AppSettings["AJMDEDCode"].ToString(), ConfigurationManager.AppSettings["AJMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = objError.ErrorMessage;
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMDEDCode"].ToString(), ConfigurationManager.AppSettings["AJMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMDEDCode"].ToString(), ConfigurationManager.AppSettings["AJMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }); ;
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMDEDCode"].ToString(), ConfigurationManager.AppSettings["AJMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMDEDCode"].ToString(), ConfigurationManager.AppSettings["AJMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMDEDCode"].ToString(), ConfigurationManager.AppSettings["AJMDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}