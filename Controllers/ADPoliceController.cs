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
    public class ADPoliceController : Controller
    {
        // GET: ADPolice
        public ActionResult Index()
        {
            return View("ADPolice");
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
                var input = new JavaScriptSerializer().Deserialize<MOHREEmployeeDetails.MOHRERequest>(postdata);
                List<ADPoliceDetails.VehicleRegistrationRequestParams> lstMOHREResponse = new List<ADPoliceDetails.VehicleRegistrationRequestParams>();
                List<ADPoliceDetails.VehicleRegistrationResponseParams> lstVehicleRegistration = new List<ADPoliceDetails.VehicleRegistrationResponseParams>();
                ADPoliceService.VehicleRegistrationPortClient client = new ADPoliceService.VehicleRegistrationPortClient();


                ADPoliceDetails.VehicleRegistrationResponseParams objVehicleRegistration = new ADPoliceDetails.VehicleRegistrationResponseParams();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    ADPoliceService.RequestTrnHeaderType objRequestTrnHeaderType = new ADPoliceService.RequestTrnHeaderType();
                    objRequestTrnHeaderType.EntityCode = "MOCD";


                    ADPoliceService.VehicleRegistrationListRequest objVehicleRegistrationListRequest = new ADPoliceService.VehicleRegistrationListRequest();
                    objVehicleRegistrationListRequest.ItemElementName = ADPoliceService.ItemChoiceType.EmiratesIDNumber;
                    objVehicleRegistrationListRequest.Item = input.EmiratesID;

                    ADPoliceService.VehicleRegistrationListType[] objVehicleRegistrationListType = null;
                    ADPoliceService.PlateColorType objPlateColorType = null;
                    ADPoliceService.PlateKindType objPlateKindType = null;
                    ADPoliceService.PlateSourceType objPlateSourceType = null;
                    ADPoliceService.VehicleRegistrationDetailsResponse objVehicleRegistrationDetailsResponse = null;
                    ADPoliceService.ItemsChoiceType[] objItemsChoiceType = null;
                    string PlateNumber = string.Empty;
                    string[] strarr = null;

                    try
                    {
                        ADPoliceService.ResponseTrnHeaderType objResponseTrnHeaderType = client.getVehicleRegistrationList(objRequestTrnHeaderType, objVehicleRegistrationListRequest, out objVehicleRegistrationListType);
                    }
                    catch { objVehicleRegistrationListType = null; }

                    if (objVehicleRegistrationListType != null && objVehicleRegistrationListType.Length > 0)
                    {
                        foreach (ADPoliceService.VehicleRegistrationListType objVehicle in objVehicleRegistrationListType)
                        {
                            objPlateColorType = new ADPoliceService.PlateColorType();
                            objPlateColorType = objVehicle.PlateColor;

                            objPlateKindType = new ADPoliceService.PlateKindType();
                            objPlateKindType = objVehicle.PlateKind;

                            objPlateSourceType = new ADPoliceService.PlateSourceType();
                            objPlateSourceType = objVehicle.PlateSource;

                            PlateNumber = objVehicle.PlateNumber;

                            objItemsChoiceType = new ADPoliceService.ItemsChoiceType[4];
                            ADPoliceService.VehicleRegistrationDetailsRequest objVehicleRegistrationDetailsRequest = new ADPoliceService.VehicleRegistrationDetailsRequest();

                            objItemsChoiceType[0] = ADPoliceService.ItemsChoiceType.PlateNumber;
                            objItemsChoiceType[1] = ADPoliceService.ItemsChoiceType.PlateColorCode;
                            objItemsChoiceType[2] = ADPoliceService.ItemsChoiceType.PlateSourceCode;
                            objItemsChoiceType[3] = ADPoliceService.ItemsChoiceType.PlateKindCode;

                            objVehicleRegistrationDetailsRequest.ItemsElementName = objItemsChoiceType;

                            strarr = new string[4];
                            strarr[0] = PlateNumber;
                            strarr[1] = objPlateColorType.PlateColorCode;
                            strarr[2] = objPlateSourceType.PlateSourceCode;
                            strarr[3] = objPlateKindType.PlateKindCode;
                            objVehicleRegistrationDetailsRequest.Items = strarr;

                            ADPoliceService.ResponseTrnHeaderType objResponseTrnHeaderTypeDetails = client.getVehicleRegistration(objRequestTrnHeaderType, objVehicleRegistrationDetailsRequest, out objVehicleRegistrationDetailsResponse);

                            if (objVehicleRegistrationDetailsResponse != null && objVehicleRegistrationDetailsResponse.Contents != null)
                            {
                                ADPoliceService.VehicleRegistrationDetailsType objVehicleRegistrationDetailsType = objVehicleRegistrationDetailsResponse.Contents;


                                objVehicleRegistration.EmiratesIDNumber = objVehicleRegistrationDetailsType.EmiratesIDNumber;
                                objVehicleRegistration.MOIUnifiedNumber = objVehicleRegistrationDetailsType.MOIUnifiedNumber;
                                objVehicleRegistration.OwnerCode = objVehicleRegistrationDetailsType.OwnerCode;
                                objVehicleRegistration.OwnerArabicName = objVehicleRegistrationDetailsType.OwnerArabicName;
                                objVehicleRegistration.OwnerEnglishName = objVehicleRegistrationDetailsType.OwnerEnglishName;
                                objVehicleRegistration.OwnerNationality = objVehicleRegistrationDetailsType.OwnerNationality.OwnerNationalityArabicDescription;
                                objVehicleRegistration.PlateNumber = objVehicleRegistrationDetailsType.PlateNumber;
                                objVehicleRegistration.PlateColor = objVehicleRegistrationDetailsType.PlateColor.PlateColorArabicDescription;
                                objVehicleRegistration.PlateKind = objVehicleRegistrationDetailsType.PlateKind.PlateKindArabicDescription;
                                objVehicleRegistration.PlateType = objVehicleRegistrationDetailsType.PlateType.PlateTypeArabicDescription;
                                objVehicleRegistration.PlateSource = objVehicleRegistrationDetailsType.PlateSource.PlateSourceArabicDescription;
                                objVehicleRegistration.InsuranceCompany = objVehicleRegistrationDetailsType.InsuranceCompany;
                                objVehicleRegistration.InsuranceType = objVehicleRegistrationDetailsType.InsuranceType.InsuranceTypeArabicDescription;
                                objVehicleRegistration.InsuranceExpiryDate = objVehicleRegistrationDetailsType.InsuranceExpiryDate;
                                objVehicleRegistration.PolicyNumber = objVehicleRegistrationDetailsType.PolicyNumber;
                                objVehicleRegistration.MortgageSource = objVehicleRegistrationDetailsType.MortgageSource;
                                objVehicleRegistration.MortgageReferences = objVehicleRegistrationDetailsType.MortgageReferences;
                                objVehicleRegistration.ChassisNumber = objVehicleRegistrationDetailsType.ChassisNumber;
                                objVehicleRegistration.EngineNumber = objVehicleRegistrationDetailsType.EngineNumber;
                                objVehicleRegistration.VehicleLicenseEndDate = objVehicleRegistrationDetailsType.VehicleLicenseEndDate;
                                objVehicleRegistration.DateOfRegister = objVehicleRegistrationDetailsType.DateOfRegister;
                                objVehicleRegistration.FullWeight = objVehicleRegistrationDetailsType.FullWeight;
                                objVehicleRegistration.EmptyWeight = objVehicleRegistrationDetailsType.EmptyWeight;
                                objVehicleRegistration.VehicleType = objVehicleRegistrationDetailsType.VehicleType.VehicleTypeArabicDescription;
                                objVehicleRegistration.VehicleModel = objVehicleRegistrationDetailsType.VehicleModel.VehicleModelArabicDescription;
                                objVehicleRegistration.ChairNumber = objVehicleRegistrationDetailsType.ChairNumber;
                                objVehicleRegistration.ManufacturingYear = objVehicleRegistrationDetailsType.ManufacturingYear;
                                objVehicleRegistration.ManufacturingCountry = objVehicleRegistrationDetailsType.ManufacturingCountry.ManufacturingCountryArabicDescription;
                                objVehicleRegistration.VehicleClass = objVehicleRegistrationDetailsType.VehicleClass.VehicleClassArabicDescription;
                                objVehicleRegistration.VehicleColor = objVehicleRegistrationDetailsType.VehicleColor.VehicleColorArabicDescription;
                                objVehicleRegistration.GearType = objVehicleRegistrationDetailsType.GearType.GearTypeArabicDescription;
                                objVehicleRegistration.FuelType = objVehicleRegistrationDetailsType.FuelType.FuelTypeArabicDescription;
                                objVehicleRegistration.WeightType = objVehicleRegistrationDetailsType.WeightType.WeightTypeArabicDescription;
                                objVehicleRegistration.SteeringType = objVehicleRegistrationDetailsType.SteeringType.SteeringTypeArabicDescription;
                                objVehicleRegistration.CylindersCount = objVehicleRegistrationDetailsType.CylindersCount;
                                objVehicleRegistration.AxisCount = objVehicleRegistrationDetailsType.AxisCount;
                                objVehicleRegistration.DoorsCount = objVehicleRegistrationDetailsType.DoorsCount;
                                objVehicleRegistration.WheelsCount = objVehicleRegistrationDetailsType.WheelsCount;
                                lstVehicleRegistration.Add(objVehicleRegistration);
                            }
                        }
                    }

                    if (lstVehicleRegistration != null && lstVehicleRegistration.Count > 0)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstVehicleRegistration, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<ADPoliceDetails.VehicleRegistrationResponseParams>>(lstVehicleRegistration), ConfigurationManager.AppSettings["ADPOLICECode"].ToString(), ConfigurationManager.AppSettings["ADPOLICE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADPOLICECode"].ToString(), ConfigurationManager.AppSettings["ADPOLICE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADPOLICECode"].ToString(), ConfigurationManager.AppSettings["ADPOLICE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
            }
            catch (WebException wex)
            {
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                flag = 3;
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new
                {
                    ResponseDescription,
                    flag
                }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADPOLICECode"].ToString(), ConfigurationManager.AppSettings["ADPOLICE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADPOLICECode"].ToString(), ConfigurationManager.AppSettings["ADPOLICE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}