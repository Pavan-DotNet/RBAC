using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using MOCDstringegrations.Models;
using System.ServiceModel.Configuration;

namespace MOCDIntegrations.Controllers
{
    public class ZHOController : Controller
    {
        // GET: ZHO
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            try
            {
                if (EmiratesId.Length == 15)
                {
                    var apiResult = ZHODetailsGet(EmiratesId);
                    if (apiResult != null)
                    {
                        if (apiResult.Status)
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { apiResult, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ZHOCode"].ToString(), ConfigurationManager.AppSettings["ZHO"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records Found";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ZHOCode"].ToString(), ConfigurationManager.AppSettings["ZHO"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ZHOCode"].ToString(), ConfigurationManager.AppSettings["ZHO"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ZHOCode"].ToString(), ConfigurationManager.AppSettings["ZHO"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ZHOCode"].ToString(), ConfigurationManager.AppSettings["ZHO"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ZHOCode"].ToString(), ConfigurationManager.AppSettings["ZHO"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static ZHODetails ZHODetailsGet(string emirateID)
        {
            ZHODetails objResponse = new ZHODetails();
            DataTable DT_LULLU = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Unified"].ToString()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("ZHODetails_Get", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@E_ID", emirateID);

                    command.CommandTimeout = 0;
                    sda.SelectCommand = command;
                    sda.Fill(DT_LULLU);
                }
                connection.Close();
            }
            objResponse = new ZHODetails();
            if (DT_LULLU.Rows.Count > 0)
            {
                foreach (DataRow DR in DT_LULLU.Rows)
                {
                    objResponse.PODID = DR["PODID"].ToString();
                    objResponse.EID = DR["EID"].ToString();
                    objResponse.FULL_NAME_EN = DR["FULL_NAME_EN"].ToString();
                    objResponse.FULL_NAME_AR = DR["FULL_NAME_AR"].ToString();
                    objResponse.DisabilityAxisCode = DR["DisabilityAxisCode"].ToString();
                    objResponse.DisabilityAxisAr = DR["DisabilityAxisAr"].ToString();
                    objResponse.DisabilityAxisEn = DR["DisabilityAxisEn"].ToString();
                    objResponse.FK_DISABILITY = DR["FK_DISABILITY"].ToString();
                    objResponse.DISABILITY_AR = DR["DISABILITY_AR"].ToString();
                    objResponse.DISABILITY_EN = DR["DISABILITY_EN"].ToString();
                    objResponse.DATE_OF_BIRTH = DR["DATE_OF_BIRTH"].ToString();
                    objResponse.IsDeath = DR["IsDeath"].ToString();
                    objResponse.DATE_OF_DEATH = DR["DATE_OF_DEATH"].ToString();
                    objResponse.AgeYears = DR["AgeYears"].ToString();
                    objResponse.FAMILY_BOOK_NO = DR["FAMILY_BOOK_NO"].ToString();
                    objResponse.UNIFIED_NO = DR["UNIFIED_NO"].ToString();
                    objResponse.PASSPORT_NO = DR["PASSPORT_NO"].ToString();
                    objResponse.PASSPORT_EXPIRY = DR["SCHOOL_AR"].ToString();
                    objResponse.VISA_EXPIRY = DR["VISA_EXPIRY"].ToString();
                    objResponse.FK_GENDER = DR["FK_GENDER"].ToString();
                    objResponse.GENDER_AR = DR["GENDER_AR"].ToString();
                    objResponse.GENDER_EN = DR["GENDER_EN"].ToString();
                    objResponse.FK_CITIZEN = DR["FK_CITIZEN"].ToString();
                    objResponse.CITIZEN_Ar = DR["CITIZEN_Ar"].ToString();
                    objResponse.CITIZEN_En = DR["CITIZEN_En"].ToString();
                    objResponse.FK_NATIONALITY = DR["FK_NATIONALITY"].ToString();
                    objResponse.NATIONALITY_AR = DR["NATIONALITY_AR"].ToString();
                    objResponse.NATIONALITY_EN = DR["NATIONALITY_EN"].ToString();
                    objResponse.DefaultLang = DR["DefaultLang"].ToString();
                    objResponse.FK_RELIGION = DR["FK_RELIGION"].ToString();
                    objResponse.RELIGION_AR = DR["RELIGION_AR"].ToString();
                    objResponse.RELIGION_EN = DR["RELIGION_EN"].ToString();
                    objResponse.FK_REGION = DR["FK_REGION"].ToString();
                    objResponse.REGION_AR = DR["REGION_AR"].ToString();
                    objResponse.REGION_EN = DR["REGION_EN"].ToString();
                    objResponse.FK_ACCOMMODATION_TYPE = DR["FK_ACCOMMODATION_TYPE"].ToString();
                    objResponse.ACCOMMODATION_TYPE_AR = DR["ACCOMMODATION_TYPE_AR"].ToString();
                    objResponse.ACCOMMODATION_TYPE_EN = DR["ACCOMMODATION_TYPE_EN"].ToString();
                    objResponse.FK_DISABILITY_LEVEL = DR["FK_DISABILITY_LEVEL"].ToString();
                    objResponse.DISABILITY_LEVEL_AR = DR["DISABILITY_LEVEL_AR"].ToString();
                    objResponse.DISABILITY_LEVEL_EN = DR["DISABILITY_LEVEL_EN"].ToString();
                    objResponse.FK_SUB_DISABILITY = DR["FK_SUB_DISABILITY"].ToString();
                    objResponse.REG_IN_CENTER = DR["REG_IN_CENTER"].ToString();
                    objResponse.REG_IN_SCHOOL = DR["REG_IN_SCHOOL"].ToString();
                    objResponse.SCHOOL_AR = DR["SCHOOL_AR"].ToString();
                    objResponse.SCHOOL_EN = DR["SCHOOL_EN"].ToString();
                    objResponse.EMAIL = DR["EMAIL"].ToString();
                    objResponse.MOBILE1 = DR["MOBILE1"].ToString();
                    objResponse.MOBILE2 = DR["MOBILE2"].ToString();
                    objResponse.HOME_TEL = DR["HOME_TEL"].ToString();
                    objResponse.MobileNo_FromSEHA = DR["MobileNo_FromSEHA"].ToString();
                    objResponse.HomeNo_FromSEHA = DR["HomeNo_FromSEHA"].ToString();
                    objResponse.Email_FromSEHA = DR["Email_FromSEHA"].ToString();
                    objResponse.FK_MARITAL_STATUS = DR["FK_MARITAL_STATUS"].ToString();
                    objResponse.MARITAL_STATUS_AR = DR["MARITAL_STATUS_AR"].ToString();
                    objResponse.MARITAL_STATUS_EN = DR["MARITAL_STATUS_EN"].ToString();
                    objResponse.FK_QUALIFICATION = DR["FK_QUALIFICATION"].ToString();
                    objResponse.QUALIFICATION_AR = DR["QUALIFICATION_AR"].ToString();
                    objResponse.QUALIFICATION_EN = DR["QUALIFICATION_EN"].ToString();
                    objResponse.IS_WORKING = DR["IS_WORKING"].ToString();
                    objResponse.Latitude = DR["Latitude"].ToString();
                    objResponse.Longitude = DR["Longitude"].ToString();
                    objResponse.IsConfirmed = DR["IsConfirmed"].ToString();
                    objResponse.HasZHOPODCard = DR["HasZHOPODCard"].ToString();
                    objResponse.ZHOPODCARDNo = DR["ZHOPODCARDNo"].ToString();
                    objResponse.ZHOPODCard_IssueDate = DR["ZHOPODCard_IssueDate"].ToString();
                    objResponse.ZHOPODCard_ExpiryDate = DR["ZHOPODCard_ExpiryDate"].ToString();
                    objResponse.HasFazaaCard = DR["HasFazaaCard"].ToString();
                    objResponse.FazaaCardNo = DR["FazaaCardNo"].ToString();
                    objResponse.IsZHOStudent = DR["IsZHOStudent"].ToString();
                    objResponse.ZHOStudentNo = DR["ZHOStudentNo"].ToString();
                    objResponse.ZHOStudentProgram_CODE = DR["ZHOStudentProgram_CODE"].ToString();
                    objResponse.ZHOStudentProgram_AR = DR["ZHOStudentProgram_AR"].ToString();
                    objResponse.ZHOStudentProgram_EN = DR["ZHOStudentProgram_EN"].ToString();
                    objResponse.IsValidEID = DR["IsValidEID"].ToString();
                    objResponse.Has_Social_Support = DR["Has_Social_Support"].ToString();
                    objResponse.SSA_Support_Amount = DR["SSA_Support_Amount"].ToString();
                    objResponse.CENTER_CODE = DR["CENTER_CODE"].ToString();
                    objResponse.CENTER_AR = DR["CENTER_AR"].ToString();
                    objResponse.CENTER_EN = DR["CENTER_EN"].ToString();
                    objResponse.FK_SOURCE = DR["FK_SOURCE"].ToString();
                    objResponse.CREATED_ON = DR["CREATED_ON"].ToString();
                    objResponse.CREATED_BY = DR["CREATED_BY"].ToString();
                    objResponse.UPDATED_ON = DR["UPDATED_ON"].ToString();
                    objResponse.UPDATED_BY = DR["UPDATED_BY"].ToString();

                    objResponse.Status = true;
                }

            }
            else
            {
                objResponse.Status = false;

            }
            return objResponse;


        }
    }
}