using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using RestSharp;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;

namespace MOCDIntegrations.Controllers
{
    public class MOJCaseDetailsController : Controller
    {
        // GET: MOJCaseDetails
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
                    var apiResult = MOJCaseDetailsGet(EmiratesId);
                    if (apiResult != null)
                    {
                        if (apiResult.Status)
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { apiResult, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["MOJCCode"].ToString(), ConfigurationManager.AppSettings["MOJC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records Found";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["MOJCCode"].ToString(), ConfigurationManager.AppSettings["MOJC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["MOJCCode"].ToString(), ConfigurationManager.AppSettings["MOJC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["MOJCCode"].ToString(), ConfigurationManager.AppSettings["MOJC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["MOJCCode"].ToString(), ConfigurationManager.AppSettings["MOJC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["MOJCCode"].ToString(), ConfigurationManager.AppSettings["MOJC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static MOJCaseDetailsModel.CaseDetails MOJCaseDetailsGet(string emirateID)
        {
            MOJCaseDetailsModel.CaseDetails objResponse = new MOJCaseDetailsModel.CaseDetails();
            DataTable DT_LULLU = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Unified"].ToString()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("MOJCaseDetails_Get", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@E_ID", emirateID);

                    command.CommandTimeout = 0;
                    sda.SelectCommand = command;
                    sda.Fill(DT_LULLU);
                }
                connection.Close();
            }
            objResponse.Plaintiff = new MOJCaseDetailsModel.PLAINTIFF();
            if (DT_LULLU.Rows.Count > 0)
            {
                foreach (DataRow DR in DT_LULLU.Rows)
                {
                    objResponse.EMIRATE = DR["EMIRATE"].ToString();
                    objResponse.COURT = DR["COURT"].ToString();
                    objResponse.CASE_NUMBER = DR["CASE_NUMBER"].ToString();
                    objResponse.YEAR = DR["YEAR"].ToString();
                    objResponse.CASE_STATUS = DR["CASE_STATUS"].ToString();
                    objResponse.COURT_TYPE = DR["COURT_TYPE"].ToString();
                    objResponse.PROCEEDING_TYPE = DR["PROCEEDING_TYPE"].ToString();
                    objResponse.SUB_PROCEEDING_TYPE = DR["SUB_PROCEEDING_TYPE"].ToString();
                    objResponse.FILING_PARTY_TYPE = DR["FILING_PARTY_TYPE"].ToString();
                    objResponse.FILING_PARTY_NAME = DR["FILING_PARTY_NAME"].ToString();
                    objResponse.DATE_CASE_OPENED = DR["DATE_CASE_OPENED"].ToString();
                    objResponse.CLAIM_AMOUNT = DR["CLAIM_AMOUNT"].ToString();
                    objResponse.FEE = DR["FEE"].ToString();
                    objResponse.FEE_PAYMENT_STATUS = DR["FEE_PAYMENT_STATUS"].ToString();

                    objResponse.Plaintiff = new MOJCaseDetailsModel.PLAINTIFF
                    {
                        PLAINTIFF_NAME = DR["NAME"].ToString(),
                        PLAINTIFF_NATIONALITY = DR["NATIONALITY"].ToString(),
                        PLAINTIFF_E_ID = DR["E_ID"].ToString(),
                        PLAINTIFF_MOBILE = DR["MOBILE"].ToString(),
                        PLAINTIFF_EMAIL = DR["EMAIL"].ToString(),
                        Type = DR["Type"].ToString() == "P" ? "PLAINTIFF" : "Defendent",
                    };
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