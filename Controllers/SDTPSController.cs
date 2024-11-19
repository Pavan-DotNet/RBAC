using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.DirectoryServices.AccountManagement;
using System.Text.RegularExpressions;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Web.Http.Description;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using static MOCDIntegrations.Models.SDTPSDetails;
using System.Reflection.Emit;
using System.Web.Services.Description;

namespace MOCDIntegrations.Controllers
{
    public class SDTPSController : Controller
    {
        // GET: SDTPS
        public ActionResult Index()
        {
            return View("SDTPS");
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

        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                var input = new JavaScriptSerializer().Deserialize<SDTPSDetails.SDTPSDetailsRequestParam>(postdata);

                SDTPSService.DhIntegrationServicePortTypeClient client = new SDTPSService.DhIntegrationServicePortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    SDTPSService.grantPlotLetterVO param = new SDTPSService.grantPlotLetterVO();
                    param.beneficiaryId = input.EmiratesId;
                    param.branchCode = input.BranchCode;
                    param.receiverId = ConfigurationManager.AppSettings["SDTPSRecieverId"].ToString();
                    param.userName = ConfigurationManager.AppSettings["SDTPSUserName"].ToString(); // Regex.Replace(User.Identity.Name, @"^(?<domain>.*)\\(?<username>.*)|(?<username>[^\@]*)@(?<domain>.*)?$", "${username}", RegexOptions.None);
                    param.beneficiaryName = input.BeneficiaryName;

                    string strTransactionNumber = client.createGrantedPlotLetterTr(param).ToString();



                    if (strTransactionNumber != null)
                    {
                        flag = 1;
                        string ResponseDescription = "Your Request has been successfully submitted.";
                        SaveSDTPSDetails(strTransactionNumber, User.Identity.Name, User.Identity.Name, input.BeneficiaryName, input.EmiratesId);
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["SDTPSCode"].ToString(), ConfigurationManager.AppSettings["SDTPS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SDTPSCode"].ToString(), ConfigurationManager.AppSettings["SDTPS"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }
            }
            catch (FaultException faultException)
            {
                var fault = faultException.CreateMessageFault();
                var doc = new XmlDocument();
                var innerdoc = new XmlDocument();
                var innersdoc = new XmlDocument();
                var nav = doc.CreateNavigator();
                flag = 3;
                string ResponseDescription = string.Empty;

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
                                    // str += "Contact GSB Support.";
                                }
                            }

                            if (child.Name == "Detail")
                            {
                                //innerdoc.LoadXml(child.InnerXml);
                                //foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                //{
                                //    if (chd.Name == "errorMessageArField")
                                //    {
                                //        str += chd.InnerText + " - ";
                                //    }
                                //    if (chd.Name == "errorMessageEnField")
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

                                str += child.InnerXml;

                            }
                            ResponseDescription += str;
                        }

                    }

                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                }
                else
                {
                    ResponseDescription = faultException.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static void SaveSDTPSDetails(string TransactionNumber, string UserName, string UserId, string BeneficiaryName, string BeneficiaryId)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["SDTPS_Save"].ToString()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["SDTPSTransaction_save"].ToString(), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@TransactionNumber", SqlDbType.NVarChar).Value = TransactionNumber;
                        command.Parameters.AddWithValue("@UserName", SqlDbType.NVarChar).Value = UserName;
                        command.Parameters.AddWithValue("@UserId", SqlDbType.NVarChar).Value = UserId;
                        command.Parameters.AddWithValue("@BeneficiaryName", SqlDbType.NVarChar).Value = BeneficiaryName;
                        command.Parameters.AddWithValue("@BeneficiaryId", SqlDbType.NVarChar).Value = BeneficiaryId;

                        command.CommandTimeout = 0;
                        command.ExecuteNonQuery();
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

        public ActionResult LoadMaster()
        {
            int flag = 0;
            var json = "";
            List<SDTPSDetails.SDTPSTransactionDetailsResponse> response = new List<SDTPSTransactionDetailsResponse>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["SDTPS_Save"].ToString()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["SDTPSTransaction_get"].ToString(), connection))
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
                                MOCDIntegrations.SDTPSService.finalProductVO responses = CheckStatus(IEUtils.ToString(reader["TransactionNumber"]));
                                response.Add(
                                    new SDTPSTransactionDetailsResponse
                                    {
                                        UserId = (IEUtils.ToString(reader["UserId"])),
                                        UserName = (IEUtils.ToString(reader["UserName"])),
                                        TransactionNumber = (IEUtils.ToString(reader["TransactionNumber"])),
                                        BeneficiaryId = (IEUtils.ToString(reader["BeneficiaryId"])),
                                        BeneficiaryName = (IEUtils.ToString(reader["BeneficiaryName"])),
                                        InsertDate = (IEUtils.ToString(reader["InsertDate"])),
                                        Status = StatusConverter(responses.statusId.ToString()),
                                        Base64 = responses.content != null ? Convert.ToBase64String(responses.content, 0, responses.content.Length) : null
                                    });
                            }
                        }

                    }

                }

                SDTPSService.DhIntegrationServicePortTypeClient client = new SDTPSService.DhIntegrationServicePortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    SDTPSService.branchVO[] branch = client.getBranchesList();
                    SDTPSDetails.BranchList objBranch = null;
                    List<SDTPSDetails.BranchList> lstBranch = new List<SDTPSDetails.BranchList>();

                    foreach (SDTPSService.branchVO obranch in branch)
                    {
                        objBranch = new SDTPSDetails.BranchList();
                        objBranch.branchId = obranch.branchId;
                        objBranch.branchNameAr = obranch.branchNameAr;
                        objBranch.branchNameEn = obranch.branchNameEn;
                        lstBranch.Add(objBranch);
                    }

                    if (lstBranch.Count > 0)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstBranch, response, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Branch Data";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    }
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new
                {
                    ResponseDescription,
                    flag
                }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public string StatusConverter(string statusId)
        {

            switch (statusId)
            {
                case "1":
                    return "مسودة";

                case "2":
                    return "قيد المراجعة لطلب التقديم";

                case "3":
                    return "بانتظار سداد الرسوم";

                case "4":
                    return "قيد الدراسة";

                case "5":
                    return "منجزة";

                case "6":
                    return "غير مقبولة";

                case "8":
                    return "منتهي الصلاحية";

                default:
                    return "";


            }
        }
        public MOCDIntegrations.SDTPSService.finalProductVO CheckStatus(string transactionNumber)
        {
            //transactionNumber = "1062023000116930";


            List<SDTPSDetails.SDTPSTransactionDetailsResponse> response = new List<SDTPSTransactionDetailsResponse>();
            try
            {


                SDTPSService.DhIntegrationServicePortTypeClient client = new SDTPSService.DhIntegrationServicePortTypeClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    MOCDIntegrations.SDTPSService.finalProductVO responses = client.getFinalProductStatus(transactionNumber);


                    return responses;
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                return new MOCDIntegrations.SDTPSService.finalProductVO();
                throw ex;
            }
        }
    }
}