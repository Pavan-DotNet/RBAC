using MOCDIntegrations.Models;
using MOCDIntegrations.Utils;
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
using static MOCDIntegrations.Models.TAWTHEEQDetails;

namespace MOCDIntegrations.Controllers
{
    [CustomJsonFilter]

    public class TAWTHEEQController : Controller
    {
        // GET: TAWTHEEQ
        public ActionResult Index()
        {
            return View("TAWTHEEQ");
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
        [HttpPost]
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<TAWTHEEQDetails.TawtheeqRequestParam>(postdata);
                DMAT.tawtheeqPortTypeClient client = new DMAT.tawtheeqPortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {

                    var httpRequestProperty = new HttpRequestMessageProperty(); 
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                                                                    Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["TAWTHEEQ_Username"].ToString() + ":" + ConfigurationManager.AppSettings["TAWTHEEQ_Password"].ToString()));
                    httpRequestProperty.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["TAWTHEEQ_Key"].ToString();

                    httpRequestProperty.Headers.Add("Entity", ConfigurationManager.AppSettings["TAWTHEEQEntity"].ToString());

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    DMAT.RequestTrnHeaderType objHeader = new DMAT.RequestTrnHeaderType();
                    objHeader.EntityCode = ConfigurationManager.AppSettings["TAWTHEEQEntity"].ToString();

                    DMAT.TawtheeqListRequestType objTawtheeqListRequestType = new DMAT.TawtheeqListRequestType();
                    objTawtheeqListRequestType.EmiratesIDNumber = input.EmiratesID;

                    DMAT.TawtheeqListResponseType objTawtheeqListResponseType = null;
                    DMAT.ResponseTrnHeaderType objResponseTrnHeaderType = client.getTawtheeqList(objHeader, objTawtheeqListRequestType, out objTawtheeqListResponseType);

                    List<TawtheeqDetailsRequest> lstTawtheeqDetailsRequest = new List<TawtheeqDetailsRequest>();
                    TawtheeqDetailsRequest objTawtheeqDetailsRequest = null;
                    DMAT.LookupType objLookup = new DMAT.LookupType();

                    if (objTawtheeqListResponseType != null && objTawtheeqListResponseType.ResponseStatus.StatusCode == "200")
                    {
                        ResponseStatus objRequestResponse = new ResponseStatus();
                        objRequestResponse.StatusCode = objTawtheeqListResponseType.ResponseStatus.StatusCode;
                        objRequestResponse.StatusDescriptionArabic = objTawtheeqListResponseType.ResponseStatus.StatusDescriptionArabic;
                        objRequestResponse.StatusDescriptionEnglish = objTawtheeqListResponseType.ResponseStatus.StatusDescriptionEnglish;

                        foreach (DMAT.TawtheeqListType objTawtheeqListType in objTawtheeqListResponseType.TawtheeqList)
                        {
                            objTawtheeqDetailsRequest = new TawtheeqDetailsRequest();
                            objTawtheeqDetailsRequest.objResponseStatus = objRequestResponse;
                            objTawtheeqDetailsRequest.ContractNo = objTawtheeqListType.ContractNo;
                            objTawtheeqDetailsRequest.ContractStartDate = objTawtheeqListType.ContractStartDate;
                            objTawtheeqDetailsRequest.ContractExpiryDate = objTawtheeqListType.ContractExpiryDate;
                            objLookup = objTawtheeqListType.ContractStatus;
                            objTawtheeqDetailsRequest.ContractStatusCode = objLookup.Code;
                            objTawtheeqDetailsRequest.ContractArabicDescription = objLookup.ArabicDescription;
                            objTawtheeqDetailsRequest.ContractEnglishDescription = objLookup.EnglishDescription;

                            lstTawtheeqDetailsRequest.Add(objTawtheeqDetailsRequest);
                        }
                        ViewData["REQUEST"] = lstTawtheeqDetailsRequest;


                        DMAT.TawtheeqRequestType objTawtheeqRequestType = null;
                        DMAT.TawtheeqAllDetailsResponseType objTawtheeqAllDetailsResponseType = null;
                        TawtheeqDetailsResponse objTawtheeqDetailsResponse = null;
                        List<TawtheeqDetailsResponse> lstTawtheeqDetailsResponse = null;
                        ContractDetails objContractDetails = null;
                        List<ContractDetails> lstContractDetails = null;
                        OwnersDetails objOwnersDetails = null;
                        List<OwnersDetails> lstOwnersDetails = null;
                        LessorDetails objLessorDetails = null;
                        List<LessorDetails> lstLessorDetails = null;
                        RentAndPaymentsDetails objRentAndPaymentsDetails = null;
                        List<RentAndPaymentsDetails> lstRentAndPaymentsDetails = null;
                        PropertyDetails objPropertyDetails = null;
                        List<PropertyDetails> lstPropertyDetails = null;
                        UnitDetails objUnitDetails = null;
                        List<UnitDetails> lstUnitDetails = null;
                        TenantDetails objTenantDetails = null;
                        List<TenantDetails> lstTenantDetails = null;
                        lstTawtheeqDetailsResponse = new List<TawtheeqDetailsResponse>();

                        foreach (TawtheeqDetailsRequest objRequest in lstTawtheeqDetailsRequest)
                        {
                            objTawtheeqRequestType = new DMAT.TawtheeqRequestType();
                            objTawtheeqRequestType.ContractNo = objRequest.ContractNo;

                            try
                            {
                                DMAT.ResponseTrnHeaderType objResponse = client.getTawtheeqDetails(objHeader, objTawtheeqRequestType, out objTawtheeqAllDetailsResponseType);
                            }
                            catch { objTawtheeqAllDetailsResponseType = null; }
                            if (objTawtheeqAllDetailsResponseType != null && objTawtheeqAllDetailsResponseType.ResponseStatus.StatusCode == "200")
                            {
                                ResponseStatus objLeaseResponse = new ResponseStatus();
                                objLeaseResponse.StatusCode = objTawtheeqAllDetailsResponseType.ResponseStatus.StatusCode;
                                objLeaseResponse.StatusDescriptionArabic = objTawtheeqAllDetailsResponseType.ResponseStatus.StatusDescriptionArabic;
                                objLeaseResponse.StatusDescriptionEnglish = objTawtheeqAllDetailsResponseType.ResponseStatus.StatusDescriptionEnglish;

                                DMAT.TawtheeqAllDetailsType objTawtheeqAllDetailsType = objTawtheeqAllDetailsResponseType.Contents;
                                lstContractDetails = new List<ContractDetails>();
                                lstOwnersDetails = new List<OwnersDetails>();
                                lstLessorDetails = new List<LessorDetails>();
                                lstRentAndPaymentsDetails = new List<RentAndPaymentsDetails>();
                                lstPropertyDetails = new List<PropertyDetails>();
                                lstUnitDetails = new List<UnitDetails>();
                                lstTenantDetails = new List<TenantDetails>();
                                objTawtheeqDetailsResponse = new TawtheeqDetailsResponse();


                                if (objTawtheeqAllDetailsType.ContractDetails != null)
                                {
                                    objContractDetails = new ContractDetails();
                                    DMAT.ContractDetailsType objContractDetailsType = objTawtheeqAllDetailsType.ContractDetails;
                                    objContractDetails.ContractNo = objContractDetailsType.ContractNo;
                                    objContractDetails.ContractRegistrationDate = objContractDetailsType.ContractRegistrationDate;
                                    objLookup = objContractDetailsType.ContractUsage;
                                    objContractDetails.Code = objLookup.Code;
                                    objContractDetails.ArabicDescription = objLookup.ArabicDescription;
                                    objContractDetails.EnglishDescription = objLookup.EnglishDescription;
                                    objContractDetails.StatusEnglishDescription = objRequest.ContractEnglishDescription;
                                    objContractDetails.StatusArabicDescription = objRequest.ContractArabicDescription;

                                    lstContractDetails.Add(objContractDetails);
                                }

                                if (objTawtheeqAllDetailsType.OwnersDetails != null && objTawtheeqAllDetailsType.OwnersDetails.Length > 0)
                                {
                                    foreach (DMAT.OwnersDetailsType objOwnersDetailsType in objTawtheeqAllDetailsType.OwnersDetails)
                                    {
                                        objOwnersDetails = new OwnersDetails();
                                        objOwnersDetails.PlotOwnerName = objOwnersDetailsType.PlotOwnerName;
                                        lstOwnersDetails.Add(objOwnersDetails);
                                    }
                                }

                                if (objTawtheeqAllDetailsType.LessorDetails != null)
                                {
                                    DMAT.LessorDetailsType objLessorDetailsType = objTawtheeqAllDetailsType.LessorDetails;
                                    objLessorDetails = new LessorDetails();
                                    objLessorDetails.ContactPersonName = objLessorDetailsType.ContactPersonName;
                                    objLessorDetails.eMailAddress = objLessorDetailsType.eMailAddress;
                                    objLessorDetails.MobileNumber = objLessorDetailsType.MobileNumber;
                                    objLessorDetails.Fax = objLessorDetailsType.Fax;
                                    if (objLessorDetailsType.LessorCity != null)
                                    {
                                        objLookup = objLessorDetailsType.LessorCity;
                                        objLessorDetails.LessorCityCode = objLookup.Code;
                                        objLessorDetails.LessorCityArabicDescription = objLookup.ArabicDescription;
                                        objLessorDetails.LessorCityEnglishDescription = objLookup.EnglishDescription;
                                    }
                                    objLessorDetails.LessorEmiratesID = objLessorDetailsType.LessorEmiratesID;
                                    objLessorDetails.LessorNameArabic = objLessorDetailsType.ContactPersonName;
                                    objLessorDetails.LessorNameEnglish = objLessorDetailsType.LessorNameEnglish;


                                    if (objLessorDetailsType.LessorNationality != null)
                                    {
                                        objLookup = objLessorDetailsType.LessorNationality;
                                        objLessorDetails.NationalityCode = objLookup.Code;
                                        objLessorDetails.NationalityArabicDescription = objLookup.ArabicDescription;
                                        objLessorDetails.NationalityEnglishDescription = objLookup.EnglishDescription;
                                        objLessorDetails.PhoneNumber = objLessorDetailsType.PhoneNumber;
                                        objLessorDetails.POBox = objLessorDetailsType.POBox;
                                        lstLessorDetails.Add(objLessorDetails);
                                    }
                                  
                                }

                                if (objTawtheeqAllDetailsType.RentAndPaymentsDetails != null)
                                {
                                    DMAT.RentAndPaymentsDetailsType objRentAndPaymentsDetailsType = objTawtheeqAllDetailsType.RentAndPaymentsDetails;
                                    objRentAndPaymentsDetails = new RentAndPaymentsDetails();
                                    objRentAndPaymentsDetails.ADWEABillPaidBy = objRentAndPaymentsDetailsType.ADWEABillPaidBy;
                                    objRentAndPaymentsDetails.ContractExpiryDate = objRentAndPaymentsDetailsType.ContractExpiryDate;
                                    objRentAndPaymentsDetails.ContractStartDate = objRentAndPaymentsDetailsType.ContractStartDate;
                                    objRentAndPaymentsDetails.NumberofInstallments = objRentAndPaymentsDetailsType.NumberofInstallments;
                                    objRentAndPaymentsDetails.RentalValue = objRentAndPaymentsDetailsType.RentalValue;
                                    objRentAndPaymentsDetails.SecurityDeposit = objRentAndPaymentsDetailsType.SecurityDeposit;
                                    objRentAndPaymentsDetails.TenancycontractDuration = objRentAndPaymentsDetailsType.TenancycontractDuration;
                                    lstRentAndPaymentsDetails.Add(objRentAndPaymentsDetails);
                                }

                                if (objTawtheeqAllDetailsType.TenantDetails != null)
                                {
                                    DMAT.TenantDetailsType objTenantDetailsType = objTawtheeqAllDetailsType.TenantDetails;
                                    objTenantDetails = new TenantDetails();
                                    objTenantDetails.TenantEmiratesID = objTenantDetailsType.TenantEmiratesID;
                                    objTenantDetails.TenantNameArabic = objTenantDetailsType.TenantNameArabic;
                                    objTenantDetails.TenantNameEnglish = objTenantDetailsType.TenantNameEnglish;

                                    if (objTenantDetailsType.TenantCity != null)
                                    {
                                        objLookup = objTenantDetailsType.TenantCity;
                                        objTenantDetails.TenantCityCode = objLookup.Code;
                                        objTenantDetails.TenantCityArabicDescription = objLookup.ArabicDescription;
                                        objTenantDetails.TenantCityEnglishDescription = objLookup.EnglishDescription;
                                    }

                                    if (objTenantDetailsType.TenantNationality != null)
                                    {
                                        objLookup = objTenantDetailsType.TenantNationality;
                                        if (objLookup != null)
                                        {
                                            objTenantDetails.NationalityCode = objLookup.Code;
                                            objTenantDetails.NationalityArabicDescription = objLookup.ArabicDescription;
                                            objTenantDetails.NationalityEnglishDescription = objLookup.EnglishDescription;
                                        }
                                    }



                                    objTenantDetails.PhoneNumber = objTenantDetailsType.PhoneNumber;
                                    objTenantDetails.POBox = objTenantDetailsType.POBox;
                                    objTenantDetails.IssuePlace = objTenantDetailsType.IssuePlace;
                                    objTenantDetails.eMailAddress = objTenantDetailsType.eMailAddress;
                                    objTenantDetails.TradeLicenseNo = objTenantDetailsType.TradeLicenseNo;
                                    lstTenantDetails.Add(objTenantDetails);
                                }

                                if (objTawtheeqAllDetailsType.PropertyDetails != null && objTawtheeqAllDetailsType.PropertyDetails.Length > 0)
                                {

                                    foreach (DMAT.PropertyDetailsType objPropertyDetailsType in objTawtheeqAllDetailsType.PropertyDetails)
                                    {
                                        objPropertyDetails = new PropertyDetails();
                                        objPropertyDetails.PlotNo = objPropertyDetailsType.PlotNo;
                                        objPropertyDetails.PremisesNumber = objPropertyDetailsType.PremisesNumber;
                                        objPropertyDetails.PropertyName = objPropertyDetailsType.PropertyName;
                                        objPropertyDetails.PropertyRegistrationNumber = objPropertyDetailsType.PropertyRegistrationNumber;

                                        if (objPropertyDetailsType.PropertyType != null)
                                        {
                                            objLookup = objPropertyDetailsType.PropertyType;
                                            objPropertyDetails.PropertyTypeCode = objLookup.Code;
                                            objPropertyDetails.PropertyTypeArabicDescription = objLookup.ArabicDescription;
                                            objPropertyDetails.PropertyTypeArabicDescription = objLookup.EnglishDescription;
                                        }

                                        if (objPropertyDetailsType.Sector != null)
                                        {
                                            objLookup = objPropertyDetailsType.Sector;
                                            objPropertyDetails.SectorCode = objLookup.Code;
                                            objPropertyDetails.SectorArabicDescription = objLookup.ArabicDescription;
                                            objPropertyDetails.SectorEnglishDescription = objLookup.EnglishDescription;
                                        }

                                        if (objPropertyDetailsType.Zone != null)
                                        {
                                            objLookup = objPropertyDetailsType.Zone;
                                            objPropertyDetails.ZoneCode = objLookup.Code;
                                            objPropertyDetails.ZoneArabicDescription = objLookup.ArabicDescription;
                                            objPropertyDetails.ZoneEnglishDescription = objLookup.EnglishDescription;
                                        }

                                        if (objPropertyDetailsType.StreetRoad != null)
                                        {
                                            objLookup = objPropertyDetailsType.StreetRoad;
                                            objPropertyDetails.StreetRoadCode = objLookup.Code;
                                            objPropertyDetails.StreetRoadArabicDescription = objLookup.ArabicDescription;
                                            objPropertyDetails.StreetRoadEnglishDescription = objLookup.EnglishDescription;
                                        }


                                        if (objPropertyDetailsType.UnitDetails != null && objPropertyDetailsType.UnitDetails.Length > 0)
                                        {
                                            lstUnitDetails = new List<UnitDetails>();
                                            foreach (DMAT.UnitDetailsType objUnitDetailsType in objPropertyDetailsType.UnitDetails)
                                            {
                                                objUnitDetails = new UnitDetails();
                                                objUnitDetails.UnitRegistrationNumber = objUnitDetailsType.UnitRegistrationNumber;
                                                objUnitDetails.UnitName = objUnitDetailsType.UnitName;

                                                objLookup = objUnitDetailsType.UnitType;
                                                objUnitDetails.UnitTypeCode = objLookup.Code;
                                                objUnitDetails.UnitTypeArabicDescription = objLookup.ArabicDescription;
                                                objUnitDetails.UnitTypeEnglishDescription = objLookup.EnglishDescription;

                                                objUnitDetails.PremisesNumber = objUnitDetailsType.PremisesNumber;

                                                if (objUnitDetailsType.OccupantsDetails != null && objUnitDetailsType.OccupantsDetails.Length > 0)
                                                {
                                                    foreach (DMAT.OccupantsDetailsType objOccupantsDetailsType in objUnitDetailsType.OccupantsDetails)
                                                    {
                                                        objUnitDetails.OccupantNameArabic = objOccupantsDetailsType.OccupantNameArabic;
                                                        objUnitDetails.OccupantNameEnglish = objOccupantsDetailsType.OccupantNameArabic;
                                                        objUnitDetails.OccupantsDetailsIDNumber = objOccupantsDetailsType.IDNumber;

                                                        objLookup = objOccupantsDetailsType.IDType;
                                                        objUnitDetails.IDTypeCode = objLookup.Code;
                                                        objUnitDetails.IDTypeArabicDescription = objLookup.ArabicDescription;
                                                        objUnitDetails.IDTypeEnglishDescription = objLookup.EnglishDescription;

                                                    }

                                                }

                                                objUnitDetails.NumberOfBedRooms = objUnitDetailsType.NumberOfBedRooms;
                                                objUnitDetails.OtherRoomsFloorNo = objUnitDetailsType.OtherRooms;
                                                objUnitDetails.OtherRoomsLeasedAreaM2 = objUnitDetailsType.LeasedAreaM2;
                                                objUnitDetails.OtherRoomsNoOfOccupants = objUnitDetailsType.NoOfOccupants;
                                                lstUnitDetails.Add(objUnitDetails);
                                            }
                                        }

                                        objPropertyDetails.lstUnitDetails = lstUnitDetails;
                                        lstPropertyDetails.Add(objPropertyDetails);
                                    }
                                }

                                objTawtheeqDetailsResponse.objResponseStatus = objLeaseResponse;
                                objTawtheeqDetailsResponse.lstContractDetails = lstContractDetails;
                                objTawtheeqDetailsResponse.lstLessorDetails = lstLessorDetails;
                                objTawtheeqDetailsResponse.lstOwnersDetails = lstOwnersDetails;
                                objTawtheeqDetailsResponse.lstRentAndPaymentsDetails = lstRentAndPaymentsDetails;
                                objTawtheeqDetailsResponse.lstPropertyDetails = lstPropertyDetails;
                                objTawtheeqDetailsResponse.lstTenantDetails = lstTenantDetails;
                                objTawtheeqDetailsResponse.listTawtheeqDetailsRequest = lstTawtheeqDetailsRequest;
                                lstTawtheeqDetailsResponse.Add(objTawtheeqDetailsResponse);
                            }
                        }

                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstTawtheeqDetailsResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                      
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<TawtheeqDetailsResponse>>(lstTawtheeqDetailsResponse), ConfigurationManager.AppSettings["TAWTHEEQCode"].ToString(), ConfigurationManager.AppSettings["TAWTHEEQ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                    else
                    {
                        flag = 2;
                        string ResponseDescription = objTawtheeqListResponseType.ResponseStatus.StatusDescriptionArabic + " - " + objTawtheeqListResponseType.ResponseStatus.StatusDescriptionEnglish;

                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["TAWTHEEQCode"].ToString(), ConfigurationManager.AppSettings["TAWTHEEQ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                                    str += "Contact GSB Support.";
                                }
                            }

                            if (child.Name == "Detail")
                            {
                                innerdoc.LoadXml(child.InnerXml);
                                foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                {
                                    if (chd.Name == "errorMessageArField")
                                    {
                                        str += chd.InnerText + " - ";
                                    }
                                    if (chd.Name == "errorMessageEnField")
                                    {
                                        str += chd.InnerText;
                                    }

                                    if (chd.Name == "details")
                                    {
                                        innersdoc.LoadXml(chd.InnerXml);
                                        foreach (XmlNode chds in innersdoc.DocumentElement.ChildNodes)
                                        {
                                            if (chds.Name == "message")
                                            {
                                                str += chd.InnerText;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        ResponseDescription = str;
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["TAWTHEEQCode"].ToString(), ConfigurationManager.AppSettings["TAWTHEEQ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["TAWTHEEQCode"].ToString(), ConfigurationManager.AppSettings["TAWTHEEQ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException wex)
            {
                flag = 3;
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["TAWTHEEQCode"].ToString(), ConfigurationManager.AppSettings["TAWTHEEQ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["TAWTHEEQCode"].ToString(), ConfigurationManager.AppSettings["TAWTHEEQ"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            var jsonResult = Json(json, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return Json(jsonResult);
        }
       
    }


}