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
    public class AJMMUNController : Controller
    {
        // GET: AJMMUN
        public ActionResult Index()
        {
            return View("AJMMUN");
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
                var input = new JavaScriptSerializer().Deserialize<AJMANHR.AJMANHRRequest>(postdata);

                AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortTypeClient client = new AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortTypeClient();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    AJMMUNService.GetResidentialGrantStatus objRequest = new AJMMUNService.GetResidentialGrantStatus();
                    objRequest.EmiratesId = input.EmiratesID.Trim();

                    AJMMUNService.GetResidentialGrantStatusResponse objAJMMUN = client.GetResidentialGrantStatus(objRequest);

                    AJMMUNService.GetResidentialGrantStatusResponseError objError = objAJMMUN.Error;
                    AJMMUNDetails.AJMMUNResponse objResponse = null;
                    List<AJMMUNDetails.AJMMUNResponse> lstResponse = new List<AJMMUNDetails.AJMMUNResponse>();


                    if (objError != null && objError.ErrorCode == "FCSA-200")
                    {
                        if (objAJMMUN != null)
                        {
                            AJMMUNService.GetResidentialGrantStatusResponseResidentialGrantStatus[] objGetResidentialGrantStatusResponseResidentialGrantStatus = objAJMMUN.ResidentialGrantStatus;
                            foreach (var obj in objGetResidentialGrantStatusResponseResidentialGrantStatus)
                            {
                                objResponse = new AJMMUNDetails.AJMMUNResponse();
                                objResponse.TypeOfLand = obj.TypeOfLand;
                                objResponse.ParcelID = obj.ParcelID;
                                objResponse.ResidentialGranted = obj.ResidentialGranted;
                                objResponse.SitePlanIssuedDate = obj.SitePlanIssuedDate;
                                lstResponse.Add(objResponse);
                            }
                        }
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<AJMMUNDetails.AJMMUNResponse>>(lstResponse), ConfigurationManager.AppSettings["AJMMUNCode"].ToString(), ConfigurationManager.AppSettings["AJMMUN"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No matching records are available."; //objError.ErrorCode + " - " + objError.ErrorMessage;
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMMUNCode"].ToString(), ConfigurationManager.AppSettings["AJMMUN"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMMUNCode"].ToString(), ConfigurationManager.AppSettings["AJMMUN"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                            }
                        }
                    }
                }
                else
                {
                    flag = 3;
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMMUNCode"].ToString(), ConfigurationManager.AppSettings["AJMMUN"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMMUNCode"].ToString(), ConfigurationManager.AppSettings["AJMMUN"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJMMUNCode"].ToString(), ConfigurationManager.AppSettings["AJMMUN"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}