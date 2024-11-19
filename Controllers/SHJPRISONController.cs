using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MOCDIntegrations.Models;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using static MOCDIntegrations.Models.DPPRISONSTATUSDetails;
using static MOCDIntegrations.Models.SDTPSDetails;
using static MOCDIntegrations.Models.SHJPRISONDATA;

namespace MOCDIntegrations.Controllers
{
    public class SHJPRISONController : Controller
    {
        // GET: SHJPRISON
        public ActionResult Index()
        {
            return View("SHJPRISON");
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
        public JsonResult Search(string index, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<SHJPRISONDATA.InquiryRequest>(postdata);

                JsonHelper objJson = new JsonHelper();
                string REQ = objJson.ConvertObjectToJSon(input);
                var REQchars = REQ.ToArray();


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["SHJPOLICEPOST_Url"].ToString());
                request.Method = "POST";
                request.ContentType = "application/json";

                byte[] bytes = Encoding.UTF8.GetBytes(REQchars);
                request.ContentLength = bytes.Length;

                request.Headers.Add("Authorization", "Bearer " + GenerateToken());

                Stream newStream = request.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                string output = string.Empty;
                using (var response = request.GetResponse())
                using (var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    output = stream.ReadToEnd();

                var RequestData = new JavaScriptSerializer().Deserialize<SHJPRISONDATA.InquirySubmitResponse>(output);


                if (RequestData.Success.ToLower() == "true")
                {


                    if (SaveSDTPSDetails(RequestData.Data, User.Identity.Name, input.InmateDetails.InmateName, input.IDNumber))
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { RequestData, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        //LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }



                    //HttpWebRequest result = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["SHJPOLICEGET_Url"].ToString() + RequestData.Data.Trim());
                    //result.Method = "GET";
                    //result.Headers.Add("Authorization", "Bearer " + GenerateToken());
                    //string finalData = string.Empty;
                    //using (var inquiryres = result.GetResponse())
                    //using (var stream = new StreamReader(inquiryres.GetResponseStream(), Encoding.UTF8))
                    //    finalData = stream.ReadToEnd();

                    //var RequestStatus = new JavaScriptSerializer().Deserialize<SHJPRISONDATA.InquiryStatusResponse>(finalData);


                }
                else if (RequestData.Success.ToLower() == "false")
                {
                    flag = 2;
                    json = JsonConvert.SerializeObject(new { output, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, output, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                json = JsonConvert.SerializeObject(new { resp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, resp, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllData(string UserAgent)
        {
            var json = "";
            int flag = 0;
            List<SHJPrResponse> response = new List<SHJPrResponse>();
            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["SDTPS_Save"].ToString()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SharjahPrisonTransacation_Get", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", User.Identity.Name);

                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //if (responses.content != null)
                            //{
                            //    string base64String = 

                            //}
                            while (reader.Read())
                            {
                                response.Add(
                                    new SHJPrResponse
                                    {
                                        TransactionId = (IEUtils.ToString(reader["TransactionId"])),
                                        EmiratesId = (IEUtils.ToString(reader["EmiratesId"])),
                                        InMateName = (IEUtils.ToString(reader["InMateName"])),
                                        InsertDate = (IEUtils.ToString(reader["InsertDate"]))
                                    });

                            }
                        }

                    }

                }

                if (response.Count > 0)
                {
                    flag = 1;

                    json = JsonConvert.SerializeObject(new { response, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    //LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string Message = "No Matching Record Found.";
                    json = JsonConvert.SerializeObject(new { Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(User.Identity.Name, Message, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                json = JsonConvert.SerializeObject(new { resp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(User.Identity.Name, resp, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(User.Identity.Name, ex.Message, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadDataByRefId(string ReferenceId, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {

                HttpWebRequest result = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["SHJPOLICEGET_Url"].ToString() + ReferenceId.Trim());
                result.Method = "GET";
                result.Headers.Add("Authorization", "Bearer " + GenerateToken());
                string finalData = string.Empty;
                using (var inquiryres = result.GetResponse())
                using (var stream = new StreamReader(inquiryres.GetResponseStream(), Encoding.UTF8))
                    finalData = stream.ReadToEnd();

                var RequestStatus = new JavaScriptSerializer().Deserialize<SHJPRISONDATA.InquiryStatusResponse>(finalData);
                if (RequestStatus.Success == "True")
                {
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { RequestStatus, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    // LogIntegrationDetails.LogSerilog(ReferenceId, json, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(ReferenceId, ResponseDescription, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }

            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                json = JsonConvert.SerializeObject(new { resp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(ReferenceId, resp, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(ReferenceId, ex.Message, ConfigurationManager.AppSettings["SHJPRISONCode"].ToString(), ConfigurationManager.AppSettings["SHJPRISON"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private static bool SaveSDTPSDetails(string TransactionId, string UserId, string InMateName, string EmiratesId)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["SDTPS_Save"].ToString()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SharjahPrisonTransacation_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@TransactionId", SqlDbType.NVarChar).Value = TransactionId;
                        command.Parameters.AddWithValue("@UserId", SqlDbType.NVarChar).Value = UserId;
                        command.Parameters.AddWithValue("@InMateName", SqlDbType.NVarChar).Value = InMateName;
                        command.Parameters.AddWithValue("@EmiratesId", SqlDbType.NVarChar).Value = EmiratesId;

                        command.CommandTimeout = 0;
                        int res = command.ExecuteNonQuery();
                        if (res > 0)
                            return true;

                        else return false;
                    }

                }
            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    try
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            APIResponse = reader.ReadToEnd();
                        }
                    }
                    catch (Exception ex1)
                    {
                        throw ex1;
                    }

                }
                throw ex;
            }
        }

    }
}
