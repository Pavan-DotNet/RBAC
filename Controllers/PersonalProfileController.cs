//using ae.gov.id.crypto;
using Microsoft.Ajax.Utilities;
using MOCDIntegrations.Models;
using MOCDIntegrations.PersonalProfile;
using MOCDstringegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace MOCDIntegrations.Controllers
{
    public class PersonalProfileController : Controller
    {
        // GET: PersonalProfile
        public ActionResult Index()
        {
            return View("PersonalProfile");
        }

        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateTokenWebMethods(ConfigurationManager.AppSettings["ICATokenUri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["ICAClientId"].ToString(), ConfigurationManager.AppSettings["ICAClientSecret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
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
            List<PPDetails> lstResponse = new List<PPDetails>();
            PPDetails person = new PPDetails();

            try
            {
                var input = new JavaScriptSerializer().Deserialize<PersonalProfileDetails.PersonalProfileDetailsRequest>(postdata);
                var response = ICPAPICALL(input.EmiratesId);
                //List<PersonalProfileDetails.PersonalProfileDetailsResponse> lstResponse = new List<PersonalProfileDetails.PersonalProfileDetailsResponse>();
                //PersonalProfileDetails.PersonalProfileDetailsResponse objPPDetails = null;

                PersonalProfileDetailsModel.Root objPPDetails = JsonConvert.DeserializeObject<PersonalProfileDetailsModel.Root>(response.Content.ToString());
                var responseIca = objPPDetails.personInquiryResponse.Body.PersonProfile;

                var result = string.Empty;

                if (responseIca != null)
                {
                    if (responseIca.IdentityCard != null)
                    {
                        person.identityCardnumber = responseIca.IdentityCard.IDN;
                        person.identityBacknumber = responseIca.IdentityCard.IDNBackNumber;
                        person.identityCardissuDate = responseIca.IdentityCard.IssueDate;
                        person.identityCardexpiryDate = responseIca.IdentityCard.ExpiryDate;
                    }
                    if (responseIca.Passport != null)
                    {
                        if (responseIca.Passport.IssueCountry != null)
                        {
                            person.passportissueCountryid = responseIca.Passport.IssueCountry.Id;
                            person.passportissueCountryarDesc = responseIca.Passport.IssueCountry.DescriptionArabic;
                            person.passportissueCountryenDesc = responseIca.Passport.IssueCountry.DescriptionEnglish;
                        }
                        if (responseIca.Passport.PassportType != null)
                        {
                            person.passporttypeid = responseIca.Passport.PassportType.Id;
                            person.passporttypearDesc = responseIca.Passport.PassportType.DescriptionArabic;
                            person.passporttypeenDesc = responseIca.Passport.PassportType.DescriptionEnglish;
                        }

                        person.passportnumber = responseIca.Passport.PassportNo;
                        person.passportissuDate = responseIca.Passport.IssueDate;
                        person.passportexpiryDate = responseIca.Passport.ExpiryDate;
                        person.passportissuePlace = responseIca.Passport.IssuePlace;
                    }

                    if (responseIca.Addresses != null && responseIca.Addresses.Address.Count > 0)
                    {
                        if (responseIca.Addresses.Address[0].LocalAddress != null)
                        {
                            person.pobox = responseIca.Addresses.Address[0].LocalAddress.PoboxNo;
                            person.mobileNumber = responseIca.Addresses.Address[0].LocalAddress.MobileNo;
                            person.homePhone = responseIca.Addresses.Address[0].LocalAddress.HomePhone;
                            person.workPhone = responseIca.Addresses.Address[0].LocalAddress.WorkPhone;
                            person.Email = responseIca.Addresses.Address[0].LocalAddress.EmailAddress;
                            if (responseIca.Addresses.Address[0].LocalAddress.City != null)
                            {
                                person.localAddresscityid = responseIca.Addresses.Address[0].LocalAddress.City.Id;
                                person.localAddresscityarDesc = responseIca.Addresses.Address[0].LocalAddress.City.DescriptionArabic;
                                person.localAddresscityenDesc = responseIca.Addresses.Address[0].LocalAddress.City.DescriptionEnglish;
                            }
                            if (responseIca.Addresses.Address[0].LocalAddress.Area != null)
                            {
                                person.localAddressareaid = responseIca.Addresses.Address[0].LocalAddress.Area.Id;
                                person.localAddressareaarDesc = responseIca.Addresses.Address[0].LocalAddress.Area.DescriptionArabic;
                                person.localAddressareaenDesc = responseIca.Addresses.Address[0].LocalAddress.Area.DescriptionEnglish;
                            }
                            if (responseIca.Addresses.Address[0].LocalAddress.Street != null)
                            {
                                person.localAddressstreetid = responseIca.Addresses.Address[0].LocalAddress.Street.Id;
                                person.localAddressstreetarDesc = responseIca.Addresses.Address[0].LocalAddress.Street.DescriptionArabic;
                                person.localAddressstreetenDesc = responseIca.Addresses.Address[0].LocalAddress.Street.DescriptionEnglish;

                            }
                            if (responseIca.Addresses.Address[0].LocalAddress.Emirate != null)
                            {
                                person.localAddressemirateid = responseIca.Addresses.Address[0].LocalAddress.Emirate.Id;
                                person.localAddressemiratearDesc = responseIca.Addresses.Address[0].LocalAddress.Emirate.DescriptionArabic;
                                person.localAddressemirateenDesc = responseIca.Addresses.Address[0].LocalAddress.Emirate.DescriptionEnglish;
                            }
                        }
                    }

                    if (responseIca.PersonName != null)
                    {
                        person.firstNameArabic = responseIca.PersonName.FirstNameArabic;
                        person.firstNameEnglish = responseIca.PersonName.FirstNameEnglish;
                        person.secondNameArabic = responseIca.PersonName.SecondNameArabic;
                        person.secondNameEnglish = responseIca.PersonName.SecondNameEnglish;
                        person.thirdNameArabic = responseIca.PersonName.ThirdNameArabic;
                        person.thirdNameEnglish = responseIca.PersonName.ThirdNameEnglish;
                        person.fourthNameArabic = responseIca.PersonName.FourthNameArabic;
                        person.fourthNameEnglish = responseIca.PersonName.FourthNameEnglish;
                        person.fullArabicName = responseIca.PersonName.FullNameArabic;
                        person.fullEnglishName = responseIca.PersonName.FullNameEnglish;
                        if (responseIca.PersonName.Tribe != null)
                        {

                            person.tribeid = responseIca.PersonName.Tribe.Id;
                            person.tribearDesc = responseIca.PersonName.Tribe.DescriptionArabic;
                            person.tribeenDesc = responseIca.PersonName.Tribe.DescriptionEnglish;
                        }
                    }



                    person.unifiedNumber = responseIca.UN;
                    person.khulasitQaidNumber = responseIca.khulasitQaidNo;
                    person.dateOfBirth = responseIca.BirthDate;
                    person.placeOfBirthAr = responseIca.BirthPlaceArabic;
                    person.placeOfBirthEn = responseIca.BirthPlaceEnglish;
                    person.familyCount = responseIca.FamilyCount;
                    person.familyMaleCount = responseIca.FamilyMaleCount;
                    person.familyFemaleCount = responseIca.FamilyFemaleCount;
                    person.familyBookNumber = responseIca.FamilyBookNumber;

                    if (responseIca.Nationality != null)
                    {
                        person.nationalityid = responseIca.Nationality.Id;
                        person.nationalityarDesc = responseIca.Nationality.DescriptionArabic;
                        person.nationalityenDesc = responseIca.Nationality.DescriptionEnglish;
                    }
                    if (responseIca.Qualification != null)
                    {
                        if (responseIca.Qualification.Specialization != null)
                        {
                            person.specialization = responseIca.Qualification.Specialization.Id;
                            person.specializationarDesc = responseIca.Qualification.Specialization.DescriptionArabic;
                            person.specializationenDesc = responseIca.Qualification.Specialization.DescriptionEnglish;
                        }
                    }
                    if (responseIca.Occupation != null)
                    {
                        person.occupationID = responseIca.Occupation.Id;
                        person.occupationArDesc = responseIca.Occupation.DescriptionArabic;
                        person.occupationEnDesc = responseIca.Occupation.DescriptionEnglish;
                    }
                    if (responseIca.Gender != null)
                    {
                        person.genderid = responseIca.Gender.Id;
                        person.genderarDesc = responseIca.Gender.DescriptionArabic;
                        person.genderenDesc = responseIca.Gender.DescriptionEnglish;
                    }
                    if (responseIca.BirthCountry != null)
                    {
                        person.countryOfBirthid = responseIca.BirthCountry.Id;
                        person.countryOfBirtharDesc = responseIca.BirthCountry.DescriptionArabic;
                        person.countryOfBirthenDesc = responseIca.BirthCountry.DescriptionEnglish;
                    }
                    if (responseIca.BirthEmirate != null)
                    {
                        person.emirateOfBirthid = responseIca.BirthEmirate.Id;
                        person.emirateOfBirtharDesc = responseIca.BirthEmirate.DescriptionArabic;
                        person.emirateOfBirthenDesc = responseIca.BirthEmirate.DescriptionEnglish;
                    }
                    if (responseIca.MaritalStatus != null)
                    {
                        person.maritalStatusid = responseIca.MaritalStatus.Id;
                        person.maritalStatusarDesc = responseIca.MaritalStatus.DescriptionArabic;
                        person.maritalStatusenDesc = responseIca.MaritalStatus.DescriptionEnglish;
                    }
                    if (responseIca.Religion != null)
                    {
                        person.religionid = responseIca.Religion.Id;
                        person.religionarDesc = responseIca.Religion.DescriptionArabic;
                        person.religionenDesc = responseIca.Religion.DescriptionEnglish;
                    }
                    if (responseIca.BirthCity != null)
                    {
                        person.cityOfBirthid = responseIca.BirthCity.Id;
                        person.cityOfBirtharDesc = responseIca.BirthCity.DescriptionArabic;
                        person.cityOfBirthenDesc = responseIca.BirthCity.DescriptionEnglish;
                    }

                }
                if (!string.IsNullOrEmpty(person.nationalityid) && !string.IsNullOrEmpty(person.identityBacknumber) && !string.IsNullOrEmpty(person.unifiedNumber))
                {
                    flag = 1;
                    lstResponse.Add(person);
                    json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<PPDetails>>(lstResponse), ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "لا يوجد سجلات";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<PPDetails>>(lstResponse), ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (Exception ex)
            {
                flag = 2;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<PPDetails>>(lstResponse), ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


            }

            return Json(json, JsonRequestBehavior.AllowGet);

        }

        private static RestResponse ICPAPICALL(string emirateID)
        {

            string apiURL = ConfigurationManager.AppSettings["ICAURL"].ToString();

            var options = new RestClientOptions(apiURL)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("MOCD-APIKey", ConfigurationManager.AppSettings["ICAAPIKey"].ToString());
            request.AddHeader("Authorization", "Bearer " + GenerateToken());
            request.AddHeader("Cookie", "BIGipServerTESTAPI.MOCD.GOV.AE_POOL_5566=929305866.48661.0000");
            var inquiryJson = new
            {

                IDN = emirateID

            };
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(inquiryJson);


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }
    }
}

//public ActionResult Search(string postdata, string UserAgent)
//{
//    var json = "";
//    int flag = 0;
//    JsonHelper objHelper = new JsonHelper();
//    List<PPDetails> lstResponse = new List<PPDetails>();

//    try
//    {
//        var input = new JavaScriptSerializer().Deserialize<PersonalProfileDetails.PersonalProfileDetailsRequest>(postdata);
//        var responseIca = Call(input.EmiratesId);
//        //List<PersonalProfileDetails.PersonalProfileDetailsResponse> lstResponse = new List<PersonalProfileDetails.PersonalProfileDetailsResponse>();
//        //PersonalProfileDetails.PersonalProfileDetailsResponse objPPDetails = null;
//        PPDetails person = new PPDetails();
//        var result = string.Empty;

//        if (responseIca != null)
//        {
//            if (responseIca.identityCard != null)
//            {
//                person.identityCardnumber = responseIca.identityCard.IDN;
//                person.identityBacknumber = responseIca.identityCard.IDNBackNumber;
//                person.identityCardissuDate = responseIca.identityCard.issueDate;
//                person.identityCardexpiryDate = responseIca.identityCard.expiryDate;
//            }
//            if (responseIca.passport != null)
//            {
//                if (responseIca.passport.issueCountry != null)
//                {
//                    person.passportissueCountryid = responseIca.passport.issueCountry.id;
//                    person.passportissueCountryarDesc = responseIca.passport.issueCountry.descriptionArabic;
//                    person.passportissueCountryenDesc = responseIca.passport.issueCountry.descriptionEnglish;
//                }
//                if (responseIca.passport.passportType != null)
//                {
//                    person.passporttypeid = responseIca.passport.passportType.id;
//                    person.passporttypearDesc = responseIca.passport.passportType.descriptionArabic;
//                    person.passporttypeenDesc = responseIca.passport.passportType.descriptionEnglish;
//                }

//                person.passportnumber = responseIca.passport.passportNo;
//                person.passportissuDate = responseIca.passport.issueDate;
//                person.passportexpiryDate = responseIca.passport.expiryDate;
//                person.passportissuePlace = responseIca.passport.issuePlace;
//            }

//            if (responseIca.addresses != null && responseIca.addresses.Count > 0)
//            {
//                if (responseIca.addresses[0].localAddress != null)
//                {
//                    person.pobox = responseIca.addresses[0].localAddress.poboxNo;
//                    person.mobileNumber = responseIca.addresses[0].localAddress.mobileNo;
//                    person.homePhone = responseIca.addresses[0].localAddress.homePhone;
//                    person.workPhone = responseIca.addresses[0].localAddress.workPhone;
//                    person.Email = responseIca.addresses[0].localAddress.emailAddress;
//                    if (responseIca.addresses[0].localAddress.city != null)
//                    {
//                        person.localAddresscityid = responseIca.addresses[0].localAddress.city.id;
//                        person.localAddresscityarDesc = responseIca.addresses[0].localAddress.city.descriptionArabic;
//                        person.localAddresscityenDesc = responseIca.addresses[0].localAddress.city.descriptionEnglish;
//                    }
//                    if (responseIca.addresses[0].localAddress.area != null)
//                    {
//                        person.localAddressareaid = responseIca.addresses[0].localAddress.area.id;
//                        person.localAddressareaarDesc = responseIca.addresses[0].localAddress.area.descriptionArabic;
//                        person.localAddressareaenDesc = responseIca.addresses[0].localAddress.area.descriptionEnglish;
//                    }
//                    if (responseIca.addresses[0].localAddress.street != null)
//                    {
//                        person.localAddressstreetid = responseIca.addresses[0].localAddress.street.id;
//                        person.localAddressstreetarDesc = responseIca.addresses[0].localAddress.street.descriptionArabic;
//                        person.localAddressstreetenDesc = responseIca.addresses[0].localAddress.street.descriptionEnglish;

//                    }
//                    if (responseIca.addresses[0].localAddress.emirate != null)
//                    {
//                        person.localAddressemirateid = responseIca.addresses[0].localAddress.emirate.id;
//                        person.localAddressemiratearDesc = responseIca.addresses[0].localAddress.emirate.descriptionArabic;
//                        person.localAddressemirateenDesc = responseIca.addresses[0].localAddress.emirate.descriptionEnglish;
//                    }
//                }
//            }

//            if (responseIca.personName != null)
//            {
//                person.firstNameArabic = responseIca.personName.firstNameArabic;
//                person.firstNameEnglish = responseIca.personName.firstNameEnglish;
//                person.secondNameArabic = responseIca.personName.secondNameArabic;
//                person.secondNameEnglish = responseIca.personName.secondNameEnglish;
//                person.thirdNameArabic = responseIca.personName.thirdNameArabic;
//                person.thirdNameEnglish = responseIca.personName.thirdNameEnglish;
//                person.fourthNameArabic = responseIca.personName.fourthNameArabic;
//                person.fourthNameEnglish = responseIca.personName.fourthNameEnglish;
//                person.fullArabicName = responseIca.personName.fullNameArabic;
//                person.fullEnglishName = responseIca.personName.fullNameEnglish;
//                if (responseIca.personName.tribe != null)
//                {

//                    person.tribeid = responseIca.personName.tribe.id;
//                    person.tribearDesc = responseIca.personName.tribe.descriptionArabic;
//                    person.tribeenDesc = responseIca.personName.tribe.descriptionEnglish;
//                }
//            }



//            person.unifiedNumber = responseIca.UN;
//            person.khulasitQaidNumber = responseIca.khulasitQaidNo;
//            person.dateOfBirth = responseIca.birthDate;
//            person.placeOfBirthAr = responseIca.birthPlaceArabic;
//            person.placeOfBirthEn = responseIca.birthPlaceEnglish;
//            person.familyCount = responseIca.familyCount;
//            person.familyMaleCount = responseIca.familyMaleCount;
//            person.familyFemaleCount = responseIca.familyFemaleCount;
//            person.familyBookNumber = responseIca.familyBookNo;

//            if (responseIca.nationality != null)
//            {
//                person.nationalityid = responseIca.nationality.id;
//                person.nationalityarDesc = responseIca.nationality.descriptionArabic;
//                person.nationalityenDesc = responseIca.nationality.descriptionEnglish;
//            }
//            if (responseIca.qualification != null)
//            {
//                if (responseIca.qualification.specialization != null)
//                {
//                    person.specialization = responseIca.qualification.specialization.id;
//                    person.specializationarDesc = responseIca.qualification.specialization.descriptionArabic;
//                    person.specializationenDesc = responseIca.qualification.specialization.descriptionEnglish;
//                }
//            }
//            if (responseIca.occupation != null)
//            {
//                person.occupationID = responseIca.occupation.id;
//                person.occupationArDesc = responseIca.occupation.descriptionArabic;
//                person.occupationEnDesc = responseIca.occupation.descriptionEnglish;
//            }
//            if (responseIca.gender != null)
//            {
//                person.genderid = responseIca.gender.id;
//                person.genderarDesc = responseIca.gender.descriptionArabic;
//                person.genderenDesc = responseIca.gender.descriptionEnglish;
//            }
//            if (responseIca.birthCountry != null)
//            {
//                person.countryOfBirthid = responseIca.birthCountry.id;
//                person.countryOfBirtharDesc = responseIca.birthCountry.descriptionArabic;
//                person.countryOfBirthenDesc = responseIca.birthCountry.descriptionEnglish;
//            }
//            if (responseIca.birthEmirate != null)
//            {
//                person.emirateOfBirthid = responseIca.birthEmirate.id;
//                person.emirateOfBirtharDesc = responseIca.birthEmirate.descriptionArabic;
//                person.emirateOfBirthenDesc = responseIca.birthEmirate.descriptionEnglish;
//            }
//            if (responseIca.maritalStatus != null)
//            {
//                person.maritalStatusid = responseIca.maritalStatus.id;
//                person.maritalStatusarDesc = responseIca.maritalStatus.descriptionArabic;
//                person.maritalStatusenDesc = responseIca.maritalStatus.descriptionEnglish;
//            }
//            if (responseIca.religion != null)
//            {
//                person.religionid = responseIca.religion.id;
//                person.religionarDesc = responseIca.religion.descriptionArabic;
//                person.religionenDesc = responseIca.religion.descriptionEnglish;
//            }
//            if (responseIca.birthCity != null)
//            {
//                person.cityOfBirthid = responseIca.birthCity.id;
//                person.cityOfBirtharDesc = responseIca.birthCity.descriptionArabic;
//                person.cityOfBirthenDesc = responseIca.birthCity.descriptionEnglish;
//            }

//        }
//        if (!string.IsNullOrEmpty(person.nationalityid) && !string.IsNullOrEmpty(person.identityBacknumber) && !string.IsNullOrEmpty(person.unifiedNumber))
//        {
//            flag = 1;
//            lstResponse.Add(person);
//            json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
//            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<PPDetails>>(lstResponse), ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
//        }
//        else
//        {
//            flag = 2;
//            string ResponseDescription = "لا يوجد سجلات";
//            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
//            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<PPDetails>>(lstResponse), ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
//        }
//    }
//    catch (Exception ex)
//    {
//        flag = 2;
//        string ResponseDescription = ex.Message.ToString();
//        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
//        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<PPDetails>>(lstResponse), ConfigurationManager.AppSettings["ICAPPCode"].ToString(), ConfigurationManager.AppSettings["ICAPP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


//    }

//    return Json(json, JsonRequestBehavior.AllowGet);

//}

//       public
//      ICAModelDetails.Root
//Call(string EID)
//       {
//           string APIKey;
//           string LoginName;
//           string Password;
//           string headerUsername;
//           string headerPassword;
//           string temphas;
//           string consumerId;

//           string transactionIdICA;

//           string dateTimeValue;

//           string timestamp;

//           string transactionRefNo;

//           string ICAKey;
//           dateTimeValue = DateTime.Now.ToString("ddMMyyHHmmss");
//           consumerId = "011";
//           transactionIdICA = "0000000000";
//           timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
//           ICP_PersonalProfile.personInquiryResponse response;
//           // 'Production
//           ICAKey = ConfigurationManager.AppSettings["ICA_Key"].ToString();
//           APIKey = ConfigurationManager.AppSettings["ICA_APIKey"].ToString();
//           LoginName = ConfigurationManager.AppSettings["ICA_LoginName"].ToString();
//           Password = ConfigurationManager.AppSettings["ICA_LoginPassword"].ToString();
//           headerUsername = ConfigurationManager.AppSettings["ICA_HeaderUsername"].ToString();
//           headerPassword = ConfigurationManager.AppSettings["ICA_HeaderPassword"].ToString();

//           transactionRefNo = (consumerId
//                       + (transactionIdICA + dateTimeValue));
//           string hashValue;
//           hashValue = ("<per1:Body>" + ("<per1:transactionRefNo>"
//                       + (transactionRefNo + ("</per1:transactionRefNo>" + ("<per1:inquiry>" + ("<per1:IDN>"
//                       + (EID + ("</per1:IDN>" + ("</per1:inquiry>" + ("<per1:timestamp>"
//                       + (timestamp + ("</per1:timestamp>" + "</per1:Body>"))))))))))));
//           try
//           {
//               ICP_PersonalProfile.PersonInquiryServiceClient client = new ICP_PersonalProfile.PersonInquiryServiceClient(EndpointConfiguration.getPersonInquiry_ICAsoaphttps);
//               using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
//               {

//                   var httpRequestProperty = new HttpRequestMessageProperty();
//                   httpRequestProperty.Headers["GSB-APIKey"] = APIKey;
//                   httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = ("Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes((LoginName + (":" + Password)))));
//                   OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

//                   ICP_PersonalProfile.HeaderType headerType = new ICP_PersonalProfile.HeaderType();

//                   headerType.serviceName = ConfigurationManager.AppSettings["ICA_ServiceName"].ToString();
//                   headerType.sourceChannel = ConfigurationManager.AppSettings["ICA_Channel"].ToString();
//                   headerType.serviceVersion = ConfigurationManager.AppSettings["ICA_Version"].ToString();
//                   headerType.serviceLanguage = ConfigurationManager.AppSettings["ICA_Lang"].ToString();
//                   headerType.userName = Encrypter.EncryptData(headerUsername, ICAKey);
//                   headerType.password = Encrypter.EncryptData(headerPassword, ICAKey);
//                   temphas = MessageSigner.SignMessage(hashValue, ICAKey);
//                   headerType.hash = Convert.FromBase64String(MessageSigner.SignMessage(hashValue, ICAKey));

//                   ICP_PersonalProfile.PersonInquiryRequestType personInquiryRequest = new ICP_PersonalProfile.PersonInquiryRequestType();

//                   ICP_PersonalProfile.PersonInquiryRequestBodyType bodyType = new ICP_PersonalProfile.PersonInquiryRequestBodyType();
//                   ICP_PersonalProfile.PersonInquiryType inquiryType = new ICP_PersonalProfile.PersonInquiryType();
//                   bodyType.transactionRefNo = transactionRefNo;

//                   inquiryType.ItemElementName = (ICP_PersonalProfile.ItemChoiceType)ItemChoiceType.IDN;
//                   inquiryType.Item = EID;

//                   bodyType.timestamp = Convert.ToDateTime(timestamp);
//                   personInquiryRequest.Header = headerType;
//                   personInquiryRequest.Body = bodyType;
//                   personInquiryRequest.Body.inquiry = inquiryType;

//                   response = client.personInquiryAsync(personInquiryRequest).Result;

//                   string result = JsonConvert.SerializeObject(response.personInquiryResponse1.Body.personProfile);
//                   ICAModelDetails.Root objresp = JsonConvert.DeserializeObject<ICAModelDetails.Root>(result);

//                   return objresp;
//               }

//           }
//           catch (Exception ex)
//           {
//               throw ex;
//           }
//       }
//   }

