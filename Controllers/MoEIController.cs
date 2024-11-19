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
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace MOCDIntegrations.Controllers
{
    public class MoEIController : Controller
    {
        // GET: MoEI
        public ActionResult Index()
        {
            return View("MoEI");
        }

      

        public ActionResult Search(string index, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<MoEIDetails.MoEIDetailsRequestParams>(postdata);
                MoEIService.IntegratedHousingSoapClient client = new MoEIService.IntegratedHousingSoapClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                     Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MoEIGSBUserName"].ToString() + ":" + ConfigurationManager.AppSettings["MoEIGSBPassword"].ToString()));
                    httpRequestProperty.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["MoEIGSBAPIKey"].ToString();
                    httpRequestProperty.Headers["x-Gateway-APIKey"] = ConfigurationManager.AppSettings["MoEISDGKey"].ToString();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;


                    MoEIService.UserCredentials credentials = new MoEIService.UserCredentials();
                    credentials.Username = ConfigurationManager.AppSettings["MoEIUserName"].ToString();
                    credentials.Password = ConfigurationManager.AppSettings["MoEIPassword"].ToString();
                    credentials.APIKey = ConfigurationManager.AppSettings["MoEIAPIKey"].ToString();

                    MoEIService.getHousingDetailsRequest getHousingDetailsRequest = new MoEIService.getHousingDetailsRequest();

                    getHousingDetailsRequest.EmirateIdCardNo = input.EmiratesID;
                    getHousingDetailsRequest.TownNo = input.TNo;
                    getHousingDetailsRequest.FamilyNo = input.FNo;

                    getHousingDetailsRequest.UserCredentials = credentials;
                    string objResponse = client.getHousingDetails(credentials, input.EmiratesID, input.TNo, input.FNo);
                    MoEIService.getHousingDetailsResponse getHousingDetailsResponse = new MoEIService.getHousingDetailsResponse(objResponse);
                    if (getHousingDetailsResponse.getHousingDetailsResult == "Error Code 101")
                    {
                        flag = 2;
                        string ResponseDescription = "API Key Is Invalid";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    }
                    else if (getHousingDetailsResponse.getHousingDetailsResult == "Error Code 104")
                    {
                        flag = 2;
                        string ResponseDescription = "Login Invalid";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                    }
                    else if (getHousingDetailsResponse.getHousingDetailsResult == "Error Code 106")
                    {
                        flag = 2;
                        string ResponseDescription = "Fail";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    }
                    else if (getHousingDetailsResponse.getHousingDetailsResult == "Error Code 108")
                    {
                        flag = 2;
                        string ResponseDescription = "No Record Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                    }
                    else if (getHousingDetailsResponse.getHousingDetailsResult == "Error Code 112")
                    {
                        flag = 2;
                        string ResponseDescription = "Invalid EmirateIdCardNo OR (TownNo and Familyno)";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                    }
                    else if (!string.IsNullOrEmpty(getHousingDetailsResponse.getHousingDetailsResult))
                    {
                        flag = 1;

                        XDocument xmlDoc = XDocument.Parse(getHousingDetailsResponse.getHousingDetailsResult);
                        XElement node1Element = xmlDoc.Descendants("Table").FirstOrDefault();
                        List<MoEIDetails.MoEIDetailsResponseParams> lstResponse = new List<MoEIDetails.MoEIDetailsResponseParams>();
                        MoEIDetails.MoEIDetailsResponseParams Response = new MoEIDetails.MoEIDetailsResponseParams();

                        Response.NameAra = node1Element.Element("NameAra").Value;
                        Response.Emirate = node1Element.Element("Emirate").Value;
                        Response.Email = node1Element.Element("Email").Value;
                        Response.TotalSalary = node1Element.Element("TotalSalary").Value;
                        Response.ReqUseOfAid = node1Element.Element("ReqUseOfAid").Value;
                        Response.DecreeDate = node1Element.Element("DecreeDate").Value;
                        Response.ApplicationDate = node1Element.Element("ApplicationDate").Value;
                        Response.BirthDate = node1Element.Element("BirthDate").Value;
                        Response.Employer = node1Element.Element("Employer").Value;
                        Response.HouseOwnNameAra = node1Element.Element("HouseOwnNameAra").Value;
                        Response.ApplicationStateNameAra = node1Element.Element("ApplicationStateNameAra").Value;
                        Response.FileDate = node1Element.Element("FileDate").Value;
                        Response.AssistantStateNameAra = node1Element.Element("AssistantStateNameAra").Value;
                        Response.Amount = node1Element.Element("Amount").Value;
                        Response.FamilyNo = node1Element.Element("FamilyNo").Value;
                        Response.TownNo = node1Element.Element("TownNo").Value;
                        Response.ApplicationNo = node1Element.Element("ApplicationNo").Value;
                        Response.Mobileno = node1Element.Element("Mobileno").Value;
                        Response.DecreeSubTypeNameAra = node1Element.Element("DecreeSubTypeNameAra").Value;
                        Response.ReqAidType = node1Element.Element("ReqAidType").Value;
                        Response.HLCEast = node1Element.Element("HLCEast").Value;
                        Response.HLCNorth = node1Element.Element("HLCNorth").Value;
                        Response.EmirateIdCardNo = node1Element.Element("EmirateIdCardNo").Value;
                        Response.RegionName = node1Element.Element("RegionName").Value;
                        Response.HasLand = node1Element.Element("HasLand").Value;
                            

                        lstResponse.Add(Response);
                        json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<MoEIDetails.MoEIDetailsResponseParams>>(lstResponse), ConfigurationManager.AppSettings["MoEICode"].ToString(), ConfigurationManager.AppSettings["MoEI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Data";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MoEICode"].ToString(), ConfigurationManager.AppSettings["MoEI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MoEICode"].ToString(), ConfigurationManager.AppSettings["MoEI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    ResponseDescription = faultException.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MoEICode"].ToString(), ConfigurationManager.AppSettings["MoEI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MoEICode"].ToString(), ConfigurationManager.AppSettings["MoEI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MoEICode"].ToString(), ConfigurationManager.AppSettings["MoEI"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}