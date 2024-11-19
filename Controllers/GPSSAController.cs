using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Controllers
{
    public class GPSSAController : Controller
    {
        // GET: GPSSA
        public ActionResult Index()
        {
            return View("GPSSA");
        }

        //private string GenerateToken()
        //{
        //    try
        //    {
        //        oAuthTokenGeneration obj = new oAuthTokenGeneration();
        //        TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["uri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["client_id"].ToString(), ConfigurationManager.AppSettings["client_secret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
        //        return tknDetails.access_token;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateTokenWebMethods(ConfigurationManager.AppSettings["ICATokenUri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["GPSSAClientId"].ToString(), ConfigurationManager.AppSettings["GPSSAClientSecret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string LoadJobTransId()
        {
            return "MOCD_GPSSAJob_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }

        private string LoadPensTransId()
        {
            return "MOCD_GPSSAPension_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }


        public JsonResult Search(string stype, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                var input = new JavaScriptSerializer().Deserialize<GPSSADetails.GPSSARequestParams>(postdata);
                try
                {
                    JsonHelper objHelper = new JsonHelper();
                    var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["GPSSAUrl"].ToString() + input.EmiratesID.Trim());
                    request.ContentType = "application/json";
                    request.Method = "GET";
                    request.Headers.Add("Authorization", "Bearer " + GenerateToken());
                    request.Headers.Add("MOCD-APIKey", ConfigurationManager.AppSettings["GPSSAAPIKEY"].ToString());

                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                    if (stype == "1")
                    {
                        GPSSADetails.GPSSAPensionAndJob gp = JsonConvert.DeserializeObject<GPSSADetails.GPSSAPensionAndJob>(responseString);
                        List<GPSSADetails.GPSSAPensionService.data> lstPerson = new List<GPSSADetails.GPSSAPensionService.data>();
                        GPSSADetails.GPSSAPensionService.data objPerson = null;

                        if (gp != null && gp.personalDetails != null && gp.personalDetails.persona != null && gp.personalDetails.persona.Count > 0)
                        {
                            flag = 1;

                            objPerson = new GPSSADetails.GPSSAPensionService.data();
                            foreach (var i in gp.personalDetails.persona)
                            {
                                if (i.type.ToLower() == "beneficiary"||i.type.ToLower() == "beneficary" || i.type.ToLower() == "pensioner")
                                {
                                    objPerson.name = gp.personalDetails.fullNameArabic;
                                    objPerson.customerSegment = "";
                                    objPerson.customerType = i.type.ToLower();
                                    objPerson.nationality = "";
                                    objPerson.nonResident = "";
                                    objPerson.emiratesId = gp.personalDetails.emiratesId;
                                    objPerson.gender = "";
                                    objPerson.dataOfBirth = "";
                                    objPerson.familyBookId = "";
                                    objPerson.familyCityId = gp.personalDetails.memberEmirate;
                                    objPerson.familyId = "";
                                    if (gp.gpssaPensionDetails != null && gp.gpssaPensionDetails.Count > 0)
                                    {
                                        if (i.type.ToLower() == "beneficary" || i.type.ToLower() == "beneficiary")
                                        {
                                            objPerson.pensionStatus = gp.gpssaPensionDetails[0].beneficiaryPensionDetails[0].pensionStatus;
                                            objPerson.monthlyPension = gp.gpssaPensionDetails[0].beneficiaryPensionDetails[0].grossPensionSalaryAmount;
                                            objPerson.pensionStartDate = gp.gpssaPensionDetails[0].beneficiaryPensionDetails[0].pensionStartDate;
                                        }

                                        if (i.type.ToLower() == "pensioner")
                                        {
                                            objPerson.pensionStatus = gp.gpssaPensionDetails[0].retirementPensionDetails[0].pensionStatus;
                                            objPerson.monthlyPension = gp.gpssaPensionDetails[0].retirementPensionDetails[0].grossPensionSalaryAmount;
                                            objPerson.pensionStartDate = gp.gpssaPensionDetails[0].retirementPensionDetails[0].pensionStartDate;

                                        }
                                    }
                                    objPerson.stopPensionReason = "";
                                    objPerson.lastDisbursementDate = "";

                                    lstPerson.Add(objPerson);

                                    json = JsonConvert.SerializeObject(new { lstPerson, flag, stype }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<GPSSADetails.GPSSAPensionService.data>>(lstPerson), ConfigurationManager.AppSettings["GPSSAPensionCode"].ToString(), ConfigurationManager.AppSettings["GPSSAPension"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


                                }
                                else
                                {
                                    flag = 2;
                                    string ResponseDescription = "No record exists";
                                    //foreach (var item in ro.errors)
                                    //    ResponseDescription += item.code + " - " + item.Message;

                                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag, stype }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["GPSSAPensionCode"].ToString(), ConfigurationManager.AppSettings["GPSSAPension"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                }

                            }


                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No record exists";
                            //foreach (var item in ro.errors)
                            //    ResponseDescription += item.code + " - " + item.Message;

                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag, stype }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["GPSSAPensionCode"].ToString(), ConfigurationManager.AppSettings["GPSSAPension"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


                        }
                    }
                    else if (stype == "2")
                    {
                        GPSSADetails.GPSSAPensionAndJob gp = JsonConvert.DeserializeObject<GPSSADetails.GPSSAPensionAndJob>(responseString);
                        List<GPSSADetails.GPSSAJobService.data> lstPerson = new List<GPSSADetails.GPSSAJobService.data>();
                        GPSSADetails.GPSSAJobService.data objPerson = null;
                        GPSSADetails.GPSSAJobService.lastEmployment objLast = null;
                        GPSSADetails.GPSSAJobService.activeEmployment objActive = null;
                        GPSSADetails.GPSSAJobService.TotalServicePeriod objService = null;

                        if (gp != null && gp.personalDetails != null && gp.personalDetails.persona != null && gp.personalDetails.persona.Count > 0)
                        {
                            flag = 1;

                            objPerson = new GPSSADetails.GPSSAJobService.data();

                            foreach (var i in gp.personalDetails.persona)
                            {
                                if (i.type.ToLower() == "insured")
                                {

                                    objPerson.name = gp.personalDetails.fullNameArabic;
                                    objPerson.customerSegment = "";
                                    objPerson.CustomerType = i.type.ToLower();
                                    objPerson.nationality = "";
                                    objPerson.emiratesId = gp.personalDetails.emiratesId;
                                    objPerson.gender = "";
                                    objPerson.dataOfBirth = "";
                                    objPerson.familyBookId = "";
                                    objPerson.familyCityId = gp.personalDetails.memberEmirate;
                                    objPerson.familyId = "";

                                    if (gp.previousGPSSAEmploymentDetails != null)
                                    {
                                        objLast = new GPSSADetails.GPSSAJobService.lastEmployment();
                                        objLast.employerName = gp.previousGPSSAEmploymentDetails[0].employerNameArabic;
                                        objLast.startDate = gp.previousGPSSAEmploymentDetails[0].employmentStartDate;
                                        objLast.endDate = gp.previousGPSSAEmploymentDetails[0].employmentStartDate;
                                        objLast.salary = "";
                                        objLast.jobTitle = gp.previousGPSSAEmploymentDetails[0].jobTitle;
                                        objLast.eosBenefits = gp.previousGPSSAEmploymentDetails[0].eosBenefitsDetails.eosRemunerationAmount;

                                    }

                                    if (gp.currentGPSSAEmploymentDetails != null)
                                    {
                                        objActive = new GPSSADetails.GPSSAJobService.activeEmployment();
                                        objActive.employerName = gp.currentGPSSAEmploymentDetails[0].employerNameArabic;
                                        objActive.startDate = gp.currentGPSSAEmploymentDetails[0].employmentStartDate;
                                        objActive.jobTitle = "";
                                        if (gp.insuredContributionDetails != null)
                                            objActive.salary = gp.insuredContributionDetails.contributableSalary;
                                    }
                                    if (gp.currentGPSSAEmploymentDetails != null && gp.currentGPSSAEmploymentDetails[0].currentEmploymentServicePeriodDetails != null)
                                    {
                                        objService = new GPSSADetails.GPSSAJobService.TotalServicePeriod();

                                        objService.days = gp.currentGPSSAEmploymentDetails[0].currentEmploymentServicePeriodDetails.totalServicePeriodDays;
                                        objService.months = gp.currentGPSSAEmploymentDetails[0].currentEmploymentServicePeriodDetails.totalServicePeriodMonths;
                                        objService.years = gp.currentGPSSAEmploymentDetails[0].currentEmploymentServicePeriodDetails.totalServicePeriodYears;


                                    }

                                    objPerson.lastEmployment = objLast;
                                    objPerson.activeEmployment = objActive;
                                    objPerson.totalServicePeriod = objService;

                                    lstPerson.Add(objPerson);

                                    json = JsonConvert.SerializeObject(new { lstPerson, flag, stype }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<GPSSADetails.GPSSAJobService.data>>(lstPerson), ConfigurationManager.AppSettings["GPSSAJobCode"].ToString(), ConfigurationManager.AppSettings["GPSSAJob"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


                                }
                                else
                                {
                                    flag = 2;
                                    string ResponseDescription = "No record exists";
                                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag, stype }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["GPSSAJobCode"].ToString(), ConfigurationManager.AppSettings["GPSSAJob"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                                }

                            }

                        }

                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No record exists";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag, stype }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["GPSSAJobCode"].ToString(), ConfigurationManager.AppSettings["GPSSAJob"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                }
                catch (WebException ex)
                {
                    flag = 3;
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        reader.ReadToEnd();

                    }
                    json = JsonConvert.SerializeObject(new { ex.Message, flag, stype }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["GPSSACode"].ToString(), ConfigurationManager.AppSettings["GPSSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                catch (Exception ex)
                {
                    flag = 3;
                    json = JsonConvert.SerializeObject(new { ex.Message, flag, stype }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["GPSSACode"].ToString(), ConfigurationManager.AppSettings["GPSSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                json = JsonConvert.SerializeObject(new
                {
                    resp,
                    flag,
                    stype
                }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, resp, ConfigurationManager.AppSettings["GPSSACode"].ToString(), ConfigurationManager.AppSettings["GPSSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                json = JsonConvert.SerializeObject(new { ex.Message, flag, stype }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["GPSSACode"].ToString(), ConfigurationManager.AppSettings["GPSSA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}