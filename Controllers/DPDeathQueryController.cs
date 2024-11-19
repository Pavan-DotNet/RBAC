using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace MOCDIntegrations.Controllers
{
    public class DPDeathQueryController : Controller
    {
        // GET: DPDeathQuery
        public ActionResult Index()
        {
            return View("DPDeathQuery");
        }

        public HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["DP_URL"].ToString());
            webRequest.Headers.Add("SOAPAction", "urn:getDeadPersonInfo");
            webRequest.ContentType = "text/xml;charset=\"UTF-8\"";
            webRequest.Accept = "gzip,deflate";
            webRequest.Method = "POST";
            webRequest.UserAgent = "Apache-HttpClient/4.1.1 (java 1.5)";

            return webRequest;
        }
        public ActionResult Search(string index, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<DPDeathQueryDeatils.DPDeathQueryDeatilsRequest>(postdata);
                string soapResult = string.Empty;
                DPDeathQueryDeatils.DPDeathQueryDeatilsResponse objDPResponse = null;
                List<DPDeathQueryDeatils.DPDeathQueryDeatilsResponse> lstDPResponse = new List<DPDeathQueryDeatils.DPDeathQueryDeatilsResponse>();
                HttpWebRequest request = CreateWebRequest();
                request.UseDefaultCredentials = true;
                request.PreAuthenticate = true;
                //request.Credentials = new NetworkCredential("mocd_sys_user", "mocd@SERV#DP2019");
                request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["DPUserName"].ToString(), ConfigurationManager.AppSettings["DPPwd"].ToString());

                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""UTF-8""?>
                <soapenv:Envelope xmlns:moid=""http://ae.dubaipolice/dataservice/moideadpersoninfo"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                   <soapenv:Header>
                      <wsse:Security soapenv:mustUnderstand = ""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu =""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
                         <wsse:UsernameToken wsu:Id = ""UsernameToken-A02AA0602BBC99D96215371653390901"" >
                            <wsse:Username>" + ConfigurationManager.AppSettings["DPUserName"].ToString() + @"</wsse:Username>
                            <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">" + ConfigurationManager.AppSettings["DPPwd"].ToString() + @"</wsse:Password >
                        </wsse:UsernameToken>
                      </wsse:Security>
                   </soapenv:Header>
                   <soapenv:Body>
                      <moid:getDeadPersonInfo>                        
                           <moid:emiratesID>" + input.EmiratesID + @"</moid:emiratesID>
                      </moid:getDeadPersonInfo>
                   </soapenv:Body>
                </soapenv:Envelope> ");

                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        StringReader StringStream = new StringReader(soapResult);
                        DataSet ds = new DataSet();
                        ds.ReadXml(StringStream);

                        DataTable dt = ds.Tables["person"];
                        if (dt != null)
                            foreach (DataRow dr in dt.Rows)
                            {
                                objDPResponse = new DPDeathQueryDeatils.DPDeathQueryDeatilsResponse();
                                objDPResponse.EMIRATES_ID = dr["personNationID"] != null ? dr["personNationID"].ToString() : string.Empty;
                                objDPResponse.NAME_ARABIC = dr["nameAr"] != null ? dr["nameAr"].ToString() : string.Empty;
                                objDPResponse.NAME_ENGLISH = dr["nameEn"] != null ? dr["nameEn"].ToString() : string.Empty;
                                objDPResponse.UNIFIED_NO = dr["personUnifiedNo"] != null ? dr["personUnifiedNo"].ToString() : string.Empty;
                                objDPResponse.NATIONALITY_CODE = dr["nationalityCode"] != null ? dr["nationalityCode"].ToString() : string.Empty;
                                objDPResponse.NATIONALITY_DESC = dr["nationalityDesc"] != null ? dr["nationalityDesc"].ToString() : string.Empty;
                                objDPResponse.DOB = dr["dateOfBirth"] != null ? IEUtils.ToString(dr["dateOfBirth"]) : string.Empty;
                                objDPResponse.GENDER = dr["gender"] != null ? IEUtils.ToInt(dr["gender"]) : -1;
                                objDPResponse.GENDER_DESC = dr["genderDesc"] != null ? dr["genderDesc"].ToString() : string.Empty;
                                objDPResponse.MOTHER_NAME_AR = dr["motherNameAr"] != null ? dr["motherNameAr"].ToString() : string.Empty;
                                objDPResponse.CASE_DATE = dr["caseDate"] != null ? IEUtils.ToString(dr["caseDate"]) : string.Empty;
                                lstDPResponse.Add(objDPResponse);
                            }
                    }
                }

                if (lstDPResponse != null && lstDPResponse.Count > 0)
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { lstDPResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<DPDeathQueryDeatils.DPDeathQueryDeatilsResponse>>(lstDPResponse), ConfigurationManager.AppSettings["DPDEATHCode"].ToString(), ConfigurationManager.AppSettings["DPDEATH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DPDEATHCode"].ToString(), ConfigurationManager.AppSettings["DPDEATH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DPDEATHCode"].ToString(), ConfigurationManager.AppSettings["DPDEATH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {

                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DPDEATHCode"].ToString(), ConfigurationManager.AppSettings["DPDEATH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DPDEATHCode"].ToString(), ConfigurationManager.AppSettings["DPDEATH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DPDEATHCode"].ToString(), ConfigurationManager.AppSettings["DPDEATH"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}