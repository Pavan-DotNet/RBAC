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
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace MOCDIntegrations.Controllers
{
    public class CDAController : Controller
    {
        // GET: CDA
        public ActionResult Index()
        {
            return View("CDA");
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

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<CDADetails.CDADetailsRequestParams>(postdata);
                CDAService.CDA_spcMOCD_spcWSDL_spcWFClient client = new CDAService.CDA_spcMOCD_spcWSDL_spcWFClient();
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                     Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["CDAGSBUserName"].ToString() + ":" + ConfigurationManager.AppSettings["CDAGSBPassword"].ToString()));
                    httpRequestProperty.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["CDAGSBAPIKey"].ToString();
                    httpRequestProperty.Headers["x-Gateway-APIKey"] = ConfigurationManager.AppSettings["CDASDGKey"].ToString();
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;


                    CDAService.CDA_spcMOCD_spcWSDL_spcWF_Input objRequest = new CDAService.CDA_spcMOCD_spcWSDL_spcWF_Input();
                    if (index == "1")
                        objRequest.Family_Number = input.FamilyNumber;
                    if (index == "2")
                        objRequest.Unified_Number = input.UnifiedNumber;
                    if (index == "3")
                        objRequest.Emirates_spcID = input.EmiratesId;
                    if (index == "4")
                        objRequest.PassportNum = input.PassportNo;

                    CDAService.CDA_spcMOCD_spcWSDL_spcWF_Output objResponse = client.CDA_spcMOCD_spcWSDL_spcWF(objRequest);

                    if (objResponse != null)
                    {
                        if (!string.IsNullOrEmpty(objResponse.Error_Message_EmiratesId) && !string.IsNullOrEmpty(objResponse.Error_Message_Passport_No) && !string.IsNullOrEmpty(objResponse.Error_Message_Family_No) && !string.IsNullOrEmpty(objResponse.Error_Message_Unified_No))
                        {
                            flag = 2;
                            string ResponseDescription = "";
                            if (index == "1")
                                ResponseDescription = objResponse.Error_Message_Family_No;
                            if (index == "2")
                                ResponseDescription = objResponse.Error_Message_Unified_No;
                            if (index == "3")
                                ResponseDescription = objResponse.Error_Message_EmiratesId;
                            if (index == "4")
                                ResponseDescription = objResponse.Error_Message_Passport_No;

                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                        else
                        {
                            flag = 1;
                            List<CDADetails.CDADetailsResponseParams> lstResponse = new List<CDADetails.CDADetailsResponseParams>();
                            CDADetails.CDADetailsResponseParams objResponseParams = new CDADetails.CDADetailsResponseParams();
                            objResponseParams.Aid_Source = objResponse.Aid_Source;
                            objResponseParams.Approval_Date = objResponse.Approval_Date;
                            objResponseParams.Approved = objResponse.Approved;
                            objResponseParams.Beneficiary_Name = objResponse.Beneficiary_Name;
                            objResponseParams.Category = objResponse.Category;
                            objResponseParams.CreatedDate = objResponse.CreatedDate;
                            objResponseParams.DateofBirth = objResponse.DateofBirth;
                            objResponseParams.DateofDeath = objResponse.DateofDeath;
                            objResponseParams.Description = objResponse.Description;
                            objResponseParams.Disbusrsment_Status = objResponse.Disbusrsment_Status;
                            objResponseParams.Education_Level = objResponse.Education_Level;
                            objResponseParams.File_Number = objResponse.File_Number;
                            objResponseParams.First_Name = objResponse.First_Name;
                            objResponseParams.Full_Name_Arabic = objResponse.Full_Name_Arabic;
                            objResponseParams.Good_Manner_and_Behavior_Letter = objResponse.Good_Manner_and_Behavior_Letter;
                            objResponseParams.Health_Situation = objResponse.Health_Situation;
                            objResponseParams.Job = objResponse.Job;
                            objResponseParams.Makani_Number = objResponse.Makani_Number;
                            objResponseParams.Marital_Status = objResponse.Marital_Status;
                            objResponseParams.Monthly_Income = objResponse.Monthly_Income;
                            objResponseParams.Mother_Name_Arabic = objResponse.Mother_Name_Arabic;
                            objResponseParams.Nationality = objResponse.Nationality;
                            objResponseParams.New = objResponse.New;
                            objResponseParams.Notes = objResponse.Notes;
                            objResponseParams.Number_of_Family_Members = objResponse.Number_of_Family_Members;
                            objResponseParams.PHApproved = objResponse.PHApproved;
                            objResponseParams.PHNew = objResponse.PHNew;
                            objResponseParams.PHNotes = objResponse.PHNotes;
                            objResponseParams.PHSR_pnd = objResponse.PHSR_pnd;
                            objResponseParams.PHStage = objResponse.PHStage;
                            objResponseParams.PHStatus = objResponse.PHStatus;
                            objResponseParams.PH_ReStudy = objResponse.PH_ReStudy;
                            objResponseParams.POD_Approved = objResponse.POD_Approved;
                            objResponseParams.POD_Created_Date = objResponse.POD_Created_Date;
                            objResponseParams.POD_New = objResponse.POD_New;
                            objResponseParams.POD_Notes = objResponse.POD_Notes;
                            objResponseParams.POD_ReStudy = objResponse.POD_ReStudy;
                            objResponseParams.POD_SR_pnd = objResponse.POD_SR_pnd;
                            objResponseParams.POD_Stage = objResponse.POD_Stage;
                            objResponseParams.POD_Status = objResponse.POD_Status;
                            objResponseParams.POD_Total_Benefit_Approved = objResponse.POD_Total_Benefit_Approved;
                            objResponseParams.Person_is_Dead = objResponse.Person_is_Dead;
                            objResponseParams.PreparingHouseBenefit = objResponse.PreparingHouseBenefit;
                            objResponseParams.Primary_Phone_Number = objResponse.Primary_Phone_Number;
                            objResponseParams.Re_Study = objResponse.Re_Study;
                            objResponseParams.Relation_Type = objResponse.Relation_Type;
                            objResponseParams.Request_Status = objResponse.Request_Status;
                            objResponseParams.SR_pnd = objResponse.SR_pnd;
                            objResponseParams.Stage = objResponse.Stage;
                            objResponseParams.Status = objResponse.Status;
                            objResponseParams.TH_Approved = objResponse.TH_Approved;
                            objResponseParams.TH_Created_Date = objResponse.TH_Created_Date;
                            objResponseParams.TH_New = objResponse.TH_New;
                            objResponseParams.TH_Notes = objResponse.TH_Notes;
                            objResponseParams.TH_ReStudy = objResponse.TH_ReStudy;
                            objResponseParams.TH_SR_pnd = objResponse.TH_SR_pnd;
                            objResponseParams.TH_Stage = objResponse.TH_Stage;
                            objResponseParams.TH_Status = objResponse.TH_Status;
                            objResponseParams.TH_Total_Benefit_Approved = objResponse.TH_Total_Benefit_Approved;
                            objResponseParams.Total_Benefit_Approved = objResponse.Total_Benefit_Approved;
                            objResponseParams.Type = objResponse.Type;
                            objResponseParams.TypeOfBenefit = objResponse.TypeOfBenefit;
                            objResponseParams.TypeOfBenefit_1 = objResponse.TypeOfBenefit_1;
                            objResponseParams.TypeOfBenefit_2 = objResponse.TypeOfBenefit_2;
                            objResponseParams.TypeOfBenefit_3 = objResponse.TypeOfBenefit_3;
                            objResponseParams.Type_of_Housing = objResponse.Type_of_Housing;
                            objResponseParams.Type_of_Income = objResponse.Type_of_Income;
                            objResponseParams.Zone = objResponse.Zone;
                            lstResponse.Add(objResponseParams);
                            json = JsonConvert.SerializeObject(new { lstResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<CDADetails.CDADetailsResponseParams>>(lstResponse), ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
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
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                else
                {
                    ResponseDescription = faultException.Message;
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;

                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["CDACode"].ToString(), ConfigurationManager.AppSettings["CDA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}