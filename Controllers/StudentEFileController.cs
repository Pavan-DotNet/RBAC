using MOCDIntegrations.Models;
using MOCDIntegrations.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static MOCDIntegrations.Models.StudentEFile;

namespace MOCDIntegrations.Controllers
{
    public class StudentEFileController : Controller
    {
        public ActionResult Index(string emiratesId)
        {
            return View("StudentEFile");
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 1;
            string ResponseDescription = "No data found";
            StudentEFileResponse objResponse;
            try
            {
                DBManager db = new DBManager();
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<StudentEFileRequest>(postdata);
                objResponse = new StudentEFileResponse();
                objResponse = db.RetrievePODCenterStudentDetails(input.EmiratesId);

                if (string.IsNullOrEmpty(objResponse.ChildName))
                    flag = 2;
                if (flag == 1)
                {
                    json = JsonConvert.SerializeObject(new { objResponse, flag });
                    LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<StudentEFileResponse>(objResponse), ConfigurationManager.AppSettings["EFileACode"].ToString(), ConfigurationManager.AppSettings["EFileApp"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag });
                    LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["PODCardCode"].ToString(), ConfigurationManager.AppSettings["PODCardApp"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["PODCardCode"].ToString(), ConfigurationManager.AppSettings["PODCardApp"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}
