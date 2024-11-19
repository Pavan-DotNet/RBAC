using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using static MOCDIntegrations.Models.ADDEDDetails.TradeLicenseAllData;
using static MOCDIntegrations.Models.ADDEDDetails;
using System.Linq;
using System.Runtime.Remoting;
using static MOCDIntegrations.Models.OwnerDetails;

namespace MOCDIntegrations.Controllers
{
    public class ADDEDController : Controller
    {
        // GET: ADDED
        public ActionResult Index()
        {
            return View("");
        }

        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            ADDEDDetails.Root objResponseJson = null;
            XmlDocument doc = null;
            TradeLicenses tradeLicenses = new TradeLicenses();
            LicenseDetailsResponse reponseObj = new LicenseDetailsResponse();
            try
            {
                var input = new JavaScriptSerializer().Deserialize<ADDEDDetails.ADDEDDetailsRequestParams>(postdata);


                string respList = CallWebService(input.EmiratesId, null, ConfigurationManager.AppSettings["ADED_Url"].ToString(), "urn:ae:gov:abudhabi:RealEstateAndHousing:HousingTypes:1", "searchAppByEmirateId");
                if (respList != "")
                {
                    DataSet DataSetList = FillDataSet_FromXML(respList, "getAttestationList");
                    if (DataSetList.Tables.Count > 0)
                    {
                        doc = new XmlDocument();
                        doc.LoadXml(respList);

                        string jsonNew = JsonConvert.SerializeXmlNode(doc);
                        if (!jsonNew.Contains("\"status\":\"0\""))
                        {
                            string isListExist = jsonNew.Substring(jsonNew.IndexOf("\"TradeLicenseModel\""), 25);
                            if (!isListExist.Contains("["))
                            {
                                jsonNew = jsonNew.Replace("\"TradeLicenseModel\":{", "\"TradeLicenseModel\":[{");
                                jsonNew = jsonNew.Replace("},\"status\"", "]},\"status\"");



                            }
                        }
                        objResponseJson = JsonConvert.DeserializeObject<ADDEDDetails.Root>(jsonNew);
                        if (objResponseJson?.soapEnvelope?.soapBody?.GetTradeLicensePartnersByEIDResponse?.GetTradeLicensePartnersByEIDResult?.status == "1" && objResponseJson?.soapEnvelope?.soapBody?.GetTradeLicensePartnersByEIDResponse?.GetTradeLicensePartnersByEIDResult?.TradeLicenses != null)
                        {
                            tradeLicenses.TradeLicenseModel = new List<TradeLicenseModel>();


                            reponseObj.PartnerDto = new List<PartnerDto>();
                            reponseObj.MasterDto = new List<MasterDto>();
                            reponseObj.Activity = new List<Activity>();
                            foreach (var item in objResponseJson?.soapEnvelope?.soapBody?.GetTradeLicensePartnersByEIDResponse?.GetTradeLicensePartnersByEIDResult?.TradeLicenses?.TradeLicenseModel)
                            {
                                tradeLicenses.TradeLicenseModel.Add(item);

                                var objResp = SaveTradeLicenseAllDetails(input.EmiratesId, item.TradeLicenseNumber);

                                foreach (var items in objResp.PartnerDto)
                                {
                                    items.TradeLicenseNumber = item.TradeLicenseNumber;
                                    reponseObj.PartnerDto.Add(items);
                                }
                                foreach (var item2 in objResp.Activity)
                                {
                                    item2.TradeLicenseNumber = item.TradeLicenseNumber;

                                    reponseObj.Activity.Add(item2);

                                }
                                foreach (var item3 in objResp.MasterDto)
                                {
                                    reponseObj.MasterDto.Add(item3);

                                }


                            }
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { reponseObj.PartnerDto, reponseObj.MasterDto, reponseObj.Activity, tradeLicenses, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["ADDEDCode"].ToString(), ConfigurationManager.AppSettings["ADDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {

                            flag = 2;
                            string ResponseDescription = "No Matching Records Found";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADDEDCode"].ToString(), ConfigurationManager.AppSettings["ADDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }

                    }
                    else
                    {

                        flag = 2;
                        string ResponseDescription = "No Matching Records Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADDEDCode"].ToString(), ConfigurationManager.AppSettings["ADDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADDEDCode"].ToString(), ConfigurationManager.AppSettings["ADDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException ex)
            {
                flag = 3;

                var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = resp.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADDEDCode"].ToString(), ConfigurationManager.AppSettings["ADDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = resp.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADDEDCode"].ToString(), ConfigurationManager.AppSettings["ADDED"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private static string CallWebService(string EmiratesId, string LiscenseNumber, string url, string actionname, string servicename, string Documentnumber = default)
        {
            string soapEnvelopeXml = "";
            if (LiscenseNumber == null)
            {
                soapEnvelopeXml = CreateSoapEnvelope(EmiratesId);

            }
            else
            {
                soapEnvelopeXml = CreateSoapEnvelopeLiscenceNumber(LiscenseNumber);

            }

            HttpWebRequest webRequest = CreateWebRequest(url, actionname);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            string soapResult;
            WebResponse webResponse;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            try
            {
                using (webResponse = webRequest.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }
                    //Console.Write(soapResult);
                }
            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }

                //CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADED", "CallWebService", "" + ex.Message.ToString() + "", EmiratesId.ToString(), soapEnvelopeXml, APIResponse);
                return "";
            }

            return soapResult;
        }
        private static string CreateSoapEnvelopeLiscenceNumber(string LiscenceNumber)
        {
            string soapEnvelopeDocument = "";
            try
            {
                 soapEnvelopeDocument =
               @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:trad=""http://TradeLicenseSearchService/"">
                  <soapenv:Header>
                      <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
                         <wsse:UsernameToken wsu:Id=""UsernameToken-C48BA694ED9109BF2E16851350596331"">
                            <wsse:Username>MOCD-G2G2</wsse:Username>
                            <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">LMB_82A54L#2GM</wsse:Password>
                            <wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">VxaL+1focQ4EtJBCFKpZfA==</wsse:Nonce>
                            <wsu:Created>2023-05-26T21:04:19.563Z</wsu:Created>
                         </wsse:UsernameToken>
                      </wsse:Security>
                   </soapenv:Header>
                   <soapenv:Body>
                      <trad:GetTradeLicenseBasicDetails>
                         <!--Optional:-->
                         <trad:LicenseID>" + LiscenceNumber + @"</trad:LicenseID>
                         <!--Optional:-->
                         <trad:token>DEDPRD_23X45#5OP</trad:token>
                      </trad:GetTradeLicenseBasicDetails>
                   </soapenv:Body>
                </soapenv:Envelope>";
                return soapEnvelopeDocument;
            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }

                //CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADED", "CreateSoapEnvelope_getAttestationList", "" + ex.Message.ToString() + "", LiscenceNumber.ToString(), soapEnvelopeDocument, APIResponse);
                return "";
            }

        }


        private static string CreateSoapEnvelope(string EmiratesId)
        {
            string soapEnvelopeDocument = "";
            try
            {
                 soapEnvelopeDocument = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:trad=""http://TradeLicenseSearchService/"">
                                                           <soapenv:Header>
                                                              <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
                                                                 <wsse:UsernameToken wsu:Id=""UsernameToken-C48BA694ED9109BF2E16851350596331"">
                                                                    <wsse:Username>MOCD-G2G2</wsse:Username>
                                                                    <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">LMB_82A54L#2GM</wsse:Password>
                                                                    <wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">VxaL+1focQ4EtJBCFKpZfA==</wsse:Nonce>
                                                                    <wsu:Created>2023-05-26T21:04:19.563Z</wsu:Created>
                                                                 </wsse:UsernameToken>
                                                              </wsse:Security>
                                                           </soapenv:Header>
                                                           <soapenv:Body>
                                                              <trad:GetTradeLicensePartnersByEID>
                                                                 <!--Optional:-->
                                                                 <trad:Eid>" + EmiratesId + @"</trad:Eid>
                                                                 <!--Optional:-->
                                                                 <trad:token>DEDPRD_23X45#5OP</trad:token>
                                                              </trad:GetTradeLicensePartnersByEID>
                                                           </soapenv:Body>
                                                        </soapenv:Envelope>GetTradeLicensePartnersByEID";
                return soapEnvelopeDocument;
            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }

                //CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADED", "CreateSoapEnvelope_getAttestationList", "" + ex.Message.ToString() + "", EmiratesId.ToString(), soapEnvelopeDocument, APIResponse);
                return "";
            }

        }

        private static void InsertSoapEnvelopeIntoWebRequest(string soapEnvelopeXml, HttpWebRequest webRequest)
        {
            try
            {
                //using (Stream webStream = webRequest.GetRequestStream())
                //using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
                //{
                //    requestWriter.Write(soapEnvelopeXml);
                //}
                using (Stream webStream = webRequest.GetRequestStream())
                    try
                    {
                        using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
                        {
                            requestWriter.Write(soapEnvelopeXml);
                        }
                    }
                    catch (Exception)
                    {

                    }

            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }
                //CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADED", "InsertSoapEnvelopeIntoWebRequest", "" + ex.Message.ToString() + "", "", soapEnvelopeXml, APIResponse);
            }

        }

        private static DataSet FillDataSet_FromXML(string xmlData, string servicename)
        {
            try
            {
                StringReader theReader = new StringReader(xmlData);
                DataSet theDataSet = new DataSet();
                if (theReader != null)
                {
                    theDataSet.ReadXml(theReader);
                    return theDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADED", "FillDataSet_FromXML", "" + ex.Message.ToString() + "", "", "", "");
                return null;
            }

        }
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add(@"SOAPAction:" + action + "");
                webRequest.Headers.Add(@"GSB-APIKey:" + ConfigurationManager.AppSettings["GSP_ADDED_APIKEY"].ToString() + "");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["ADED_username"].ToString() + ":" + ConfigurationManager.AppSettings["ADED_password"].ToString()));
                webRequest.Headers["Authorization"] = "Basic " + authInfo;
                return webRequest;
            }
            catch (Exception ex)
            {
                //CommonHelper.CommonFunctions.GetInstance().ErrorLogs(null, "ADED", "CreateWebRequest", "" + ex.Message.ToString() + "", "", "", "");

                return null;
            }

        }

        private static LicenseDetailsResponse SaveTradeLicenseAllDetails(string DATA, string TradeLicenseNumber)
        {
            bool IsActiviy = false, IsParticipant = false, IsMaster = true;
            string respDataList = null, jsonAllData = null;
            XmlDocument doc = new XmlDocument();
            TradeLicenseAllData.Root objAllData = null;

            respDataList = CallWebService(DATA, TradeLicenseNumber, ConfigurationManager.AppSettings["ADDED_GetAllDataUrl"].ToString(), "urn:ae:gov:abudhabi:RealEstateAndHousing:HousingTypes:1", "searchAppByLiscenceId");
            doc.LoadXml(respDataList);
            if (respDataList != null)
            {
                jsonAllData = JsonConvert.SerializeXmlNode(doc);
                if (jsonAllData != null)
                {
                    if (jsonAllData.Contains("\"master\":{\"master\":[{"))
                    {
                        IsMaster = true;
                        jsonAllData = jsonAllData.Replace(":{\"master\":{\"master\":[{\"", ":{\"master1\":{\"master1\":[{\"");
                    }
                    else
                    {
                        IsMaster = false;
                    }
                    if (jsonAllData.Contains("\"activity\":{\"activity\":[{") && jsonAllData.Contains(":{\"partner\":{\"partner\":[{\""))
                    {
                        IsActiviy = true;
                        IsParticipant = true;
                        jsonAllData = jsonAllData.Replace(":{\"partner\":{\"partner\":[{\"", ":{\"partner1\":{\"partner1\":[{\"");
                    }
                    else if (!jsonAllData.Contains("\"activity\":{\"activity\":[{") && !jsonAllData.Contains(":{\"partner\":{\"partner\":[{\""))
                    {
                        IsActiviy = false;
                        IsParticipant = false;
                        jsonAllData = jsonAllData.Replace("\"activity\":{\"activity\"", "\"activity1\":{\"activity1\"");
                    }
                    else if (!jsonAllData.Contains("\"activity\":{\"activity\":[{") && jsonAllData.Contains(":{\"partner\":{\"partner\":[{\""))
                    {
                        IsParticipant = false;
                        IsActiviy = true;
                        jsonAllData = jsonAllData.Replace("\"activity\":{\"activity\"", "\"activity1\":{\"activity1\"");
                        jsonAllData = jsonAllData.Replace(":{\"partner\":{\"partner\":[{\"", ":{\"partner1\":{\"partner1\":[{\"");
                    }
                    else if (jsonAllData.Contains("\"activity\":{\"activity\":[{") && !jsonAllData.Contains(":{\"partner\":{\"partner\":[{\""))
                    {
                        IsParticipant = false;
                        IsActiviy = true;
                        jsonAllData = jsonAllData.Replace(":{\"partner\":{\"partner\":[{\"", ":{\"partner1\":{\"partner1\":[{\"");
                    }
                    else
                    {
                        IsParticipant = false;
                        IsActiviy = false;
                    }
                    objAllData = JsonConvert.DeserializeObject<TradeLicenseAllData.Root>(jsonAllData);

                    return SaveAllTradieData(objAllData?.soapEnvelope?.soapBody?.GetTradeLicenseBasicDetailsResponse?.GetTradeLicenseBasicDetailsResult, DATA, TradeLicenseNumber, IsActiviy, IsParticipant, IsMaster);
                }
            }

            return null;
        }
        private static LicenseDetailsResponse SaveAllTradieData(GetTradeLicenseBasicDetailsResult DRitem, string EmirateID, string LiscenceNumber, bool IsActivityList, bool IsParticipantList, bool IsMasterList)
        {
            LicenseDetailsResponse listReponse = new LicenseDetailsResponse();

            if (DRitem != null)
            {
                List<PartnerDto> partnerList = new List<PartnerDto>();
                List<MasterDto> masterList = new List<MasterDto>();
                List<Activity> list = new List<Activity>();


                if (!IsMasterList)
                {
                    DateTime.Now.ToString("yyyyMMdd-HHmmss");

                    if (DRitem.master?.master != null)
                    {
                        if (DRitem.master.master != null)
                        {
                            string EstablishmentDate = DRitem.master.master.EstablishmentDate.ToString("yyyyMMdd-HHmmss");
                            masterList.Add(
                                new MasterDto
                                {
                                    BusinessNameArb = DRitem.master.master.BusinessNameArb,
                                    BusinessLicenseBranchFlag = DRitem.master.master.BusinessLicenseBranchFlag,
                                    MotherCompany = DRitem.master.master.MotherCompany,
                                    BusinesslicenseCityArb = DRitem.master.master.BusinesslicenseCityArb,
                                    BusinesslicenseCityEng = DRitem.master.master.BusinesslicenseCityEng,
                                    EstablishmentDate = DRitem.master.master.EstablishmentDate.ToString("yyyyMMdd-HHmmss"),
                                    BusinessLicenseIssueDate = DRitem.master.master.BusinessLicenseIssueDate.ToString("yyyyMMdd-HHmmss"),
                                    BusinessLicenseExpiryDate = DRitem.master.master.BusinessLicenseExpiryDate.ToString("yyyyMMdd-HHmmss"),
                                    CancelDate = DRitem.master.master.CancelDate,
                                    BusinessLicenseAddressArb = DRitem.master.master.BusinessLicenseAddressArb,
                                    BusinessLicenseAddressEng = DRitem.master.master.BusinessLicenseAddressEng,
                                    BuildingNumber = DRitem.master.master.BuildingNumber,
                                    Building_Name = DRitem.master.master.Building_Name,
                                    CoordinatesX = DRitem.master.master.CoordinatesX,
                                    CoordinatesY = DRitem.master.master.CoordinatesY,
                                    ADCCIUnifiedID = DRitem.master.master.ADCCIUnifiedID,
                                    LicenseRemarksArabic = DRitem.master.master.LicenseRemarksArabic,
                                    BranchTypeCode = DRitem.master.master.BranchType.BranchTypeCode,
                                    BranchTypeArb = DRitem.master.master.BranchType.BranchTypeArb,
                                    BranchTypeEng = DRitem.master.master.BranchType.BranchTypeEng,
                                    LegalFormCode = DRitem.master.master.LegalForm.LegalFormCode,
                                    BusinessLegalFormArb = DRitem.master.master.LegalForm.BusinessLegalFormArb,
                                    BusinessLegalFormEng = DRitem.master.master.LegalForm.BusinessLegalFormEng,
                                    LicenseTypeCode = DRitem.master.master.LicenseType.LicenseTypeCode,
                                    BusinessLicenseTypeArb = DRitem.master.master.LicenseType.BusinessLicenseTypeArb,
                                    BusinessLicenseTypeEng = DRitem.master.master.LicenseType.BusinessLicenseTypeEng,
                                    CountryCode = DRitem.master.master.EstablishmentCountry.CountryCode,
                                    CountryNameArb = DRitem.master.master.EstablishmentCountry.CountryNameArb,
                                    CountryNameEng = DRitem.master.master.EstablishmentCountry.CountryNameEng,
                                    BusinessLicenseStatusCode = DRitem.master.master.BusinessLicenseStatus.BusinessLicenseStatusCode,

                                    BusinessLicenseStatusArb = DRitem.master.master.BusinessLicenseStatus.BusinessLicenseStatusArb,
                                    BusinessLicenseStatusEng = DRitem.master.master.BusinessLicenseStatus.BusinessLicenseStatusEng,
                                    ClasificationCode = DRitem.master.master.Clasification.ClasificationCode,
                                    ClasificationArb = DRitem.master.master.Clasification.ClasificationArb,
                                    ClasificationEng = DRitem.master.master.Clasification.ClasificationEng,
                                    IssuePlaceCode = DRitem.master.master.IssuePlace.IssuePlaceCode,
                                    IssuePlaceArb = DRitem.master.master.IssuePlace.IssuePlaceArb,
                                    IssuePlaceEng = DRitem.master.master.IssuePlace.IssuePlaceEng,
                                    OfficialMobile = DRitem.master.master.OfficialMobile,
                                    OfficialEmail = DRitem.master.master.OfficialEmail,
                                    ProName = DRitem.master.master.ProName,
                                    PROMobileNumber = DRitem.master.master.PROMobileNumber,
                                    ProEmail = DRitem.master.master.ProEmail,
                                    HasUpdatedRealBeneficiary = DRitem.master.master.HasUpdatedRealBeneficiary

                                });
                            //   var masteList = CommonHelper.Base64APIs.GetInstance().ToDataTable(masterList);
                            // command.Parameters.AddWithValue("@Master_DT", masteList);
                        }

                    }

                }
                else
                {
                    foreach (var item in DRitem.master1.master1)
                    {
                        masterList.Add(new MasterDto
                        {
                            BusinessNameArb = item.BusinessNameArb,
                            BusinessLicenseBranchFlag = item.BusinessLicenseBranchFlag,
                            MotherCompany = item.MotherCompany,
                            BusinesslicenseCityArb = item.BusinesslicenseCityArb,
                            BusinesslicenseCityEng = item.BusinesslicenseCityEng,
                            EstablishmentDate = item.EstablishmentDate.ToString("yyyyMMdd-HHmmss"),
                            BusinessLicenseIssueDate = item.BusinessLicenseIssueDate.ToString("yyyyMMdd-HHmmss"),
                            BusinessLicenseExpiryDate = item.BusinessLicenseExpiryDate.ToString("yyyyMMdd-HHmmss"),
                            CancelDate = item.CancelDate,
                            BusinessLicenseAddressArb = item.BusinessLicenseAddressArb,
                            BusinessLicenseAddressEng = item.BusinessLicenseAddressEng,
                            BuildingNumber = item.BuildingNumber,
                            Building_Name = item.Building_Name,
                            CoordinatesX = item.CoordinatesX,
                            CoordinatesY = item.CoordinatesY,
                            ADCCIUnifiedID = item.ADCCIUnifiedID,
                            LicenseRemarksArabic = item.LicenseRemarksArabic,
                            BranchTypeCode = item.BranchType.BranchTypeCode,
                            BranchTypeArb = item.BranchType.BranchTypeArb,
                            BranchTypeEng = item.BranchType.BranchTypeEng,
                            LegalFormCode = item.LegalForm.LegalFormCode,
                            BusinessLegalFormArb = item.LegalForm.BusinessLegalFormArb,
                            BusinessLegalFormEng = item.LegalForm.BusinessLegalFormEng,
                            LicenseTypeCode = item.LicenseType.LicenseTypeCode,
                            BusinessLicenseTypeArb = item.LicenseType.BusinessLicenseTypeArb,
                            BusinessLicenseTypeEng = item.LicenseType.BusinessLicenseTypeEng,
                            CountryCode = item.EstablishmentCountry.CountryCode,
                            CountryNameArb = item.EstablishmentCountry.CountryNameArb,
                            CountryNameEng = item.EstablishmentCountry.CountryNameEng,
                            BusinessLicenseStatusCode = item.BusinessLicenseStatus.BusinessLicenseStatusCode,
                            BusinessLicenseStatusArb = item.BusinessLicenseStatus.BusinessLicenseStatusArb,
                            BusinessLicenseStatusEng = item.BusinessLicenseStatus.BusinessLicenseStatusEng,
                            ClasificationCode = item.Clasification.ClasificationCode,
                            ClasificationArb = item.Clasification.ClasificationArb,
                            ClasificationEng = item.Clasification.ClasificationEng,
                            IssuePlaceCode = item.IssuePlace.IssuePlaceCode,
                            IssuePlaceArb = item.IssuePlace.IssuePlaceArb,
                            IssuePlaceEng = item.IssuePlace.IssuePlaceEng,
                            OfficialMobile = item.OfficialMobile,
                            OfficialEmail = item.OfficialEmail,
                            ProName = item.ProName,
                            PROMobileNumber = item.PROMobileNumber,
                            ProEmail = item.ProEmail,
                            HasUpdatedRealBeneficiary = item.HasUpdatedRealBeneficiary
                        });
                    }

                }
                if (IsActivityList)
                {
                    if (DRitem.activity != null && DRitem.activity?.activity != null)
                    {
                        //var activiytList = CommonHelper.Base64APIs.GetInstance().ToDataTable(DRitem.activity.activity);
                        //activiytList.Columns.Remove("activity");

                        //command.Parameters.AddWithValue("@Activity_DT", activiytList);

                        list = DRitem.activity?.activity;
                    }
                }
                else
                {
                    if (DRitem.activity1 != null && DRitem.activity1?.activity1 != null)
                    {
                        list.Add(new Activity
                        {
                            ActivityCode = DRitem.activity1.activity1.ActivityCode,
                            ActivityNameArb = DRitem.activity1.activity1.ActivityNameArb,
                            ActivityNameEng = DRitem.activity1.activity1.ActivityNameEng,

                        });
                        //var activiytList = CommonHelper.Base64APIs.GetInstance().ToDataTable(list);
                        //activiytList.Columns.Remove("activity");

                        // command.Parameters.AddWithValue("@Activity_DT", activiytList);
                    }

                }
                if (!IsParticipantList)
                {

                    if (DRitem.partner != null)
                    {


                        if (DRitem.partner?.partner != null)
                        {
                            partnerList.Add(new PartnerDto
                            {
                                CompanyLicenseNo = DRitem.partner.partner.CompanyLicenseNo,
                                PartnerNameArb = DRitem.partner.partner.PartnerNameArb,
                                PartnerNameEng = DRitem.partner.partner.PartnerNameEng,
                                PartnerSharePercentage = DRitem.partner.partner.PartnerSharePercentage,
                                Gender = DRitem.partner.partner.Gender,
                                LicenseRelationshipCode = DRitem.partner.partner.LicenseRelationship.LicenseRelationshipCode,
                                BusinessLicenseRelationshipArb = DRitem.partner.partner.LicenseRelationship.BusinessLicenseRelationshipArb,
                                BusinessLicenseRelationshipEng = DRitem.partner.partner.LicenseRelationship.BusinessLicenseRelationshipEng,
                                PartnerTypeArb = DRitem.partner.partner.PartnerType.PartnerTypeArb,
                                PartnerTypeCode = DRitem.partner.partner.PartnerType.PartnerTypeCode,
                                PartnerTypeEng = DRitem.partner.partner.PartnerType.PartnerTypeEng,
                                CountryCode = DRitem.partner.partner.PartnerNationality.CountryCode,
                                CountryNameArb = DRitem.partner.partner.PartnerNationality.CountryNameArb,
                                CountryNameEng = DRitem.partner.partner.PartnerNationality.CountryNameEng,

                            });
                            //var prtnerList = CommonHelper.Base64APIs.GetInstance().ToDataTable(partnerList);
                            // command.Parameters.AddWithValue("@Partner_DT", prtnerList);

                        }
                    }

                }
                else
                {
                    foreach (var item in DRitem.partner1.partner1)
                    {
                        if (item != null)
                        {
                            partnerList.Add(new PartnerDto
                            {
                                CompanyLicenseNo = item.CompanyLicenseNo,
                                PartnerNameArb = item.PartnerNameArb,
                                PartnerNameEng = item.PartnerNameEng,
                                PartnerSharePercentage = item.PartnerSharePercentage,
                                Gender = item.Gender,
                                LicenseRelationshipCode = item.LicenseRelationship.LicenseRelationshipCode,
                                BusinessLicenseRelationshipArb = item.LicenseRelationship.BusinessLicenseRelationshipArb,
                                BusinessLicenseRelationshipEng = item.LicenseRelationship.BusinessLicenseRelationshipEng,
                                PartnerTypeArb = item.PartnerType.PartnerTypeArb,
                                PartnerTypeCode = item.PartnerType.PartnerTypeCode,
                                PartnerTypeEng = item.PartnerType.PartnerTypeEng,
                                CountryCode = item.PartnerNationality.CountryCode,
                                CountryNameArb = item.PartnerNationality.CountryNameArb,
                                CountryNameEng = item.PartnerNationality.CountryNameEng,

                            });

                        }

                        //var prtnerList = CommonHelper.Base64APIs.GetInstance().ToDataTable(partnerList);


                        // command.Parameters.AddWithValue("@Partner_DT", prtnerList);


                    }
                }

                listReponse.PartnerDto = partnerList;
                listReponse.MasterDto = masterList;
                listReponse.Activity = list;

            }

            return listReponse;
        }
    }
}
