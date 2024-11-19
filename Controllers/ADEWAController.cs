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

namespace MOCDIntegrations.Controllers
{
    public class ADEWAController : Controller
    {
        // GET: ADEWA
        public ActionResult Index()
        {
            return View("ADEWA");
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
            JsonHelper objHelper = new JsonHelper();
            ADEWA.ResponseTrnHeaderType objResponseADDCTrnHeaderType = null;
            List<ADEWADetails.ADEWADetailsResponseParams> lstResponseParams = new List<ADEWADetails.ADEWADetailsResponseParams>();
            try
            {
                var input = new JavaScriptSerializer().Deserialize<ADEWADetails.ADEWADetailsRequestParams>(postdata);

                string AccountID = string.Empty;
                string AccountStatus = string.Empty;
                decimal CurrentBalance = 0;
                string CustomerClassArabicName = string.Empty;
                string CustomerClassCode = string.Empty;
                string CustomerClassEnglishName = string.Empty;
                string CustomerMobileNumber = string.Empty;
                string CustomerNameArabic = string.Empty;
                string CustomerNameEnglish = string.Empty;
                string CustomerPremiseTypeArabicName = string.Empty;
                string CustomerPremiseTypeCode = string.Empty;
                string CustomerPremiseTypeEnglishName = string.Empty;
                string EmiratesIDNumber = string.Empty;
                string LandlordName = string.Empty;
                string PersonPrimaryAddress = string.Empty;
                string PlotNumber = string.Empty;
                string PremiseAddress = string.Empty;
                string PremiseArea = string.Empty;
                string PremiseCityPostal = string.Empty;
                string PremiseID = string.Empty;
                string SectorCommunityCode = string.Empty;
                string TradeLicense = string.Empty;
                string ZoneDistrictCode = string.Empty;

                ADEWA.WaterElectricityBillClient client = new ADEWA.WaterElectricityBillClient();

                bool value = false;
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {


                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                             Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["ADEWA_Username"].ToString() + ":" + ConfigurationManager.AppSettings["ADEWA_Password"].ToString()));
                    httpRequestProperty.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["ADEWA_Key"].ToString();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    ADEWA.WaterElectricityBillList[] objWaterElectricityBillLists = null;


                    ADEWA.RequestTrnHeaderType objRequestTrnHeaderType = new ADEWA.RequestTrnHeaderType();
                    objRequestTrnHeaderType.EntityCode = ConfigurationManager.AppSettings["Entity"].ToString();

                    ADEWA.CompanyType objCompanyName = ADEWA.CompanyType.ADDC;
                    ADEWA.CompanyType objCompanyNameDetails = ADEWA.CompanyType.AADC;

                    ADEWA.CustomerClass objCustomerClass = new ADEWA.CustomerClass();
                    ADEWA.CustomerName objCustomerName = new ADEWA.CustomerName();
                    ADEWA.CustomerPremiseType objCustomerPremiseType = new ADEWA.CustomerPremiseType();

                    ADEWADetails.ADEWADetailsResponseParams objResponse = null;

                    try
                    {

                        ADEWA.GetWaterElectricityBillList objADDCBillList = new ADEWA.GetWaterElectricityBillList();
                        objADDCBillList.Company = objCompanyName;
                        objADDCBillList.EmiratesIDNumber = input.EmiratesID;

                        objResponseADDCTrnHeaderType = client.GetWaterElectricityBillList(objRequestTrnHeaderType, objADDCBillList, out objWaterElectricityBillLists);


                        if (objWaterElectricityBillLists != null)
                            foreach (ADEWA.WaterElectricityBillList objWaterElectricityBillList in objWaterElectricityBillLists)
                            {
                                objResponse = new ADEWADetails.ADEWADetailsResponseParams();

                                objResponse.AccountID = AccountID = objWaterElectricityBillList.AccountID;   //"8374136155";
                                objResponse.PremiseID = PremiseID = objWaterElectricityBillList.PremiseID; //"4000010304";

                                try
                                {
                                    value = true;
                                    objResponseADDCTrnHeaderType = client.GetWaterElectricityBillDetails(objRequestTrnHeaderType, ref AccountID, objCompanyName
                                       , ref PremiseID, out AccountStatus, out CurrentBalance, out objCustomerClass, out CustomerMobileNumber, out objCustomerName, out objCustomerPremiseType,
                                       out EmiratesIDNumber, out LandlordName, out PersonPrimaryAddress, out PlotNumber, out PremiseAddress,
                                      out PremiseArea, out PremiseCityPostal, out SectorCommunityCode, out TradeLicense, out ZoneDistrictCode);
                                }
                                catch (FaultException faultException)
                                {
                                    //var fault = faultException.CreateMessageFault();
                                    //var doc = new XmlDocument();
                                    //var innerdoc = new XmlDocument();
                                    //var innersdoc = new XmlDocument();
                                    //var nav = doc.CreateNavigator();

                                    //if (fault.HasDetail)
                                    //{
                                    //    if (nav != null)
                                    //    {
                                    //        using (var writer = nav.AppendChild())
                                    //        {
                                    //            fault.WriteTo(writer, EnvelopeVersion.Soap12);
                                    //        }

                                    //        string str = string.Empty; //do something with it
                                    //        foreach (XmlNode child in doc.DocumentElement.ChildNodes)
                                    //        {

                                    //            if (child.Name == "Code")
                                    //            {
                                    //                innerdoc.LoadXml(child.InnerXml);
                                    //                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                    //                {
                                    //                    str += "Contact GSB Support.";
                                    //                }
                                    //            }

                                    //            if (child.Name == "Detail")
                                    //            {
                                    //                innerdoc.LoadXml(child.InnerXml);
                                    //                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                    //                {
                                    //                    if (chd.Name == "ErrorMessageAr")
                                    //                    {
                                    //                        str += chd.InnerText + " - ";
                                    //                    }
                                    //                    if (chd.Name == "ErrorMessageEn")
                                    //                    {
                                    //                        str += chd.InnerText;
                                    //                    }

                                    //                    if (chd.Name == "details")
                                    //                    {
                                    //                        innersdoc.LoadXml(chd.InnerXml);
                                    //                        foreach (XmlNode chds in innersdoc.DocumentElement.ChildNodes)
                                    //                        {
                                    //                            if (chds.Name == "message")
                                    //                            {
                                    //                                str += chd.InnerText;
                                    //                            }
                                    //                        }
                                    //                    }
                                    //                }

                                    //            }
                                    //        }
                                    //        ViewData["ADDCError"] = str;
                                    //    }
                                    //}
                                    //else
                                    //    ViewData["ADDCError"] = fault.Reason.ToString();
                                    value = false;
                                }
                                catch (Exception ex)
                                {
                                    value = false;
                                }

                                if (value)
                                {
                                    objResponse.AccountStatus = AccountStatus;
                                    objResponse.CurrentBalance = CurrentBalance.ToString();
                                    objResponse.CustomerClassArabicName = CustomerClassArabicName = objCustomerClass.CustomerClassArabicName;
                                    objResponse.CustomerClassCode = CustomerClassCode = objCustomerClass.CustomerClassCode;
                                    objResponse.CustomerClassEnglishName = CustomerClassEnglishName = objCustomerClass.CustomerClassEnglishName; ;
                                    objResponse.CustomerMobileNumber = CustomerMobileNumber;
                                    objResponse.CustomerNameArabic = CustomerNameArabic = objCustomerName.CustomerNameArabic;
                                    objResponse.CustomerNameEnglish = CustomerNameEnglish = objCustomerName.CustomerNameEnglish;
                                    objResponse.CustomerPremiseTypeArabicName = CustomerPremiseTypeArabicName = objCustomerPremiseType.CustomerPremiseTypeArabicName;
                                    objResponse.CustomerPremiseTypeCode = CustomerPremiseTypeCode = objCustomerPremiseType.CustomerPremiseTypeCode;
                                    objResponse.CustomerPremiseTypeEnglishName = CustomerPremiseTypeEnglishName = objCustomerPremiseType.CustomerPremiseTypeEnglishName;
                                    objResponse.EmiratesIDNumber = EmiratesIDNumber;
                                    objResponse.LandlordName = LandlordName;
                                    objResponse.PersonPrimaryAddress = PersonPrimaryAddress;
                                    objResponse.PlotNumber = PlotNumber;
                                    objResponse.PremiseAddress = PremiseAddress;
                                    objResponse.PremiseArea = PremiseArea;
                                    objResponse.PremiseCityPostal = PremiseCityPostal;
                                    objResponse.SectorCommunityCode = SectorCommunityCode;
                                    lstResponseParams.Add(objResponse);
                                }

                            }

                    }
                    catch (FaultException faultException)
                    {
                        //var fault = faultException.CreateMessageFault();
                        //var doc = new XmlDocument();
                        //var innerdoc = new XmlDocument();
                        //var innersdoc = new XmlDocument();
                        //var nav = doc.CreateNavigator();

                        //if (fault.HasDetail)
                        //{
                        //    if (nav != null)
                        //    {
                        //        using (var writer = nav.AppendChild())
                        //        {
                        //            fault.WriteTo(writer, EnvelopeVersion.Soap12);
                        //        }

                        //        string str = string.Empty; //do something with it
                        //        foreach (XmlNode child in doc.DocumentElement.ChildNodes)
                        //        {

                        //            if (child.Name == "Code")
                        //            {
                        //                innerdoc.LoadXml(child.InnerXml);
                        //                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                        //                {
                        //                    str += "Contact GSB Support.";
                        //                }
                        //            }

                        //            if (child.Name == "Detail")
                        //            {
                        //                innerdoc.LoadXml(child.InnerXml);
                        //                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                        //                {
                        //                    if (chd.Name == "ErrorMessageAr")
                        //                    {
                        //                        str += chd.InnerText + " - ";
                        //                    }
                        //                    if (chd.Name == "ErrorMessageEn")
                        //                    {
                        //                        str += chd.InnerText;
                        //                    }

                        //                    if (chd.Name == "details")
                        //                    {
                        //                        innersdoc.LoadXml(chd.InnerXml);
                        //                        foreach (XmlNode chds in innersdoc.DocumentElement.ChildNodes)
                        //                        {
                        //                            if (chds.Name == "message")
                        //                            {
                        //                                str += chd.InnerText;
                        //                            }
                        //                        }
                        //                    }
                        //                }

                        //            }
                        //        }
                        //        ViewData["ADDCError"] = str;
                        //    }
                        //}
                        //else
                        //    ViewData["ADDCError"] = fault.Reason.ToString();
                    }
                    catch (WebException wex)
                    {
                        //var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                        //ViewData["ADDCError"] = resp;
                    }
                    catch (Exception ex)
                    {
                        //ViewData["ADDCError"] = ex.Message;
                    }

                    try
                    {


                        ADEWA.GetWaterElectricityBillList objAADCBillList = new ADEWA.GetWaterElectricityBillList();
                        objAADCBillList.Company = objCompanyNameDetails;
                        objAADCBillList.EmiratesIDNumber = input.EmiratesID;

                        ADEWA.ResponseTrnHeaderType objResponseAADCTrnHeaderType = client.GetWaterElectricityBillList(objRequestTrnHeaderType, objAADCBillList, out objWaterElectricityBillLists);

                        if (objWaterElectricityBillLists != null)
                            foreach (ADEWA.WaterElectricityBillList objWaterElectricityBillList in objWaterElectricityBillLists)
                            {
                                objResponse = new ADEWADetails.ADEWADetailsResponseParams();
                                objResponse.AccountID = AccountID = objWaterElectricityBillList.AccountID;
                                objResponse.PremiseID = PremiseID = objWaterElectricityBillList.PremiseID;

                                try
                                {
                                    value = true;
                                    objResponseAADCTrnHeaderType = client.GetWaterElectricityBillDetails(objRequestTrnHeaderType, ref AccountID, objCompanyNameDetails
                                       , ref PremiseID, out AccountStatus, out CurrentBalance, out objCustomerClass, out CustomerMobileNumber, out objCustomerName, out objCustomerPremiseType,
                                       out EmiratesIDNumber, out LandlordName, out PersonPrimaryAddress, out PlotNumber, out PremiseAddress,
                                      out PremiseArea, out PremiseCityPostal, out SectorCommunityCode, out TradeLicense, out ZoneDistrictCode);
                                }
                                catch (FaultException faultException)
                                {
                                    //var fault = faultException.CreateMessageFault();
                                    //var doc = new XmlDocument();
                                    //var innerdoc = new XmlDocument();
                                    //var innersdoc = new XmlDocument();
                                    //var nav = doc.CreateNavigator();

                                    //if (fault.HasDetail)
                                    //{
                                    //    if (nav != null)
                                    //    {
                                    //        using (var writer = nav.AppendChild())
                                    //        {
                                    //            fault.WriteTo(writer, EnvelopeVersion.Soap12);
                                    //        }

                                    //        string str = string.Empty; //do something with it
                                    //        foreach (XmlNode child in doc.DocumentElement.ChildNodes)
                                    //        {

                                    //            if (child.Name == "Code")
                                    //            {
                                    //                innerdoc.LoadXml(child.InnerXml);
                                    //                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                    //                {
                                    //                    str += "Contact GSB Support.";
                                    //                }
                                    //            }

                                    //            if (child.Name == "Detail")
                                    //            {
                                    //                innerdoc.LoadXml(child.InnerXml);
                                    //                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                    //                {
                                    //                    if (chd.Name == "ErrorMessageAr")
                                    //                    {
                                    //                        str += chd.InnerText + " - ";
                                    //                    }
                                    //                    if (chd.Name == "ErrorMessageEn")
                                    //                    {
                                    //                        str += chd.InnerText;
                                    //                    }

                                    //                    if (chd.Name == "details")
                                    //                    {
                                    //                        innersdoc.LoadXml(chd.InnerXml);
                                    //                        foreach (XmlNode chds in innersdoc.DocumentElement.ChildNodes)
                                    //                        {
                                    //                            if (chds.Name == "message")
                                    //                            {
                                    //                                str += chd.InnerText;
                                    //                            }
                                    //                        }
                                    //                    }
                                    //                }

                                    //            }
                                    //        }
                                    //        ViewData["ADDCError"] = str;
                                    //    }
                                    //}
                                    //else
                                    //    ViewData["ADDCError"] = fault.Reason.ToString();
                                    value = false;
                                }
                                catch (Exception ex)
                                {
                                    value = false; ;
                                }

                                if (value)
                                {
                                    objResponse.AccountStatus = AccountStatus;
                                    objResponse.CurrentBalance = CurrentBalance.ToString();
                                    objResponse.CustomerClassArabicName = CustomerClassArabicName = objCustomerClass.CustomerClassArabicName;
                                    objResponse.CustomerClassCode = CustomerClassCode = objCustomerClass.CustomerClassCode;
                                    objResponse.CustomerClassEnglishName = CustomerClassEnglishName = objCustomerClass.CustomerClassEnglishName; ;
                                    objResponse.CustomerMobileNumber = CustomerMobileNumber;
                                    objResponse.CustomerNameArabic = CustomerNameArabic = objCustomerName.CustomerNameArabic;
                                    objResponse.CustomerNameEnglish = CustomerNameEnglish = objCustomerName.CustomerNameEnglish;
                                    objResponse.CustomerPremiseTypeArabicName = CustomerPremiseTypeArabicName = objCustomerPremiseType.CustomerPremiseTypeArabicName;
                                    objResponse.CustomerPremiseTypeCode = CustomerPremiseTypeCode = objCustomerPremiseType.CustomerPremiseTypeCode;
                                    objResponse.CustomerPremiseTypeEnglishName = CustomerPremiseTypeEnglishName = objCustomerPremiseType.CustomerPremiseTypeEnglishName;
                                    objResponse.EmiratesIDNumber = EmiratesIDNumber;
                                    objResponse.LandlordName = LandlordName;
                                    objResponse.PersonPrimaryAddress = PersonPrimaryAddress;
                                    objResponse.PlotNumber = PlotNumber;
                                    objResponse.PremiseAddress = PremiseAddress;
                                    objResponse.PremiseArea = PremiseArea;
                                    objResponse.PremiseCityPostal = PremiseCityPostal;
                                    objResponse.SectorCommunityCode = SectorCommunityCode;
                                    lstResponseParams.Add(objResponse);
                                }

                            }

                    }
                    catch (FaultException faultException)
                    {
                        //var fault = faultException.CreateMessageFault();
                        //var doc = new XmlDocument();
                        //var innerdoc = new XmlDocument();
                        //var innersdoc = new XmlDocument();
                        //var nav = doc.CreateNavigator();

                        //if (fault.HasDetail)
                        //{
                        //    if (nav != null)
                        //    {
                        //        using (var writer = nav.AppendChild())
                        //        {
                        //            fault.WriteTo(writer, EnvelopeVersion.Soap12);
                        //        }

                        //        string str = string.Empty; //do something with it
                        //        foreach (XmlNode child in doc.DocumentElement.ChildNodes)
                        //        {

                        //            if (child.Name == "Code")
                        //            {
                        //                innerdoc.LoadXml(child.InnerXml);
                        //                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                        //                {
                        //                    str += "Contact GSB Support.";
                        //                }
                        //            }

                        //            if (child.Name == "Detail")
                        //            {
                        //                innerdoc.LoadXml(child.InnerXml);
                        //                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                        //                {
                        //                    if (chd.Name == "ErrorMessageAr")
                        //                    {
                        //                        str += chd.InnerText + " - ";
                        //                    }
                        //                    if (chd.Name == "ErrorMessageEn")
                        //                    {
                        //                        str += chd.InnerText;
                        //                    }

                        //                    if (chd.Name == "details")
                        //                    {
                        //                        innersdoc.LoadXml(chd.InnerXml);
                        //                        foreach (XmlNode chds in innersdoc.DocumentElement.ChildNodes)
                        //                        {
                        //                            if (chds.Name == "message")
                        //                            {
                        //                                str += chd.InnerText;
                        //                            }
                        //                        }
                        //                    }
                        //                }

                        //            }
                        //        }
                        //        ViewData["AADCError"] = str;
                        //    }
                        //}
                        //else
                        //    ViewData["AADCError"] = fault.Reason.ToString();
                    }
                    catch (Exception ex)
                    {
                        //ViewData["AADCError"] = ex.Message;
                    }                  

                }

                if (lstResponseParams != null && lstResponseParams.Count > 0)
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { lstResponseParams, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<ADEWADetails.ADEWADetailsResponseParams>>(lstResponseParams), ConfigurationManager.AppSettings["ADEWACode"].ToString(), ConfigurationManager.AppSettings["ADEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Available";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADEWACode"].ToString(), ConfigurationManager.AppSettings["ADEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADEWACode"].ToString(), ConfigurationManager.AppSettings["ADEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {

                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADEWACode"].ToString(), ConfigurationManager.AppSettings["ADEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException wex)
            {
                flag = 3;
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADEWACode"].ToString(), ConfigurationManager.AppSettings["ADEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }); ViewData["AADCError"] = ex.Message;
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADEWACode"].ToString(), ConfigurationManager.AppSettings["ADEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }

}
