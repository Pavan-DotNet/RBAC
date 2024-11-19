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
    public class SHJHRController : Controller
    {
        // GET: SHJHR
        public ActionResult Index()
        {
            return View("SHJHR");
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
                var input = new JavaScriptSerializer().Deserialize<DPPRISONSTATUSDetails.DPRequest>(postdata);

                SHJHR.SHJ_DHR_getEmployeeDetailsbyEIDPortTypeClient client = new SHJHR.SHJ_DHR_getEmployeeDetailsbyEIDPortTypeClient();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    SHJHR.GetEmployeeDetails objRequest = new SHJHR.GetEmployeeDetails();
                    objRequest.EmiratesId = input.EmiratesId.Trim();

                    SHJHR.GetEmployeeDetailsResponse objResponse = client.GetEmployeeDetails(objRequest);

                    List<SHJHRDetails.SHJHRResponse> lstResponse = new List<SHJHRDetails.SHJHRResponse>();
                    SHJHRDetails.SHJHRResponse objRes = null;

                    if (objResponse != null)
                    {
                        objRes = new SHJHRDetails.SHJHRResponse();
                        SHJHR.GetEmployeeDetailsResponseError objError = objResponse.Error;

                        if (objError.ErrorCode == "FCSA-200")
                        {
                            SHJHR.GetEmployeeDetailsResponseEmployeeDetails[] objEmployeeDetailsResponseEmployeeDetails = objResponse.EmployeeDetails;

                            if (objEmployeeDetailsResponseEmployeeDetails != null && objEmployeeDetailsResponseEmployeeDetails.Length > 0)
                            {
                                foreach (var obj in objEmployeeDetailsResponseEmployeeDetails)
                                {
                                    objRes.EmiratesId = obj.EmiratesId;
                                    objRes.EmployeeNumber = obj.EmployeeNumber;
                                    objRes.EmployeeNameEn = obj.EmployeeNameEn;
                                    objRes.EmployeeNameAr = obj.EmployeeNameAr;
                                    objRes.EmployerName = obj.EmployerName;
                                    objRes.JobTitle = obj.JobTitle;
                                    objRes.JoiningDate = obj.JoiningDate;
                                    objRes.EndingDate = obj.EndingDate;
                                    objRes.MonthlySalary = obj.MonthlySalary;
                                    objRes.LivingExpenses = obj.LivingExpenses;
                                    lstResponse.Add(objRes);
                                }
                            }

                            flag = 1;
                            json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<SHJHRDetails.SHJHRResponse>>(lstResponse), ConfigurationManager.AppSettings["SHJHRCode"].ToString(), ConfigurationManager.AppSettings["SHJHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = objError.ErrorDescription;
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SHJHRCode"].ToString(), ConfigurationManager.AppSettings["SHJHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
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
                                string ResponseDescription = child.InnerXml;
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SHJHRCode"].ToString(), ConfigurationManager.AppSettings["SHJHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    string ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SHJHRCode"].ToString(), ConfigurationManager.AppSettings["SHJHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                // var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SHJHRCode"].ToString(), ConfigurationManager.AppSettings["SHJHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SHJHRCode"].ToString(), ConfigurationManager.AppSettings["SHJHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }


    }


}