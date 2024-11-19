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
    public class ADJDController : Controller
    {
        // GET: ADJD
        public ActionResult Index()
        {
            return View("ADJD");
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
                var input = new JavaScriptSerializer().Deserialize<ADJDDetails.ADJDDetailsRequestParams>(postdata);
                ADJDService.ABU_JD_GetMarraigeContractDetailsPortTypeClient client = new ADJDService.ABU_JD_GetMarraigeContractDetailsPortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    ADJDDetails.ADJDDetailsResponseParams objResponseParams = new ADJDDetails.ADJDDetailsResponseParams();

                    ADJDDetails.MarriageContractDetails objMarriageContractDetails = null;
                    ADJDDetails.PartyDetails objPartyDetails = null;
                    List<ADJDDetails.MarriageContractDetails> lstMarriageContractDetails = new List<ADJDDetails.MarriageContractDetails>();
                    List<ADJDDetails.PartyDetails> lstPartyDetails = new List<ADJDDetails.PartyDetails>();

                    ADJDService.GetMarriageContractDetails objInput = new ADJDService.GetMarriageContractDetails();
                    objInput.GetMarriageContractDetails1 = new ADJDService.GetMarriageContractDetailsGetMarriageContractDetails();
                    if (index == "2")
                        objInput.GetMarriageContractDetails1.EmiratesID = input.EmiratesID;
                    else if (index == "3")
                        objInput.GetMarriageContractDetails1.PassportNumber = input.Passport;

                    ADJDService.GetMarriageContractDetailsResponse objGetMarriageContractDetailsResponse = client.GetMarriageContractDetails(objInput);
                    ADJDService.GetMarriageContractDetailsResponseError[] lstError = objGetMarriageContractDetailsResponse.Error;

                    foreach (ADJDService.GetMarriageContractDetailsResponseError objError in lstError)
                    {
                        if (objError.ErrorCode == "FCSA-200")
                        {
                            foreach (ADJDService.GetMarriageContractDetailsResponseMarriageContractDetails objGetMarriageContractDetailsResponseMarriageContractDetails in objGetMarriageContractDetailsResponse.MarriageContractDetails)
                            {
                                objMarriageContractDetails = new ADJDDetails.MarriageContractDetails();

                                objMarriageContractDetails.CityCode = objGetMarriageContractDetailsResponseMarriageContractDetails.CityCode;
                                objMarriageContractDetails.CityDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetails.CityDescriptionArabic;
                                objMarriageContractDetails.CityDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetails.CityDescriptionEnglish;
                                objMarriageContractDetails.ContractDate = objGetMarriageContractDetailsResponseMarriageContractDetails.ContractDate;
                                objMarriageContractDetails.ContractNumber = objGetMarriageContractDetailsResponseMarriageContractDetails.ContractNumber;
                                objMarriageContractDetails.CourtNameCode = objGetMarriageContractDetailsResponseMarriageContractDetails.CourtNameCode;
                                objMarriageContractDetails.CourtNameDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetails.CourtNameDescriptionArabic;
                                objMarriageContractDetails.CourtNameDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetails.CourtNameDescriptionEnglish;
                                objMarriageContractDetails.FolderNumber = objGetMarriageContractDetailsResponseMarriageContractDetails.FolderNumber;
                                objMarriageContractDetails.FolderYear = objGetMarriageContractDetailsResponseMarriageContractDetails.FolderYear;
                                objMarriageContractDetails.MarriagePlaceCode = objGetMarriageContractDetailsResponseMarriageContractDetails.MarriagePlaceCode;
                                objMarriageContractDetails.MarriagePlaceDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetails.MarriagePlaceDescriptionArabic;
                                objMarriageContractDetails.MarriagePlaceDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetails.MarriagePlaceDescriptionEnglish;
                                objMarriageContractDetails.ReferenceNumber = objGetMarriageContractDetailsResponseMarriageContractDetails.ReferenceNumber;
                                objMarriageContractDetails.RegionCode = objGetMarriageContractDetailsResponseMarriageContractDetails.RegionCode;
                                objMarriageContractDetails.RegionDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetails.RegionDescriptionArabic;
                                objMarriageContractDetails.RegionDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetails.RegionDescriptionEnglish;

                                if (objGetMarriageContractDetailsResponseMarriageContractDetails.PartyDetails != null)
                                    foreach (ADJDService.GetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails in objGetMarriageContractDetailsResponseMarriageContractDetails.PartyDetails)
                                    {
                                        objPartyDetails = new ADJDDetails.PartyDetails();
                                        objPartyDetails.CityCode = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.CityCode;
                                        objPartyDetails.CityDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.CityDescriptionArabic;
                                        objPartyDetails.CityDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.CityDescriptionEnglish;
                                        objPartyDetails.CountryCode = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.CountryCode;
                                        objPartyDetails.CountryDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.CountryDescriptionArabic;
                                        objPartyDetails.CountryDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.CountryDescriptionEnglish;
                                        objPartyDetails.DateOfBirth = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.DateOfBirth;
                                        objPartyDetails.EmailAddress = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.EmailAddress;
                                        objPartyDetails.EmiratesID = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.EmiratesID;
                                        objPartyDetails.FamilyName = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.FamilyName;
                                        objPartyDetails.FirstName = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.FirstName;
                                        objPartyDetails.FourthName = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.FourthName;
                                        objPartyDetails.GenderCode = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.GenderCode;
                                        objPartyDetails.GenderDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.GenderDescriptionArabic;
                                        objPartyDetails.GenderDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.GenderDescriptionEnglish;
                                        objPartyDetails.IDTypeCode = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.IDTypeCode;
                                        objPartyDetails.IDTypeDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.IDTypeDescriptionArabic;
                                        objPartyDetails.IDTypeDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.IDTypeDescriptionEnglish;
                                        objPartyDetails.NameInEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.NameInEnglish;
                                        objPartyDetails.NationalityCode = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.NationalityCode;
                                        objPartyDetails.NationalityDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.NationalityDescriptionArabic;
                                        objPartyDetails.NationalityDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.NationalityDescriptionEnglish;
                                        objPartyDetails.PartyRoleCode = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.PartyRoleCode;
                                        objPartyDetails.PartyRoleDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.PartyRoleDescriptionArabic;
                                        objPartyDetails.PartyRoleDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.PartyRoleDescriptionEnglish;
                                        objPartyDetails.PlaceOfBirth = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.PlaceOfBirth;
                                        objPartyDetails.PlaceOfResidence = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.PlaceOfResidence;
                                        objPartyDetails.ProfessionDescription = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.ProfessionDescription;
                                        objPartyDetails.QualificationCode = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.QualificationCode;
                                        objPartyDetails.QualificationDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.QualificationDescriptionArabic;
                                        objPartyDetails.QualificationDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.QualificationDescriptionEnglish;
                                        objPartyDetails.RegionCode = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.RegionCode;
                                        objPartyDetails.RegionDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.RegionDescriptionArabic;
                                        objPartyDetails.RegionDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.RegionDescriptionEnglish;
                                        objPartyDetails.ReligionCode = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.ReligionCode;
                                        objPartyDetails.ReligionDescriptionArabic = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.ReligionDescriptionArabic;
                                        objPartyDetails.ReligionDescriptionEnglish = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.ReligionDescriptionEnglish;
                                        objPartyDetails.SecondName = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.SecondName;
                                        objPartyDetails.Telephone = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.Telephone;
                                        objPartyDetails.ThirdName = objGetMarriageContractDetailsResponseMarriageContractDetailsPartyDetails.ThirdName;
                                        lstPartyDetails.Add(objPartyDetails);

                                    }


                                lstMarriageContractDetails.Add(objMarriageContractDetails);

                            }

                            objResponseParams.lstMarriageContractDetails = lstMarriageContractDetails;
                            objResponseParams.lstPartyDetails = lstPartyDetails;

                            flag = 1;
                            json = JsonConvert.SerializeObject(new { objResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<ADJDDetails.ADJDDetailsResponseParams>(objResponseParams), ConfigurationManager.AppSettings["ADJDCode"].ToString(), ConfigurationManager.AppSettings["ADJD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = objError.ErrorDescription;
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADJDCode"].ToString(), ConfigurationManager.AppSettings["ADJD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADJDCode"].ToString(), ConfigurationManager.AppSettings["ADJD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    ResponseDescription = faultException.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADJDCode"].ToString(), ConfigurationManager.AppSettings["ADJD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

               // var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADJDCode"].ToString(), ConfigurationManager.AppSettings["ADJD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString(); ;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADJDCode"].ToString(), ConfigurationManager.AppSettings["ADJD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}