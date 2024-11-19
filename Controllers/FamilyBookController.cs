using MOCDIntegrations.Models;
using MOCDstringegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
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
    public class FamilyBookController : Controller
    {
        // GET: FamilyBook
        public ActionResult Index()
        {
            return View("FamilyBook");
        }


        private string LoadTransId()
        {
            return "MOCD_ICA_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<FamilyBookDetails.FamilyBookDetailsRequest>(postdata);
                FamilyBookDetails.FamilyBookDetailsRespose objFamilyHead = null;
                FamilyBookDetails.Dependent objDependent = null;
                List<FamilyBookDetails.Dependent> lstDependent = new List<FamilyBookDetails.Dependent>();
                FamilyBookDetails.Wives objWives = null;
                List<FamilyBookDetails.Wives> lstWives = new List<FamilyBookDetails.Wives>();
                var response = FamilybookAPICALL(input.EmiratesId);
                FamilyBookModel.Root objResponse = JsonConvert.DeserializeObject<FamilyBookModel.Root>(response.Content.ToString());
                if (objResponse.familyBookDetailsResponse != null)
                {
                    flag = 1;
                    objFamilyHead = new FamilyBookDetails.FamilyBookDetailsRespose();

                    objFamilyHead.familySequence = objResponse.familyBookDetailsResponse.familySequence;
                    objFamilyHead.childrenCount = objResponse.familyBookDetailsResponse.childrenCount.ToString();
                    objFamilyHead.wivesCount = objResponse.familyBookDetailsResponse.wivesCount.ToString();
                    objFamilyHead.city = objResponse.familyBookDetailsResponse.city.arDesc;


                    objFamilyHead.fullArabicName = objResponse.familyBookDetailsResponse.familyHead?.fullArabicName;
                    objFamilyHead.clanNameArabic = objResponse.familyBookDetailsResponse.familyHead?.clanNameArabic;
                    objFamilyHead.fullEnglishName = objResponse.familyBookDetailsResponse.familyHead?.fullEnglishName;
                    objFamilyHead.clanNameEnglish = objResponse.familyBookDetailsResponse.familyHead?.clanNameEnglish;
                    objFamilyHead.unifiedNumber = objResponse.familyBookDetailsResponse.familyHead?.unifiedNumber.ToString();
                    objFamilyHead.identityCardNumber = objResponse.familyBookDetailsResponse.familyHead?.identityCardNumber;
                    objFamilyHead.nationality = objResponse.familyBookDetailsResponse.familyHead?.nationality.arDesc;
                    objFamilyHead.gender = objResponse.familyBookDetailsResponse.familyHead.gender != null ? objResponse.familyBookDetailsResponse.familyHead.gender?.arDesc : string.Empty;
                    objFamilyHead.dateOfBirth = objResponse.familyBookDetailsResponse.familyHead?.dateOfBirth.ToShortDateString();
                    objFamilyHead.countryOfBirth = objResponse.familyBookDetailsResponse.familyHead.countryOfBirth != null ? objResponse.familyBookDetailsResponse.familyHead.countryOfBirth?.arDesc : string.Empty;
                    objFamilyHead.placeOfBirthAr = objResponse.familyBookDetailsResponse.familyHead?.placeOfBirthAr;
                    objFamilyHead.placeOfBirthEn = objResponse.familyBookDetailsResponse.familyHead?.placeOfBirthEn;
                    objFamilyHead.maritalStatus = objResponse.familyBookDetailsResponse.familyHead.maritalStatus?.arDesc;
                    objFamilyHead.religion = objResponse.familyBookDetailsResponse.familyHead.religion?.arDesc;
                    objFamilyHead.motherNameArabic = objResponse.familyBookDetailsResponse.familyHead?.motherNameArabic;
                    objFamilyHead.motherNameEnglish = objResponse.familyBookDetailsResponse.familyHead?.motherNameEnglish;

                    if (objResponse.familyBookDetailsResponse.dependents != null && objResponse.familyBookDetailsResponse.dependents.dependent.Count > 0)
                    {
                        foreach (var dt in objResponse.familyBookDetailsResponse.dependents.dependent)
                        {
                            objDependent = new FamilyBookDetails.Dependent();
                            objDependent.identityCardNumber = dt.identityCardNumber;
                            objDependent.unifiedNumber = dt.unifiedNumber != null ? dt.unifiedNumber.ToString() : "0";
                            objDependent.fullArabicName = dt.fullArabicName;
                            objDependent.clanNameArabic = dt.clanNameArabic;
                            objDependent.gender = dt.gender?.arDesc;
                            objDependent.maritalStatus = dt.maritalStatus?.arDesc;
                            objDependent.motherNameArabic = dt.motherNameArabic;
                            objDependent.dateOfBirth = dt.dateOfBirth != null ? dt.dateOfBirth.ToString() : string.Empty;
                            objDependent.relationshipToFamily = dt.relationshipToFamily?.arDesc;
                            objDependent.motherIdentityCardNumber = dt.motherIdentityCardNumber;
                            lstDependent.Add(objDependent);
                        }

                        objFamilyHead.lstDependent = lstDependent;
                    }

                    if (objResponse.familyBookDetailsResponse.wives != null && objResponse.familyBookDetailsResponse.wives.wife.Count > 0)
                    {
                        foreach (var dt in objResponse.familyBookDetailsResponse.wives.wife)
                        {
                            objWives = new FamilyBookDetails.Wives();
                            objWives.identityCardNumber = dt.identityCardNumber;
                            objWives.unifiedNumber = dt.unifiedNumber != null ? dt.unifiedNumber.ToString() : "0";
                            objWives.fullArabicName = dt.fullArabicName;
                            objWives.clanNameArabic = dt.clanNameArabic;
                            objWives.gender = dt.gender?.arDesc;
                            objWives.maritalStatus = dt.maritalStatus?.arDesc;
                            objWives.motherNameArabic = dt.motherNameArabic;
                            objWives.dateOfBirth = dt.dateOfBirth != null ? dt.dateOfBirth.ToString() : string.Empty;
                            objWives.marriageDateSpecified = dt.marriageDateSpecified ? dt.marriageDate.ToShortDateString() : string.Empty;
                            objWives.birthCity = dt.cityOfBirth != null ? dt.cityOfBirth.arDesc : string.Empty;

                            lstWives.Add(objWives);

                        }
                        objFamilyHead.lstWives = lstWives;
                    }
                    #region Adding Family Details

                    if (objFamilyHead.lstDependent != null && objFamilyHead.lstDependent.Count() > 0)
                    {
                        for (int i = 0; i < objFamilyHead.lstDependent.Count(); i++)
                        {
                            try
                            {
                                var responses = FamilybookAPICALL(objFamilyHead.lstDependent[i].identityCardNumber);
                                FamilyBookModel.Root objResponseDep = JsonConvert.DeserializeObject<FamilyBookModel.Root>(responses.Content.ToString());

                                if (objResponseDep.familyBookDetailsResponse != null)
                                {

                                    lstDependent[i].familySequence = objResponseDep.familyBookDetailsResponse.familySequence;
                                    lstDependent[i].city = objResponseDep.familyBookDetailsResponse.city?.arDesc;
                                }

                            }
                            catch (TimeoutException e)
                            {
                                continue;
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
                                                    str += "Contact GSB Support.";
                                                }
                                            }
                                            if (child.Name == "Detail")
                                            {
                                                ResponseDescription = child.InnerXml;
                                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ResponseDescription = fault.Reason.ToString();
                                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                }
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                            System.Threading.Thread.Sleep(1000);
                        }
                    }


                    if (objFamilyHead.lstWives != null && objFamilyHead.lstWives.Count() > 0)
                    {

                        for (int i = 0; i < objFamilyHead.lstWives.Count(); i++)
                        {
                            try
                            {
                                var responsesWife = FamilybookAPICALL(objFamilyHead.lstWives[i].identityCardNumber);
                                FamilyBookModel.Root objResponseWife = JsonConvert.DeserializeObject<FamilyBookModel.Root>(responsesWife.Content.ToString());

                                if (objResponseWife.familyBookDetailsResponse != null)
                                {

                                    objFamilyHead.lstWives[i].familySequence = objResponseWife.familyBookDetailsResponse.familySequence;
                                    objFamilyHead.lstWives[i].city = objResponse.familyBookDetailsResponse.city?.arDesc;

                                }


                            }
                            catch (TimeoutException e)
                            {
                                continue;
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
                                                    str += "Contact GSB Support.";
                                                }
                                            }
                                            if (child.Name == "Detail")
                                            {
                                                ResponseDescription = child.InnerXml;
                                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ResponseDescription = fault.Reason.ToString();
                                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                }
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                            System.Threading.Thread.Sleep(1000);
                        }
                    }


                    #endregion
                    flag = 1;

                    json = JsonConvert.SerializeObject(new { objFamilyHead, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<FamilyBookDetails.FamilyBookDetailsRespose>(objFamilyHead), ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search1(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<FamilyBookDetails.FamilyBookDetailsRequest>(postdata);
                FamilyBookDetails.FamilyBookDetailsRespose objFamilyHead = null;
                FamilyBookDetails.Dependent objDependent = null;
                List<FamilyBookDetails.Dependent> lstDependent = new List<FamilyBookDetails.Dependent>();
                FamilyBookDetails.Wives objWives = null;
                List<FamilyBookDetails.Wives> lstWives = new List<FamilyBookDetails.Wives>();
                FamilybookAPICALL(input.EmiratesId);

                FamilyBook.getFamilyBookDetails_pttClient client = new FamilyBook.getFamilyBookDetails_pttClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["FAMILY_BOOK_API_KEY"].ToString();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                     Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["FAMILY_BOOK_username"].ToString() + ":" + ConfigurationManager.AppSettings["FAMILY_BOOK_password"].ToString()));
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;


                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    FamilyBook.TrnHeaderType objHeader = new FamilyBook.TrnHeaderType();
                    objHeader.transactionId = LoadTransId();
                    objHeader.serviceProviderEntity = "MOCD";

                    FamilyBook.ItemsChoiceType[] objItemsChoiceType = new FamilyBook.ItemsChoiceType[1];
                    objItemsChoiceType[0] = FamilyBook.ItemsChoiceType.identityCardNumber;

                    object[] obj = new object[1];
                    obj[0] = input.EmiratesId;


                    FamilyBook.FamilyBookRequestType objid = new FamilyBook.FamilyBookRequestType();
                    objid.ItemsElementName = objItemsChoiceType;
                    objid.Items = obj;

                    FamilyBook.FamilyBookDetailsResponseType objResponse = client.getFamilyBookDetails(ref objHeader, objid);

                    if (objResponse != null)
                    {
                        flag = 1;
                        objFamilyHead = new FamilyBookDetails.FamilyBookDetailsRespose();

                        objFamilyHead.familySequence = objResponse.familySequence;
                        objFamilyHead.childrenCount = objResponse.childrenCount.ToString();
                        objFamilyHead.wivesCount = objResponse.wivesCount.ToString();
                        objFamilyHead.city = objResponse.city.arDesc;


                        objFamilyHead.fullArabicName = objResponse.familyHead?.fullArabicName;
                        objFamilyHead.clanNameArabic = objResponse.familyHead?.clanNameArabic;
                        objFamilyHead.fullEnglishName = objResponse.familyHead?.fullEnglishName;
                        objFamilyHead.clanNameEnglish = objResponse.familyHead?.clanNameEnglish;
                        objFamilyHead.unifiedNumber = objResponse.familyHead?.unifiedNumber.ToString();
                        objFamilyHead.identityCardNumber = objResponse.familyHead?.identityCardNumber;
                        objFamilyHead.nationality = objResponse.familyHead?.nationality.arDesc;
                        objFamilyHead.gender = objResponse.familyHead.gender != null ? objResponse.familyHead.gender?.arDesc : string.Empty;
                        objFamilyHead.dateOfBirth = objResponse.familyHead?.dateOfBirth.ToShortDateString();
                        objFamilyHead.countryOfBirth = objResponse.familyHead.countryOfBirth != null ? objResponse.familyHead.countryOfBirth?.arDesc : string.Empty;
                        objFamilyHead.placeOfBirthAr = objResponse.familyHead?.placeOfBirthAr;
                        objFamilyHead.placeOfBirthEn = objResponse.familyHead?.placeOfBirthEn;
                        objFamilyHead.maritalStatus = objResponse.familyHead.maritalStatus?.arDesc;
                        objFamilyHead.religion = objResponse.familyHead.religion?.arDesc;
                        objFamilyHead.motherNameArabic = objResponse.familyHead?.motherNameArabic;
                        objFamilyHead.motherNameEnglish = objResponse.familyHead?.motherNameEnglish;

                        if (objResponse.dependents != null && objResponse.dependents.Length > 0)
                        {
                            foreach (FamilyBook.DependentType dt in objResponse.dependents)
                            {
                                objDependent = new FamilyBookDetails.Dependent();
                                objDependent.identityCardNumber = dt.identityCardNumber;
                                objDependent.unifiedNumber = dt.unifiedNumber != null ? dt.unifiedNumber.ToString() : "0";
                                objDependent.fullArabicName = dt.fullArabicName;
                                objDependent.clanNameArabic = dt.clanNameArabic;
                                objDependent.gender = dt.gender?.arDesc;
                                objDependent.maritalStatus = dt.maritalStatus?.arDesc;
                                objDependent.motherNameArabic = dt.motherNameArabic;
                                objDependent.dateOfBirth = dt.dateOfBirth != null ? dt.dateOfBirth.ToString() : string.Empty;
                                objDependent.relationshipToFamily = dt.relationshipToFamily?.arDesc;
                                objDependent.motherIdentityCardNumber = dt.motherIdentityCardNumber;
                                lstDependent.Add(objDependent);
                            }

                            objFamilyHead.lstDependent = lstDependent;
                        }

                        if (objResponse.wives != null && objResponse.wives.Length > 0)
                        {
                            foreach (FamilyBook.WifeType dt in objResponse.wives)
                            {
                                objWives = new FamilyBookDetails.Wives();
                                objWives.identityCardNumber = dt.identityCardNumber;
                                objWives.unifiedNumber = dt.unifiedNumber != null ? dt.unifiedNumber.ToString() : "0";
                                objWives.fullArabicName = dt.fullArabicName;
                                objWives.clanNameArabic = dt.clanNameArabic;
                                objWives.gender = dt.gender?.arDesc;
                                objWives.maritalStatus = dt.maritalStatus?.arDesc;
                                objWives.motherNameArabic = dt.motherNameArabic;
                                objWives.dateOfBirth = dt.dateOfBirth != null ? dt.dateOfBirth.ToString() : string.Empty;
                                objWives.marriageDateSpecified = dt.marriageDateSpecified ? dt.marriageDate.ToShortDateString() : string.Empty;
                                objWives.birthCity = dt.cityOfBirth != null ? dt.cityOfBirth.arDesc : string.Empty;

                                lstWives.Add(objWives);

                            }
                            objFamilyHead.lstWives = lstWives;
                        }

                        json = JsonConvert.SerializeObject(new { objFamilyHead, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<FamilyBookDetails.FamilyBookDetailsRespose>(objFamilyHead), ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }

                #region "Add Family Sequence Number"

                // Get Family Sequence Number - 08-Mar-2023 @kidar suggest to get the sequence number for all dependent and wivies -- changes by ATEEQ
                // Get Family Sequence number for dependent
                if (objFamilyHead.lstDependent != null && objFamilyHead.lstDependent.Count() > 0)
                {
                    for (int i = 0; i < objFamilyHead.lstDependent.Count(); i++)
                    {
                        try
                        {


                            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                            {
                                var httpRequestProperty = new HttpRequestMessageProperty();
                                httpRequestProperty.Headers["GSB-APIKey"] = "4547a030-4bd5-11e9-9d74-ccf938d4a5ee".ToString();
                                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                                 Convert.ToBase64String(Encoding.ASCII.GetBytes("MOCDprdconsumer1" + ":" + "mocd@conuser$17"));
                                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;


                                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                                FamilyBook.TrnHeaderType objHeader = new FamilyBook.TrnHeaderType();
                                objHeader.transactionId = LoadTransId();
                                objHeader.serviceProviderEntity = "MOCD";

                                FamilyBook.ItemsChoiceType[] objItemsChoiceType = new FamilyBook.ItemsChoiceType[1];
                                objItemsChoiceType[0] = FamilyBook.ItemsChoiceType.identityCardNumber;

                                object[] obj = new object[1];
                                obj[0] = objFamilyHead.lstDependent[i].identityCardNumber;


                                FamilyBook.FamilyBookRequestType objid = new FamilyBook.FamilyBookRequestType();
                                objid.ItemsElementName = objItemsChoiceType;
                                objid.Items = obj;

                                FamilyBook.FamilyBookDetailsResponseType objResponse = client.getFamilyBookDetails(ref objHeader, objid);

                                if (objResponse != null)
                                {

                                    lstDependent[i].familySequence = objResponse.familySequence;
                                    lstDependent[i].city = objResponse.city?.arDesc;

                                }

                            }
                        }
                        catch (TimeoutException e)
                        {
                            continue;
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
                                                str += "Contact GSB Support.";
                                            }
                                        }
                                        if (child.Name == "Detail")
                                        {
                                            ResponseDescription = child.InnerXml;
                                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ResponseDescription = fault.Reason.ToString();
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                // Get Family Sequence number for wives
                if (objFamilyHead.lstWives != null && objFamilyHead.lstWives.Count() > 0)
                {

                    for (int i = 0; i < objFamilyHead.lstWives.Count(); i++)
                    {
                        try
                        {
                            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                            {
                                var httpRequestProperty = new HttpRequestMessageProperty();
                                httpRequestProperty.Headers["GSB-APIKey"] = "4547a030-4bd5-11e9-9d74-ccf938d4a5ee".ToString();
                                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                                 Convert.ToBase64String(Encoding.ASCII.GetBytes("MOCDprdconsumer1" + ":" + "mocd@conuser$17"));
                                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;



                                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                                FamilyBook.TrnHeaderType objHeader = new FamilyBook.TrnHeaderType();
                                objHeader.transactionId = LoadTransId();
                                objHeader.serviceProviderEntity = "MOCD";

                                FamilyBook.ItemsChoiceType[] objItemsChoiceType = new FamilyBook.ItemsChoiceType[1];
                                objItemsChoiceType[0] = FamilyBook.ItemsChoiceType.identityCardNumber;

                                object[] obj = new object[1];
                                obj[0] = objFamilyHead.lstWives[i].identityCardNumber;


                                FamilyBook.FamilyBookRequestType objid = new FamilyBook.FamilyBookRequestType();
                                objid.ItemsElementName = objItemsChoiceType;
                                objid.Items = obj;

                                FamilyBook.FamilyBookDetailsResponseType objResponse = client.getFamilyBookDetails(ref objHeader, objid);

                                if (objResponse != null)
                                {

                                    objFamilyHead.lstWives[i].familySequence = objResponse.familySequence;
                                    objFamilyHead.lstWives[i].city = objResponse.city?.arDesc;

                                }

                            }
                        }
                        catch (TimeoutException e)
                        {
                            continue;
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
                                                str += "Contact GSB Support.";
                                            }
                                        }
                                        if (child.Name == "Detail")
                                        {
                                            ResponseDescription = child.InnerXml;
                                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ResponseDescription = fault.Reason.ToString();
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                flag = 1;
                json = JsonConvert.SerializeObject(new { objFamilyHead, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                #endregion
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
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ICAFBCode"].ToString(), ConfigurationManager.AppSettings["ICAFB"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static RestResponse FamilybookAPICALL(string emirateID)
        {

            string apiURL = ConfigurationManager.AppSettings["FBURL"].ToString();

            var options = new RestClientOptions(apiURL)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("MOCD-APIKey", ConfigurationManager.AppSettings["FBAPIKey"].ToString());
            request.AddHeader("Authorization", "Bearer " + GenerateToken());
            var inquiryJson = new
            {

                identityCardNumber = emirateID

            };
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(inquiryJson);


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }
        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateTokenWebMethods(ConfigurationManager.AppSettings["ICATokenUri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["FBClientId"].ToString(), ConfigurationManager.AppSettings["FBClientSecret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
