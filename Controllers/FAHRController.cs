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
    public class FAHRController : Controller
    {
        // GET: FAHR
        public ActionResult Index()
        {
            return View("FAHR");
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
        HttpWebRequest CreateWebRequest(string URL)
        {

            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://dpebus.dubaipolice.gov.ae/services/MOIDeadPersonInfoServ?wsdl");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"" + URL + "");
            webRequest.Headers.Add("SOAPAction", "urn:getDeadPersonInfo");
            webRequest.ContentType = "text/xml;charset=\"UTF-8\"";
            webRequest.Accept = "gzip,deflate";
            webRequest.Method = "POST";
            webRequest.UserAgent = "Apache-HttpClient/4.1.1 (java 1.5)";

            return webRequest;
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<FAHRDetails.FAHRRequestParams>(postdata);
                List<FAHRDetails.FAHRResponseParams> lstFahrResponse = new List<FAHRDetails.FAHRResponseParams>();
                FAHRDetails.FAHRResponseParams objFahrResponse = null;
                DataSet ds = new DataSet();
                try
                {
                    //List<FAHRModels.FahrData> lstFahr = new List<FAHRModels.FahrData>();
                    //FAHRModels.FahrData objFahr = null;
                    ////List<FAHRModels.FAHRResponseParams> lstFahrResponse = new List<FAHRModels.FAHRResponseParams>();
                    ////FAHRModels.FAHRResponseParams objFahrResponse = null;
                    //List<FAHRModels.FahrData> lstRequestParams = lstFahr;
                    //int flag = 0;
                    try
                    {
                        if (!string.IsNullOrEmpty(input.EmiratesId) && input.EmiratesId.Length == 15)
                        {
                            flag++;

                            HttpWebRequest request = CreateWebRequest(ConfigurationManager.AppSettings["FAHRURi"].ToString());
                            XmlDocument soapEnvelopeXml = new XmlDocument();
                            soapEnvelopeXml.LoadXml(@"
                           <soapenv:Envelope xmlns:get=""http://xmlns.oracle.com/apps/per/soaprovider/plsql/xxfahr_mocd_webservice_pkg/get_employee_details/"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xxf=""http://xmlns.oracle.com/apps/per/soaprovider/plsql/xxfahr_mocd_webservice_pkg/"">
                           <soapenv:Header>
                              <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
                                 <wsse:UsernameToken wsu:Id=""UsernameToken-CDEA965A3D22C4A2531547379154486228"">
                                    <wsse:Username>ESB_MOCD_INTEG_USER</wsse:Username>
                                    <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">je8f41eN1</wsse:Password>
                                    <wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">vV0/FkOamqGTJnAW69JCXw==</wsse:Nonce>
                                    <wsu:Created>2019-01-13T11:32:34.486Z</wsu:Created>
                                 </wsse:UsernameToken>
                              </wsse:Security>
                              <xxf:SOAHeader></xxf:SOAHeader>
                           </soapenv:Header>
                           <soapenv:Body>
                              <get:InputParameters>
                                 <!--Optional:-->
                                 <get:P_EMIRATES_ID>" + input.EmiratesId + @"</get:P_EMIRATES_ID>
                                 <!--Optional:-->
                                 <get:P_EMPLOYEE_NAME/>
                                 <!--Optional:-->
                                 <get:P_DATE_OF_BIRTH/>
                                 <!--Optional:-->
                                 <get:P_FAMILY_NUMBER/>
                                 <!--Optional:-->
                                 <get:P_EMIRATE_NUMBER/>
                              </get:InputParameters>
                           </soapenv:Body>
                        </soapenv:Envelope>

                        ");

                            using (Stream stream = request.GetRequestStream())
                            {
                                soapEnvelopeXml.Save(stream);
                            }
                            using (WebResponse response = request.GetResponse())
                            {
                                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                                {
                                    string soapResult = rd.ReadToEnd();
                                    StringReader StringStream = new StringReader(soapResult);
                                    ds.ReadXml(StringStream);
                                    if (ds != null && ds.Tables.Count > 0 && ds.Tables["Column"] != null)
                                    {
                                        int i = 0; string str = string.Empty;
                                        for (i = 0; i <= ds.Tables.Count - 1; i++)
                                        {
                                            if (ds.Tables[i].TableName == "Row")
                                            {
                                                foreach (DataRow dr in ds.Tables[i].Rows)
                                                {
                                                    if (ds.Tables["Column"] != null && ds.Tables["Column"].Rows.Count > 0)
                                                    {
                                                        DataRow[] drRow = ds.Tables["Column"].Select("Row_Id=" + dr["Row_Id"].ToString());
                                                        objFahrResponse = new FAHRDetails.FAHRResponseParams();
                                                        foreach (DataRow data in drRow)
                                                        {

                                                            if (data[0].ToString().Trim() == "EMIRATES_ID")
                                                                objFahrResponse.EMIRATES_ID = data[2] != null ? IEUtils.ToString(data[2]) : string.Empty;
                                                            else if (data[0].ToString().Trim() == "EMPLOYEE_NUMBER")
                                                                objFahrResponse.EMPLOYEE_NUMBER = data[2] != null ? IEUtils.ToString(data[2]) : string.Empty;
                                                            else if (data[0].ToString().Trim() == "EMPLOYEE_NAME")
                                                                objFahrResponse.EMPLOYEE_NAME = data[2] != null ? IEUtils.ToString(data[2]) : string.Empty;
                                                            else if (data[0].ToString().Trim() == "EMPLOYEE_NAME_EN")
                                                                objFahrResponse.EMPLOYEE_NAME_EN = data[2] != null ? IEUtils.ToString(data[2]) : string.Empty;
                                                            else if (data[0].ToString().Trim() == "ENTITY_CODE")
                                                                objFahrResponse.ENTITY_CODE = data[2] != null ? IEUtils.ToString(data[2]) : string.Empty;
                                                            else if (data[0].ToString().Trim() == "ENTITY_NAME_EN")
                                                                objFahrResponse.ENTITY_NAME_EN = data[2] != null ? IEUtils.ToString(data[2]) : string.Empty;
                                                            else if (data[0].ToString().Trim() == "ENTITY_NAME_AR")
                                                                objFahrResponse.ENTITY_NAME_AR = data[2] != null ? IEUtils.ToString(data[2]) : string.Empty;
                                                            else if (data[0].ToString().Trim() == "HIRE_DATE")
                                                                objFahrResponse.HIRE_DATE = data[2] != null ? IEUtils.ToString(data[2]) : null;
                                                            else if (data[0].ToString().Trim() == "MONTHLY_SALARY")
                                                                objFahrResponse.MONTHLY_SALARY = data[2] != null ? IEUtils.ToInt(data[2]) : 0;
                                                            else if (data[0].ToString().Trim() == "END_OF_SERVICE_DATE")
                                                                objFahrResponse.END_OF_SERVICE_DATE = data[2] != null ? IEUtils.ToString(data[2]) : null ;
                                                          

                                                            else if (data[0].ToString().Trim() == "END_OF_SERVICE_REASON")
                                                                objFahrResponse.END_OF_SERVICE_REASON = data[2] != null ? IEUtils.ToString(data[2]) : string.Empty;
                                                            else if (data[0].ToString().Trim() == "PERSON_TYPE_AR")
                                                                objFahrResponse.PERSON_TYPE_AR = data[2] != null ? IEUtils.ToString(data[2]) : null;

                                                            else if (data[0].ToString().Trim() == "PERSON_TYPE_EN")
                                                                objFahrResponse.PERSON_TYPE_EN = data[2] != null ? IEUtils.ToString(data[2]) : null;

                                                        }
                                                        lstFahrResponse.Add(objFahrResponse);
                                                    }
                                                }
                                            }


                                        }

                                        if (lstFahrResponse != null && lstFahrResponse.Count >= 0)
                                        {
                                            json = JsonConvert.SerializeObject(new { lstFahrResponse, flag=1 }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                            LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                        }
                                        else
                                        {
                                            flag = 2;
                                            string ResponseDescription = "No Matching Records Available.";
                                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                        }
                                    }

                                    else
                                    {
                                        flag = 2;
                                        string ResponseDescription = "No Matching Records Available.";
                                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                    }
                                }

                            }
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "Please Enter correct emirates id";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        //CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "FAHR", "RunFahrService", "" + ex.Message.ToString() + "", EmirateID.ToString(), "", "");
                        //return null;
                    }
                }
                catch (WebException ex)
                {
                    flag = 3;
                    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    string ResponseDescription = resp.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                catch (Exception ex)
                {
                    flag = 3;
                    string ResponseDescription = ex.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                #region "OLD Code"
                //FAHRService2.XXFAHR_MOCD_WEBSERVICE_PKG_PortTypeClient client = new FAHRService2.XXFAHR_MOCD_WEBSERVICE_PKG_PortTypeClient();

                //using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                //{
                //    //var httpRequestProperty = new HttpRequestMessageProperty();
                //    //httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                //    //OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                //    FAHRService2.InputParameters objInput = new FAHRService2.InputParameters();
                //    objInput.P_EMIRATES_ID = input.EmiratesId;
                //    objInput.P_DATE_OF_BIRTH = null;
                //    objInput.P_DATE_OF_BIRTHSpecified = false;
                //    objInput.P_EMPLOYEE_NAME = "";
                //    objInput.P_FAMILY_NUMBER = "";



                //    FAHRService2.SOAHeader objHeader = new FAHRService2.SOAHeader();
                //    objHeader.Responsibility = ConfigurationManager.AppSettings["Responsibility"].ToString();
                //    objHeader.RespApplication = ConfigurationManager.AppSettings["RespApplication"].ToString();
                //    objHeader.SecurityGroup = ConfigurationManager.AppSettings["SecurityGroup"].ToString();
                //    objHeader.NLSLanguage = ConfigurationManager.AppSettings["NLSLanguage"].ToString();
                //    FAHRService2.GET_EMPLOYEE_DETAILSRequest objRequest = new FAHRService2.GET_EMPLOYEE_DETAILSRequest();
                //    objRequest.InputParameters = objInput;

                //    FAHRService2.ServiceDetail objt = new FAHRService2.ServiceDetail();
                //    FAHRService2.OutputParameters objOutput = client.GET_EMPLOYEE_DETAILS(objHeader, objInput);

                //    if (objOutput.O_RETURN_CODE == 0)
                //    {

                //        FAHRService2.RowSetRowColumn[][] objRC = objOutput.O_MESSAGE;

                //        flag = 1;
                //        for (var i = 0; i < objRC.Length; i++)
                //        {
                //            objFahrResponse = new FAHRDetails.FAHRResponseParams();
                //            objFahrResponse.EMIRATES_ID = objRC[i][0].Value != null ? IEUtils.ToString(objRC[i][0].Value) : string.Empty;
                //            if (objRC[i][1] != null)
                //                objFahrResponse.EMPLOYEE_NUMBER = objRC[i][1].Value != null ? IEUtils.ToString(objRC[i][1].Value) : string.Empty; ;
                //            if (objRC[i][2] != null)
                //                objFahrResponse.EMPLOYEE_NAME = objRC[i][2].Value != null ? IEUtils.ToString(objRC[i][2].Value) : string.Empty; ;
                //            if (objRC[i][3] != null)
                //            objFahrResponse.EMPLOYEE_NAME_EN = objRC[i][3].Value != null ? IEUtils.ToString(objRC[i][3].Value) : string.Empty; ;
                //            if (objRC[i][4] != null)
                //                objFahrResponse.ENTITY_CODE = objRC[i][4].Value != null ? IEUtils.ToString(objRC[i][4].Value) : string.Empty; ;
                //            if (objRC[i][5] != null)
                //                objFahrResponse.ENTITY_NAME_EN = objRC[i][5].Value != null ? IEUtils.ToString(objRC[i][5].Value) : string.Empty;
                //            if (objRC[i][6] != null)
                //                objFahrResponse.ENTITY_NAME_AR = objRC[i][6].Value != null ? IEUtils.ToString(objRC[i][6].Value) : string.Empty;
                //            if (objRC[i][7] != null)
                //                objFahrResponse.HIRE_DATE = objRC[i][7].Value != null ? IEUtils.ToDate(objRC[i][7].Value) : (DateTime?)null;

                //            if (objRC[i][8] != null)
                //                objFahrResponse.MONTHLY_SALARY = objRC[i][8].Value != null ? IEUtils.ToInt(objRC[i][8].Value) : 0;
                //            if (objRC[i][9] != null)
                //                objFahrResponse.END_OF_SERVICE_DATE = objRC[i][9].Value != null ? IEUtils.ToDate(objRC[i][9].Value) : (DateTime?)null;
                //            if (objRC[i][10] != null)
                //                objFahrResponse.END_OF_SERVICE_REASON = objRC[i][10].Value != null ? IEUtils.ToString(objRC[i][10].Value) : string.Empty; ;
                //            lstFahrResponse.Add(objFahrResponse);
                //        }
                //        if (objRC.Length > 0)
                //        {
                //            json = JsonConvert.SerializeObject(new { lstFahrResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                //            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<FAHRDetails.FAHRResponseParams>>(lstFahrResponse), ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                //        }
                //        else
                //        {
                //            flag = 2;
                //            string ResponseDescription = "No Matching Records Available.";
                //            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                //            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                //        }
                //    }
                //    else
                //    {
                //        flag = 2;
                //        string ResponseDescription = "No Matching Records Available";
                //        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                //        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                //    }

                //}
                #endregion

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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["FAHRCode"].ToString(), ConfigurationManager.AppSettings["FAHR"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}