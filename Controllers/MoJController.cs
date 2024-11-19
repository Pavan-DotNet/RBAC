using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    public class MoJController : Controller
    {
        // GET: MoJService
        public ActionResult Index()
        {
            return View("MoJ");
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
        public ActionResult Search(string mid, string index, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                DataTable dtMarital = null;
                var input = new JavaScriptSerializer().Deserialize<MoJDeatils.MoJDeatilsRequest>(postdata);
                MoJService.Moj_GetDataSoapClient client = new MoJService.Moj_GetDataSoapClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    List<MoJDeatils.MoJDeatilsResponse> lstResponseParams = new List<MoJDeatils.MoJDeatilsResponse>();
                    MoJDeatils.MoJDeatilsResponse objResponseParams = null;
                    MoJService.Data_OutPut objOutput = null;
                    string ReturnValue = string.Empty;

                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    if (mid == "2" && index == "2")
                        objOutput = client.GetMarriage_Details_byEID(ConfigurationManager.AppSettings["MOJUserName"].ToString(), ConfigurationManager.AppSettings["MOJPassword"].ToString(), input.EmiratesID);
                    else if (mid == "2" && index == "3")
                        objOutput = client.GetMarriage_Details_byName(ConfigurationManager.AppSettings["MOJUserName"].ToString(), ConfigurationManager.AppSettings["MOJPassword"].ToString(), input.Name);
                    else if (mid == "2" && index == "4")
                        objOutput = client.GetMarriage_Details_byDOB(ConfigurationManager.AppSettings["MOJUserName"].ToString(), ConfigurationManager.AppSettings["MOJPassword"].ToString(), input.DOB);

                    if (mid == "3" && index == "2")
                        objOutput = client.GetDivorce_Details_byEID(ConfigurationManager.AppSettings["MOJUserName"].ToString(), ConfigurationManager.AppSettings["MOJPassword"].ToString(), input.EmiratesID);
                    else if (mid == "3" && index == "3")
                        objOutput = client.GetDivorce_Details_byName(ConfigurationManager.AppSettings["MOJUserName"].ToString(), ConfigurationManager.AppSettings["MOJPassword"].ToString(), input.Name);
                    else if (mid == "3" && index == "4")
                        objOutput = client.GetDivorce_Details_byDOB(ConfigurationManager.AppSettings["MOJUserName"].ToString(), ConfigurationManager.AppSettings["MOJPassword"].ToString(), input.DOB);


                    if (objOutput != null)
                        dtMarital = objOutput.Data_output;


                    if (dtMarital != null && dtMarital.Rows.Count > 0)
                        foreach (DataRow dr in dtMarital.Rows)
                        {
                            objResponseParams = new MoJDeatils.MoJDeatilsResponse();
                            objResponseParams.AppNo = IEUtils.ToString(dr["AppNo"]);
                            objResponseParams.ContractNo = IEUtils.ToString(dr["ContractNo"]);
                            objResponseParams.ContractDate = IEUtils.ToDate(dr["ContractDate"]);
                            objResponseParams.Husband_ID = IEUtils.ToString(dr["Husband_ID"]);
                            objResponseParams.Husband_Dob = IEUtils.ToDate(dr["Husband_Dob"]);
                            objResponseParams.Husband_NameAr = IEUtils.ToString(dr["Husband_NameAr"]);
                            objResponseParams.Wife_ID = IEUtils.ToString(dr["Wife_ID"]);
                            objResponseParams.Wife_Dob = IEUtils.ToDate(dr["Wife_Dob"]);
                            objResponseParams.Wife_NameAr = IEUtils.ToString(dr["Wife_NameAr"]);
                            if (mid == "3")
                                objResponseParams.Sons_Num = IEUtils.ToString(dr["Sons_Num"]);
                            objResponseParams.Husband_BoysNo = IEUtils.ToString(dr["Husband_BoysNo"]);
                            objResponseParams.Husband_GirlsNo = IEUtils.ToString(dr["Husband_GirlsNo"]);
                            objResponseParams.Wife_BoysNo = IEUtils.ToString(dr["Wife_BoysNo"]);
                            objResponseParams.Wife_GirlsNo = IEUtils.ToString(dr["Wife_GirlsNo"]);
                            lstResponseParams.Add(objResponseParams);
                        }

                    if (lstResponseParams.Count > 0)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<MoJDeatils.MoJDeatilsResponse>>(lstResponseParams), ConfigurationManager.AppSettings["MOJCode"].ToString(), ConfigurationManager.AppSettings["MOJ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOJCode"].ToString(), ConfigurationManager.AppSettings["MOJ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOJCode"].ToString(), ConfigurationManager.AppSettings["MOJ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {

                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOJCode"].ToString(), ConfigurationManager.AppSettings["MOJ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOJCode"].ToString(), ConfigurationManager.AppSettings["MOJ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOJCode"].ToString(), ConfigurationManager.AppSettings["MOJ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}