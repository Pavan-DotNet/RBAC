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
    public class AJMFZController : Controller
    {
        // GET: AJMFZ
        public ActionResult Index()
        {
            return View("AJMFZ");
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
                var input = new JavaScriptSerializer().Deserialize<AJMFZDetails.AJMFZRequest>(postdata);

                AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortTypeClient client = new AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortTypeClient();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    List<AJMFZDetails.AJMFZResponse> lstResponse = new List<AJMFZDetails.AJMFZResponse>();
                    AJMFZDetails.AJMFZResponse objLicenses = null; ;

                    AJMFZ.GetLicenseDetails_Input objInput = new AJMFZ.GetLicenseDetails_Input();
                    AJMFZ.GetLicenseDetails_Output objOutput = new AJMFZ.GetLicenseDetails_Output();
                    AJMFZ.GetLicenseDetails obj = new AJMFZ.GetLicenseDetails();
                    obj.PassportNumber = input.PassportNumber.Trim();  // "Z3672727" 

                    objInput.GetLicenseDetails = obj;

                    AJMFZ.GetLicenseDetailsResponse objResponse = client.GetLicenseDetails(objInput.GetLicenseDetails);
                    AJMFZ.GetLicenseDetailsResponseLicenseDetails[] objResult = objResponse.LicenseDetails;

                    AJMFZ.GetLicenseDetailsResponseError objError = objResponse.Error;

                    if (objError.ErrorCode == "FCSA-200")
                    {
                        if (objResult != null && objResult.Length > 0)
                        {

                            foreach (AJMFZ.GetLicenseDetailsResponseLicenseDetails objRes in objResult)
                            {
                                objLicenses = new AJMFZDetails.AJMFZResponse();
                                objLicenses.CompanyNameAR = objRes.CompanyNameAR;
                                objLicenses.CompanyNameEN = objRes.CompanyNameEN;
                                objLicenses.CompanyStatus = objRes.CompanyStatus;
                                objLicenses.LicenseExpiryDate = objRes.LicenseExpiryDate;
                                objLicenses.LicenseNumber = objRes.LicenseNumber;
                                objLicenses.LicenseStartDate = objRes.LicenseStartDate;
                                objLicenses.CompanyStatus = objRes.CompanyStatus;
                                objLicenses.ResidenceVisaQuotaUsed = objRes.ResidenceVisaQuotaUsed;
                                objLicenses.ShareholderFirstName = objRes.ShareholderFirstName;
                                objLicenses.ShareholderLastName = objRes.ShareholderLastName;
                                objLicenses.ShareHolderNationality = objRes.ShareHolderNationality;
                                objLicenses.ShareHolderNoOfShares = objRes.ShareHolderNoOfShares;
                                objLicenses.ShareHolderPercentage = objRes.ShareHolderPercentage;

                                lstResponse.Add(objLicenses);
                            }


                            flag = 1;
                            json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<AJMFZDetails.AJMFZResponse>>(lstResponse), ConfigurationManager.AppSettings["AJMFZCode"].ToString(), ConfigurationManager.AppSettings["AJMFZ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = objError.ErrorCode + " - " + objError.ErrorDescription;
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMFZCode"].ToString(), ConfigurationManager.AppSettings["AJMFZ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = objError.ErrorCode + " - " + objError.ErrorDescription;
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMFZCode"].ToString(), ConfigurationManager.AppSettings["AJMFZ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMFZCode"].ToString(), ConfigurationManager.AppSettings["AJMFZ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMFZCode"].ToString(), ConfigurationManager.AppSettings["AJMFZ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMFZCode"].ToString(), ConfigurationManager.AppSettings["AJMFZ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMFZCode"].ToString(), ConfigurationManager.AppSettings["AJMFZ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}