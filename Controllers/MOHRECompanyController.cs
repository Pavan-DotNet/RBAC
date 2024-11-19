using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static MOCDIntegrations.Models.OwnerDetails;

namespace MOCDIntegrations.Controllers
{
    public class MOHRECompanyController : Controller
    {
        // GET: MOHRE
        public ActionResult Index()
        {
            return View("MOHRECompany");
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
        public ActionResult Search(string index, string postdata, string UserAgent)
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
                var input = new JavaScriptSerializer().Deserialize<MOHRECompanyDetails.MOHRERequest>(postdata);
                List<MOHRECompanyDetails.MOHREResponse> lstMOHREResponse = new List<MOHRECompanyDetails.MOHREResponse>();

              
                JObject DATA = new JObject();
                if (index == "2")
                    // DATA.Add("eida", "78419755405207");
                    DATA.Add("eida", input.EmiratesId);
                else if (index == "3")
                {
                    DATA.Add("passport", input.PassportNo);
                    DATA.Add("dateOfBirth", input.DOB);
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["MOHRERCIUrl"].ToString());
                request.Method = "POST";
                request.ContentType = "application/json";

                request.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["Mohre_username"].ToString() + ":" + ConfigurationManager.AppSettings["Mohre_password"].ToString()));
                request.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["MohreCom_Key"].ToString();
                request.Headers.Add("Entity", "MOCD");

                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(DATA);
                }


                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {

                    string response = responseReader.ReadToEnd();
                    //var model = JsonConvert.DeserializeObject<List<MOHRECompanyDetails>>(response);
                    //var obj = ToObject(response) as List<MOHRECompanyDetails.MOHREResponse>; // as IDictionary<string, object>;                

                    dynamic jsonObj = JsonConvert.DeserializeObject(response);


                    if (jsonObj != null)
                    {
                        MOHRECompanyDetails.MOHREResponse objResponse = new MOHRECompanyDetails.MOHREResponse();
                        objResponse.ownerNameEnglish = jsonObj.ownerNameEnglish;
                        objResponse.ownerNameArabic = jsonObj.ownerNameArabic;
                        // objResponse.SuccessFlag = jsonObj.SuccessFlag;
                        // objResponse.ErrorDescArabic = jsonObj.ErrorDescArabic;
                        // objResponse.ErrorDescEnglish = jsonObj.ErrorDescEnglish;
                        // objResponse.ErrorFlag = jsonObj.ErrorFlag;

                        List<MOHRECompanyDetails.NationalCompaniesList> lstNationalCompanyList = new List<MOHRECompanyDetails.NationalCompaniesList>();
                        MOHRECompanyDetails.NationalCompaniesList objCmpyList = null;
                        List<MOHRECompanyDetails.ActivitiesList> lstActivitiesList = null;
                        MOHRECompanyDetails.ActivitiesList objActList = null;

                        if (jsonObj.NationalCompaniesList != null)
                        {
                            foreach (var obj in jsonObj.NationalCompaniesList)
                            {
                                objCmpyList = new MOHRECompanyDetails.NationalCompaniesList();
                                objCmpyList.CompanyCode = obj.CompanyCode;
                                objCmpyList.CompanyLicenseNumber = obj.CompanyLicenseNumber;
                                objCmpyList.CompanyNameArabic = obj.CompanyNameArabic;
                                objCmpyList.CompanyNameEnglish = obj.CompanyNameEnglish;
                                objCmpyList.LabourOfficeArb = obj.LabourOfficeArb;
                                objCmpyList.LabourOfficeEng = obj.LabourOfficeEng;
                                objCmpyList.LicenseStartDate = obj.LicenseStartDate;
                                objCmpyList.LicenseExpiryDate = obj.LicenseExpiryDate;
                                objCmpyList.TotalEmployee = obj.TotalEmployee;
                                string status = obj.ComStatus;
                                objCmpyList.ComStatus=MOHRECompanyDetails.GetComStatusDescription(status);
                                if (obj.ActivitiesList != null)
                                {
                                    lstActivitiesList = new List<MOHRECompanyDetails.ActivitiesList>();
                                    foreach (var objAct in obj.ActivitiesList)
                                    {
                                        objActList = new MOHRECompanyDetails.ActivitiesList();
                                        objActList.arabicName = objAct.arabicName;
                                        objActList.englishName = objAct.englishName;
                                        lstActivitiesList.Add(objActList);
                                    }

                                }
                                objCmpyList.ActivitiesList = lstActivitiesList;
                                lstNationalCompanyList.Add(objCmpyList);
                            }

                            objResponse.NationalCompanyList = lstNationalCompanyList;
                        }

                        lstMOHREResponse.Add(objResponse);
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstMOHREResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<MOHRECompanyDetails.MOHREResponse>>(lstMOHREResponse), ConfigurationManager.AppSettings["MOHRECOMCode"].ToString(), ConfigurationManager.AppSettings["MOHRECOM"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOHRECOMCode"].ToString(), ConfigurationManager.AppSettings["MOHRECOM"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }
            }
            catch (WebException wex)
            {
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                flag = 3;
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOHRECOMCode"].ToString(), ConfigurationManager.AppSettings["MOHRECOM"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MOHRECOMCode"].ToString(), ConfigurationManager.AppSettings["MOHRECOM"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
