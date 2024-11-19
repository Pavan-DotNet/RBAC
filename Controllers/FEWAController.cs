using MOCDIntegrations.Models;
using MOCDIntegrations.Models.FEWA;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Org.BouncyCastle.Asn1.Ocsp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Xml;
using static MOCDIntegrations.Models.OwnerDetails;

namespace MOCDIntegrations.Controllers
{
    public class FEWAController : Controller
    {
        // GET: PersonalProfile
        public ActionResult Index()
        {
            return View("FEWA");
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
        private string LoadTransId()
        {
            return "MOCD_ICA_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }
        private (Models.FEWA.Root, Master) FEWAApiCall(string mode, string emirateID)
        {
            //string DATA = @"{""Mode"":""01"", ""SearchString"": " + emirateID + "  }";
            string DATA = @"{"+'\u0022'+"Mode"+ '\u0022'+":"+ '\u0022' + mode + '\u0022' + ","+ '\u0022'+"SearchString"+ '\u0022'+":"+  emirateID +  "}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["FEWA_Url"].ToString());
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = DATA.Length;
            var secKey = Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["FEWA_user"].ToString() + ":" + ConfigurationManager.AppSettings["FEWA_password"].ToString()));
            request.Headers.Add("Authorization", "Basic " + secKey);
            request.Headers.Add("GSB-APIKey", ConfigurationManager.AppSettings["FEWA_API_KEY"].ToString());

            using (Stream webStream = request.GetRequestStream())
            using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
            {
                requestWriter.Write(DATA);
            }

            Models.FEWA.Root objresp = null;
            Master obj = null;
            WebResponse webResponse = request.GetResponse();
            using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
            using (StreamReader responseReader = new StreamReader(webStream))
            {
                string response = responseReader.ReadToEnd();

                if (response.Contains(@"""AccountDetails"":["))
                {
                    objresp = JsonConvert.DeserializeObject<Models.FEWA.Root>(response);
                    foreach (var item in objresp.AccountDetails)
                    {
                        if (!string.IsNullOrEmpty(item?.LatestInvoice?.Date))
                        {
                            string yyyy = item.LatestInvoice.Date.Substring(0, 4);
                            string MM = item.LatestInvoice.Date.Substring(4, 2);
                            string dd = item.LatestInvoice.Date.Substring(6);
                            DateTime dt = Convert.ToDateTime(MM + "/" + dd + "/" + yyyy);
                            item.LatestInvoice.Date = dt.ToString("MMM-yyyy-dd");
                        }
                    }
                }
                else
                {
                    obj = JsonConvert.DeserializeObject<Master>(response);
                    if (!string.IsNullOrEmpty(obj?.AccountDetails?.LatestInvoice?.Date))
                    {
                        string yyyy = obj.AccountDetails.LatestInvoice.Date.Substring(0, 4);
                        string MM = obj.AccountDetails.LatestInvoice.Date.Substring(4, 2);
                        string dd = obj.AccountDetails.LatestInvoice.Date.Substring(6);
                        DateTime dt = Convert.ToDateTime(MM + "/" + dd + "/" + yyyy);
                        obj.AccountDetails.LatestInvoice.Date = dt.ToString("MMM-yyyy-dd");
                    }
                }
                

                // objresp.AccountDetails.LatestInvoice.Date= Convert.ToDateTime(objresp.AccountDetails.LatestInvoice.Date).ToString("dd-MM-yyyy");
            }
            return (objresp,obj);
        }
        
      
        public ActionResult Search(string Mode , string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            JsonHelper objHelper = new JsonHelper();
            try
            {
                var obj = FEWAApiCall(Mode, EmiratesId);
                if (obj.Item2 != null )
                {
                    if (obj.Item2 != null && obj.Item2.AccountDetails != null)
                    {
                        flag = 1;
                        Models.FEWA.Root lstResponse = new Models.FEWA.Root();
                        lstResponse.AccountDetails = new List<AccountDetail>();
                        lstResponse.AccountDetails.Add(obj.Item2.AccountDetails);
                        json = JsonConvert.SerializeObject(new { lstResponse, flag, }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, objHelper.ConvertObjectToJSon<Models.FEWA.Root>(lstResponse), ConfigurationManager.AppSettings["FEWACode"].ToString(), ConfigurationManager.AppSettings["FEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        flag = 2;
                        Models.FEWA.Root lstResponse = new Models.FEWA.Root();
                        lstResponse.AccountDetails = new List<AccountDetail>();
                        lstResponse.AccountDetails.Add(obj.Item2.AccountDetails);
                        json = JsonConvert.SerializeObject(new { lstResponse, flag, }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, objHelper.ConvertObjectToJSon<Models.FEWA.Root>(lstResponse), ConfigurationManager.AppSettings["FEWACode"].ToString(), ConfigurationManager.AppSettings["FEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (obj.Item1 != null && obj.Item1.AccountDetails != null && obj.Item1.AccountDetails.Count > 0)
                    {
                        flag = 1;
                        Models.FEWA.Root lstResponse = obj.Item1 as Models.FEWA.Root;
                        json = JsonConvert.SerializeObject(new { lstResponse, flag, }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, objHelper.ConvertObjectToJSon<Models.FEWA.Root>(lstResponse), ConfigurationManager.AppSettings["FEWACode"].ToString(), ConfigurationManager.AppSettings["FEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        flag = 2;
                        Models.FEWA.Root lstResponse = obj.Item1 as Models.FEWA.Root;
                        json = JsonConvert.SerializeObject(new { lstResponse, flag, }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, objHelper.ConvertObjectToJSon<Models.FEWA.Root>(lstResponse), ConfigurationManager.AppSettings["FEWACode"].ToString(), ConfigurationManager.AppSettings["FEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        return Json(json, JsonRequestBehavior.AllowGet);
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
                                    str += "FEWA Support.";
                                }
                            }
                            if (child.Name == "Detail")
                            {
                                ResponseDescription = child.InnerXml;
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }); ;
                                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    return Json(json, JsonRequestBehavior.AllowGet);

                }
            }
            catch (WebException ex)
            {
                var ResponseDescription = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["FEWACode"].ToString(), ConfigurationManager.AppSettings["FEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
    }
}