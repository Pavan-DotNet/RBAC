using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Net;
using System.ServiceModel;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace MOCDIntegrations.Controllers
{
    public class MOEChildProtectionController : Controller
    {
        // GET: MOEPODEducation
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
        public MOEEducationDetails.ChildProtectionUnitDatum LoadData(string emiratesId)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Unified"].ToString()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["MOEPODEducation_get"].ToString(), connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@studentEmirateID", emiratesId);

                    command.CommandTimeout = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        MOEEducationDetails.ChildProtectionUnitDatum response = new MOEEducationDetails.ChildProtectionUnitDatum();
                        if (reader.Read())
                        {
                            response = new MOEEducationDetails.ChildProtectionUnitDatum
                            {
                                childFullName = IEUtils.ToString(reader["childFullName"]),
                                dateOfTheIncidentCreation = IEUtils.ToString(reader["dateOfTheIncidentCreation"]),
                                incidentNumber = IEUtils.ToString(reader["incidentNumber"]),
                                studentEmirateID = IEUtils.ToString(reader["studentEmirateID"]),
                                studentID = IEUtils.ToString(reader["studentID"]),
                                typeAndDegreeOfAbuse = IEUtils.ToString(reader["typeAndDegreeOfAbuse"]),
                                Success = true
                            };

                        }
                        return response;
                    }

                }
            }
        }
    }
}