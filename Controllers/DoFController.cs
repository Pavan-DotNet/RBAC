using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MOCDIntegrations
{
    public class DoFController : Controller
    {
        // GET: DoF
        public ActionResult Index()
        {
            return View("DoF");
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
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<DoFDetails.DoFRequest>(postdata);
                List<DoFDetails.DoFResponse> lstDOFResponse = new List<DoFDetails.DoFResponse>();

                DOFPENSION.PensionInquiryDetailsPortTypeClient client = new DOFPENSION.PensionInquiryDetailsPortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    DOFPENSION.pensionDetailsPerEmiratesIDRequest pensionDetailsPerEmiratesIDRequest = new DOFPENSION.pensionDetailsPerEmiratesIDRequest(input.EmiratesID);
                    DOFPENSION.Entry[] objEntry = client.pensionDetailsPerEmiratesID(pensionDetailsPerEmiratesIDRequest.p_emirates_id);

                    DOFPENSION.pensionDetailsPerEmiratesIDResponse pensionDetailsPerEmiratesIDResponse = new DOFPENSION.pensionDetailsPerEmiratesIDResponse(objEntry);


                    DoFDetails.DoFResponse objDoF = null;
                    if (objEntry != null && objEntry.Length > 0)
                    {
                        flag = 1;
                        objDoF = new DoFDetails.DoFResponse();
                        foreach (DOFPENSION.Entry obj in objEntry)
                        {
                            objDoF.EMIRATES_ID = obj.EMIRATES_ID;
                            objDoF.PENSIONER_DEATH = obj.PENSIONER_DEATH;
                            objDoF.PENSIONER_NAME_AR = obj.PENSIONER_NAME_AR;
                            objDoF.PENSIONER_NAME_US = obj.PENSIONER_NAME_US;
                            objDoF.PENSION_AMOUNT = obj.PENSION_AMOUNT;
                            objDoF.PENSION_ID = obj.PENSION_ID;
                            objDoF.PENSION_START_DATE = obj.PENSION_START_DATE;

                            lstDOFResponse.Add(objDoF);
                        }

                        json = JsonConvert.SerializeObject(new { lstDOFResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<DoFDetails.DoFResponse>>(lstDOFResponse), ConfigurationManager.AppSettings["DOFCode"].ToString(), ConfigurationManager.AppSettings["DOF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matchng Record Available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DOFCode"].ToString(), ConfigurationManager.AppSettings["DOF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                string ResponseDescription = string.Empty;

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
                                ResponseDescription = child.InnerXml;
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DOFCode"].ToString(), ConfigurationManager.AppSettings["DOF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    json = JsonConvert.SerializeObject(new { fex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, fex.Message, ConfigurationManager.AppSettings["DOFCode"].ToString(), ConfigurationManager.AppSettings["DOF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DOFCode"].ToString(), ConfigurationManager.AppSettings["DOF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.StackTrace;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DOFCode"].ToString(), ConfigurationManager.AppSettings["DOF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}