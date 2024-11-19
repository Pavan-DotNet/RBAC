using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MOCDIntegrations.Controllers
{
    public class MOEStudentsHistoricalController : Controller
    {
        // GET: MOEStudentsHistorical
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
                    var response = LoadData(EmiratesId);//("784198932621877");
                    if (response.Success)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { response, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADGCode"].ToString(), ConfigurationManager.AppSettings["ADG"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADGCode"].ToString(), ConfigurationManager.AppSettings["ADG"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
            }
            catch (FaultException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADGCode"].ToString(), ConfigurationManager.AppSettings["ADG"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (WebException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADGCode"].ToString(), ConfigurationManager.AppSettings["ADG"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public MOEStudentHistoricalDetails LoadData(string emiratesId)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Unified"].ToString()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["MOEStudentData_get"].ToString(), connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@studentEmirateID", emiratesId);

                    command.CommandTimeout = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        MOEStudentHistoricalDetails response = new MOEStudentHistoricalDetails();
                        if (reader.Read())
                        {
                            response = new MOEStudentHistoricalDetails
                            {
                                academic_Year = IEUtils.ToString(reader["academic_Year"]),
                                date_of_Birth = IEUtils.ToString(reader["date_of_Birth"]),
                                emirates_ID = IEUtils.ToString(reader["emirates_ID"]),
                                exitreason_ar = IEUtils.ToString(reader["exitreason_ar"]),
                                exit_date = IEUtils.ToString(reader["exit_date"]),
                                finalAVG = IEUtils.ToString(reader["finalAVG"]),
                                Success = true,
                                full_Name_AR = IEUtils.ToString(reader["full_Name_AR"]),
                                full_Name_EN = IEUtils.ToString(reader["full_Name_EN"]),
                                gender = IEUtils.ToString(reader["gender"]),
                                grade_level = IEUtils.ToString(reader["grade_level"]),
                                nationality = IEUtils.ToString(reader["nationality"]),
                                schoolID = IEUtils.ToString(reader["schoolID"]),
                                studentID = IEUtils.ToString(reader["studentID"]),
                                student_Age = IEUtils.ToString(reader["student_Age"]),

                            };

                        }
                        return response;
                    }

                }
            }
        }
    }
}