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
    public class AJREController : Controller
    {
        // GET: AJRE
        public ActionResult Index()
        {
            return View("AJRE");
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

                AJREService.AJM_RE_getOwnerPropertyDetailsbyEIDPortTypeClient client = new AJREService.AJM_RE_getOwnerPropertyDetailsbyEIDPortTypeClient();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    AJREService.GetOwnerPropertyDetails objRequest = new AJREService.GetOwnerPropertyDetails();
                    objRequest.EmiratesID = input.EmiratesID.Trim();
                    objRequest.PreviousProperties = "true";

                    AJREService.GetOwnerPropertyDetailsResponse objAJRE = client.GetOwnerPropertyDetails(objRequest);

                    AJREService.GetOwnerPropertyDetailsResponseError[] objError = objAJRE.Error;
                    AJREService.GetOwnerPropertyDetailsResponseOwnerPropertyDetails[] objGetOwnerPropertyDetailsResponseOwnerPropertyDetails = null;
                    AJRE.OwnerInfo objOwnerInfo = null;
                    List<AJRE.OwnerInfo> lstOwnerInfo = new List<AJRE.OwnerInfo>();
                    AJRE.LandsInfo objLandsInfo = null;
                    List<AJRE.LandsInfo> lstLandsInfo = new List<AJRE.LandsInfo>();
                    AJRE.UnitsInfo objUnitsInfo = null;
                    List<AJRE.UnitsInfo> lstUnitsInfo = new List<AJRE.UnitsInfo>();



                    if (objError != null && objError.Length > 0)
                    {
                        foreach (AJREService.GetOwnerPropertyDetailsResponseError objERR in objError)
                        {
                            if (objERR.ErrorCode == "FCSA-200")
                            {
                                objGetOwnerPropertyDetailsResponseOwnerPropertyDetails = objAJRE.OwnerPropertyDetails;

                                foreach (AJREService.GetOwnerPropertyDetailsResponseOwnerPropertyDetails objOwn in objGetOwnerPropertyDetailsResponseOwnerPropertyDetails)
                                {
                                    objOwnerInfo = new AJRE.OwnerInfo();
                                    objOwnerInfo.OwnerIdentityId = objOwn.OwnerIdentityId;
                                    objOwnerInfo.OwnerNameAR = objOwn.OwnerNameAR;
                                    objOwnerInfo.OwnerNameEN = objOwn.OwnerNameEN;
                                    objOwnerInfo.OwnerNationalityCode = objOwn.OwnerNationalityCode;

                                    if (objOwn.UnitsInfo != null && objOwn.UnitsInfo.Length > 0)
                                        foreach (AJREService.GetOwnerPropertyDetailsResponseOwnerPropertyDetailsUnitsInfo objUnits in objOwn.UnitsInfo)
                                        {
                                            objUnitsInfo = new AJRE.UnitsInfo();
                                            objUnitsInfo.UnitProjectId = objUnits.UnitProjectId;
                                            objUnitsInfo.UnitPropertyId = objUnits.UnitPropertyId;
                                            objUnitsInfo.UnitMainProjectNameAR = objUnits.UnitMainProjectNameAR;
                                            objUnitsInfo.UnitMainProjectNameEN = objUnits.UnitMainProjectNameEN;
                                            objUnitsInfo.UnitCreatedAt = objUnits.UnitCreatedAt;
                                            objUnitsInfo.UnitShare = objUnits.UnitShare;
                                            lstUnitsInfo.Add(objUnitsInfo);
                                        }

                                    if (objOwn.LandsInfo != null && objOwn.LandsInfo.Length > 0)
                                        foreach (AJREService.GetOwnerPropertyDetailsResponseOwnerPropertyDetailsLandsInfo objLand in objOwn.LandsInfo)
                                        {
                                            objLandsInfo = new AJRE.LandsInfo();
                                            objLandsInfo.LandId = objLand.LandId;
                                            objLandsInfo.LandOwnershipType = objLand.LandOwnershipType;
                                            objLandsInfo.LandSectorAR = objLand.LandSectorAR;
                                            objLandsInfo.LandSectorEN = objLand.LandSectorEN;
                                            objLandsInfo.LandShare = objLand.LandShare;
                                            objLandsInfo.LandDistrictAR = objLand.LandDistrictAR;
                                            objLandsInfo.LandDistrictEN = objLand.LandDistrictEN;
                                            objLandsInfo.LandCityAR = objLand.LandCityAR;
                                            objLandsInfo.LandCityEN = objLand.LandCityEN;
                                            objLandsInfo.LandDeedId = objLand.LandDeedId;
                                            objLandsInfo.LandCreatedAt = objLand.LandCreatedAt;
                                            lstLandsInfo.Add(objLandsInfo);
                                        }
                                    objOwnerInfo.LandsInfoList = lstLandsInfo;
                                    objOwnerInfo.UnitsInfoList = lstUnitsInfo;
                                                                      
                                }
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { objOwnerInfo, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<AJRE.OwnerInfo>(objOwnerInfo), ConfigurationManager.AppSettings["AJRECode"].ToString(), ConfigurationManager.AppSettings["AJRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = "No Matching Records are Available";//objERR.ErrorCode + " - " + objERR.ErrorMessage;
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJRECode"].ToString(), ConfigurationManager.AppSettings["AJRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJRECode"].ToString(), ConfigurationManager.AppSettings["AJRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {

                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJRECode"].ToString(), ConfigurationManager.AppSettings["AJRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJRECode"].ToString(), ConfigurationManager.AppSettings["AJRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["AJRECode"].ToString(), ConfigurationManager.AppSettings["AJRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}

