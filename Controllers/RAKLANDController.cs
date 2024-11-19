using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace MOCDIntegrations.Controllers
{
    public class RAKLANDController : Controller
    {

        // GET: RAKLAND
        public ActionResult Index()
        {
            DirectorySearcher searcher = new DirectorySearcher();
            string UserName = Regex.Replace(User.Identity.Name, @"^(?<domain>.*)\\(?<username>.*)|(?<username>[^\@]*)@(?<domain>.*)?$", "${username}", RegexOptions.None);
            searcher.Filter = string.Format("sAMAccountName={0}", UserName);
            SearchResult user = searcher.FindOne();
            if (user != null)
            {
                string emailAddr = user.Properties["mail"][0].ToString();
                ViewBag.Email = emailAddr;
            }
            else
                ViewBag.Email = UserName + "@msa.gov.ae";
            return View("RAKLAND");
        }

        public ActionResult GetEmailAddress()
        {
            ViewBag.Email = Membership.GetUser().Email;
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> UploadEIDFile()
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        RAKLandDocuments objdoc = new RAKLandDocuments();
                        objdoc.FileName = fileContent.FileName;
                        objdoc.FileType = fileContent.ContentType;
                        objdoc.FileSize = fileContent.ContentLength.ToString();

                        byte[] fileInBytes = new byte[fileContent.ContentLength];
                        using (BinaryReader theReader = new BinaryReader(fileContent.InputStream))
                        {
                            fileInBytes = theReader.ReadBytes(fileContent.ContentLength);
                        }
                        // string fileAsString = Convert.ToBase64String(fileInBytes);

                        objdoc.Base64String = fileInBytes;

                        Session["EIDDocument"] = objdoc;
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("تم تحميل الملف بنجاح");
        }

        [HttpPost]
        public async Task<JsonResult> UploadPassFile()
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        RAKLandDocuments objdoc = new RAKLandDocuments();
                        objdoc.FileName = fileContent.FileName;
                        objdoc.FileType = fileContent.ContentType;
                        objdoc.FileSize = fileContent.ContentLength.ToString();

                        byte[] fileInBytes = new byte[fileContent.ContentLength];
                        using (BinaryReader theReader = new BinaryReader(fileContent.InputStream))
                        {
                            fileInBytes = theReader.ReadBytes(fileContent.ContentLength);
                        }
                        // string fileAsString = Convert.ToBase64String(fileInBytes);

                        objdoc.Base64String = fileInBytes;

                        Session["PassDocument"] = objdoc;
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("تم تحميل الملف بنجاح");
        }

        [HttpPost]
        public async Task<JsonResult> UploadFamFile()
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        RAKLandDocuments objdoc = new RAKLandDocuments();
                        objdoc.FileName = fileContent.FileName;
                        objdoc.FileType = fileContent.ContentType;
                        objdoc.FileSize = fileContent.ContentLength.ToString();

                        byte[] fileInBytes = new byte[fileContent.ContentLength];
                        using (BinaryReader theReader = new BinaryReader(fileContent.InputStream))
                        {
                            fileInBytes = theReader.ReadBytes(fileContent.ContentLength);
                        }
                        // string fileAsString = Convert.ToBase64String(fileInBytes);

                        objdoc.Base64String = fileInBytes;

                        Session["FamDocument"] = objdoc;
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("تم تحميل الملف بنجاح");
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

            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<RAKLandDetails>(postdata);
                RAKLAND.SI_MOCD_CI_OBClient client = new RAKLAND.SI_MOCD_CI_OBClient();

                RAKLAND.SI_MOCD_CI_OBRequest objRequest = new RAKLAND.SI_MOCD_CI_OBRequest();
                RAKLAND.DT_MOCD_CI_Request dT_MOCD_CI_Request = new RAKLAND.DT_MOCD_CI_Request();
                RAKLAND.DT_MOCD_CI_RequestEid_att dT_MOCD_CI_RequestEid_Att = new RAKLAND.DT_MOCD_CI_RequestEid_att();
                RAKLAND.DT_MOCD_CI_RequestPassport_att dT_MOCD_CI_RequestPassport_Att = new RAKLAND.DT_MOCD_CI_RequestPassport_att();
                RAKLAND.DT_MOCD_CI_RequestFamily_book_att dT_MOCD_CI_RequestFamily_Book_Att = new RAKLAND.DT_MOCD_CI_RequestFamily_book_att();


                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;


                    dT_MOCD_CI_Request.eid = input.EmiratesId;
                    dT_MOCD_CI_Request.email = input.Email;

                    if (Session["EIDDocument"] != null)
                    {
                        RAKLandDocuments objEID = Session["EIDDocument"] as RAKLandDocuments;

                        dT_MOCD_CI_RequestEid_Att.file_description = objEID.FileName;
                        dT_MOCD_CI_RequestEid_Att.file_data = objEID.Base64String;
                        dT_MOCD_CI_RequestEid_Att.file_mime_type = objEID.FileType;

                        dT_MOCD_CI_Request.eid_att = dT_MOCD_CI_RequestEid_Att;
                    }

                    if (Session["PassDocument"] != null)
                    {
                        RAKLandDocuments objPass = Session["PassDocument"] as RAKLandDocuments;

                        dT_MOCD_CI_RequestPassport_Att.file_description = objPass.FileName;
                        dT_MOCD_CI_RequestPassport_Att.file_data = objPass.Base64String;
                        dT_MOCD_CI_RequestPassport_Att.file_mime_type = objPass.FileType;

                        dT_MOCD_CI_Request.passport_att = dT_MOCD_CI_RequestPassport_Att;
                    }


                    if (Session["FamDocument"] != null)
                    {
                        RAKLandDocuments objFAM = Session["FamDocument"] as RAKLandDocuments;

                        dT_MOCD_CI_RequestFamily_Book_Att.file_description = objFAM.FileName;
                        dT_MOCD_CI_RequestFamily_Book_Att.file_data = objFAM.Base64String;
                        dT_MOCD_CI_RequestFamily_Book_Att.file_mime_type = objFAM.FileType;

                        dT_MOCD_CI_Request.family_book_att = dT_MOCD_CI_RequestFamily_Book_Att;

                    }

                    RAKLandResponse objRakLandResponse = null;
                    RAKLAND.DT_MOCD_CI_Response objResponse = client.SI_MOCD_CI_OB(dT_MOCD_CI_Request);

                    if (objResponse != null)
                    {

                        RAKLAND.DT_MOCD_CI_ResponseEt_return[] dT_MOCD_CI_ResponseEt_Return = objResponse.et_return;

                        if (dT_MOCD_CI_ResponseEt_Return.Length > 0)
                        {
                            flag = 1;
                            foreach (RAKLAND.DT_MOCD_CI_ResponseEt_return objreturn in dT_MOCD_CI_ResponseEt_Return)
                            {
                                objRakLandResponse = new RAKLandResponse();
                                objRakLandResponse.id = objreturn.id;
                                objRakLandResponse.field = objreturn.field;
                                objRakLandResponse.log_msg_no = objreturn.log_msg_no;
                                objRakLandResponse.log_no = objreturn.log_no;
                                objRakLandResponse.message = objreturn.message;
                                objRakLandResponse.message_v1 = objreturn.message_v1;
                                objRakLandResponse.message_v2 = objreturn.message_v2;
                                objRakLandResponse.message_v3 = objreturn.message_v3;
                                objRakLandResponse.message_v4 = objreturn.message_v4;
                                objRakLandResponse.number = objreturn.number;
                                objRakLandResponse.parameter = objreturn.parameter;
                                objRakLandResponse.row = objreturn.row;
                                objRakLandResponse.System = objreturn.system;
                                objRakLandResponse.type = objreturn.type;

                                json = JsonConvert.SerializeObject(new { objRakLandResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<RAKLandResponse>(objRakLandResponse), ConfigurationManager.AppSettings["RAKLANDCode"].ToString(), ConfigurationManager.AppSettings["RAKLAND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                            }
                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "غير قادر على إنشاء حالة";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["RAKLANDCode"].ToString(), ConfigurationManager.AppSettings["RAKLAND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                    }
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["RAKLANDCode"].ToString(), ConfigurationManager.AppSettings["RAKLAND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["RAKLANDCode"].ToString(), ConfigurationManager.AppSettings["RAKLAND"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);

        }
    }
}