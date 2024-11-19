using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace MOCDIntegrations.Controllers
{
    public class DCMARITALController : Controller
    {
        // GET: DCMARITAL
        public ActionResult Index()
        {
            return View("");
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

        private string LoadTransId()
        {
            return String.Format("{0:d10}", (DateTime.Now.Ticks / 10) % 1000000000);
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<DCMARITALDetails.DCMARITALRequest>(postdata);
                DCMaritalService.DubaiCourtMarriageDivorceAttestationPortTypeClient client = new DCMaritalService.DubaiCourtMarriageDivorceAttestationPortTypeClient();

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    DCMaritalService.inHeaderType objHeaderType = new DCMaritalService.inHeaderType();
                    objHeaderType.Entity = "MOCD";
                    objHeaderType.EntityTransRefId = LoadTransId();

                    List<DCMARITALDetails.DCMARITALResponse> lstDCMARITALResponse = new List<DCMARITALDetails.DCMARITALResponse>();
                    DCMARITALDetails.DCMARITALResponse objDCMARITALResponse = null;

                    string ResponseDescription = string.Empty;
                    if (input.MSType == "1" || input.MSType == "2")
                    {
                        if (input.MSType == "1")
                        {
                            DCMaritalService.getBinaryFileByCertifcateRefRequestType objRequestType = new DCMaritalService.getBinaryFileByCertifcateRefRequestType();

                            if (input.DocumentType == "2")
                                objRequestType.DocumentType = DCMaritalService.getBinaryFileByCertifcateRefRequestTypeDocumentType.Attestation;
                            else if (input.DocumentType == "3")
                                objRequestType.DocumentType = DCMaritalService.getBinaryFileByCertifcateRefRequestTypeDocumentType.Divorce;
                            else if (input.DocumentType == "4")
                                objRequestType.DocumentType = DCMaritalService.getBinaryFileByCertifcateRefRequestTypeDocumentType.Marriage;

                            if (input.DocumetIssuerAuthority == "1")
                                objRequestType.DocumetIssuerAuthority = DCMaritalService.getBinaryFileByCertifcateRefRequestTypeDocumetIssuerAuthority.Court;
                            else if (input.DocumetIssuerAuthority == "2")
                                objRequestType.DocumetIssuerAuthority = DCMaritalService.getBinaryFileByCertifcateRefRequestTypeDocumetIssuerAuthority.MarriageOfficialsCourts;
                            else if (input.DocumetIssuerAuthority == "3")
                                objRequestType.DocumetIssuerAuthority = DCMaritalService.getBinaryFileByCertifcateRefRequestTypeDocumetIssuerAuthority.MarriageOfficialsOthers;
                            else if (input.DocumetIssuerAuthority == "4")
                                objRequestType.DocumetIssuerAuthority = DCMaritalService.getBinaryFileByCertifcateRefRequestTypeDocumetIssuerAuthority.Old;

                            objRequestType.DocumentNumber = input.DocumentNumber;
                            objRequestType.DocumentYear = input.DocumentYear;

                            DCMaritalService.getBinaryFileByCertifcateRefRequest objRequest = new DCMaritalService.getBinaryFileByCertifcateRefRequest();
                            objRequest.getBinaryFileByCertifcateRef = objRequestType;

                            DCMaritalService.getBinaryFileByCertifcateRefResponseType objResponseType = null;
                            DCMaritalService.outHeaderType objoutHeaderType = client.getBinaryFileByCertifcateRef(objHeaderType, objRequestType, out objResponseType);
                            if (objResponseType != null && objResponseType.result != null)
                            {
                                DCMaritalService.getBinaryFileByCertifcateRefResponseTypeResult objResult = objResponseType.result;
                                DCMaritalService.dcrequeststatusType objStatus = objResult.mdwsResponseSixthOptAllType.requeststatus;
                                if (objStatus.dcCode == "DC-200")
                                {
                                    foreach (DCMaritalService.mdwsrecType i in objResult.mdwsResponseSixthOptAllType.response)
                                    {
                                        objDCMARITALResponse = new DCMARITALDetails.DCMARITALResponse();
                                        objDCMARITALResponse.DocumentType = i.DocumentType;
                                        objDCMARITALResponse.CertifcateRef = i.CertifcateRef;
                                        objDCMARITALResponse.GregorianDate = i.GregorianDate;
                                        objDCMARITALResponse.Party1Name = i.Party1Name;
                                        objDCMARITALResponse.Party1UID = i.Party1UID;
                                        objDCMARITALResponse.Party1Nationality = i.Party1Nationality;
                                        objDCMARITALResponse.Party1BirthDate = i.Party1BirthDate;
                                        objDCMARITALResponse.Party2Name = i.Party2Name;
                                        objDCMARITALResponse.Party2UID = i.Party2UID;
                                        objDCMARITALResponse.Party2Nationality = i.Party2Nationality;
                                        objDCMARITALResponse.Party2BirthDate = i.Party2BirthDate;
                                        objDCMARITALResponse.BinaryFile = objResult.mdwsResponseSixthOptAllType.BinaryFile;
                                        lstDCMARITALResponse.Add(objDCMARITALResponse);
                                    }

                                }
                                else
                                {
                                    flag = 2;
                                    ResponseDescription = objStatus.dcCode + " - " + objStatus.dcDescription;
                                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                }
                            }

                        }

                        if (input.MSType == "2")
                        {
                            DCMaritalService.getMDinfoByCertifcateRefRequestType objRequestType = new DCMaritalService.getMDinfoByCertifcateRefRequestType();


                            if (input.DocumentType == "2")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoByCertifcateRefRequestTypeDocumentType.Attestation;
                            else if (input.DocumentType == "3")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoByCertifcateRefRequestTypeDocumentType.Divorce;
                            else if (input.DocumentType == "4")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoByCertifcateRefRequestTypeDocumentType.Marriage;

                            if (input.DocumetIssuerAuthority == "1")
                                objRequestType.DocumetIssuerAuthority = DCMaritalService.getMDinfoByCertifcateRefRequestTypeDocumetIssuerAuthority.Court;
                            else if (input.DocumetIssuerAuthority == "2")
                                objRequestType.DocumetIssuerAuthority = DCMaritalService.getMDinfoByCertifcateRefRequestTypeDocumetIssuerAuthority.MarriageOfficialsCourts;
                            else if (input.DocumetIssuerAuthority == "3")
                                objRequestType.DocumetIssuerAuthority = DCMaritalService.getMDinfoByCertifcateRefRequestTypeDocumetIssuerAuthority.MarriageOfficialsOthers;
                            else if (input.DocumetIssuerAuthority == "4")
                                objRequestType.DocumetIssuerAuthority = DCMaritalService.getMDinfoByCertifcateRefRequestTypeDocumetIssuerAuthority.Old;

                            objRequestType.DocumentNumber = input.DocumentNumber;
                            objRequestType.DocumentYear = input.DocumentYear;

                            DCMaritalService.getMDinfoByCertifcateRefRequest objRequest = new DCMaritalService.getMDinfoByCertifcateRefRequest();
                            objRequest.getMDinfoByCertifcateRef = objRequestType;

                            DCMaritalService.getMDinfoByCertifcateRefResponseType objResponseType = null;
                            DCMaritalService.outHeaderType objoutHeaderType = client.getMDinfoByCertifcateRef(objHeaderType, objRequestType, out objResponseType);
                            if (objResponseType != null && objResponseType.result != null)
                            {
                                DCMaritalService.getMDinfoByCertifcateRefResponseTypeResult objResult = objResponseType.result;
                                DCMaritalService.dcrequeststatusType objStatus = objResult.mdwsresponsefifthoptallType.requeststatus;
                                if (objStatus.dcCode == "DC-200")
                                {
                                    foreach (DCMaritalService.mdwsrecType i in objResult.mdwsresponsefifthoptallType.response)
                                    {
                                        objDCMARITALResponse = new DCMARITALDetails.DCMARITALResponse();
                                        objDCMARITALResponse.DocumentType = i.DocumentType;
                                        objDCMARITALResponse.CertifcateRef = i.CertifcateRef;
                                        objDCMARITALResponse.GregorianDate = i.GregorianDate;
                                        objDCMARITALResponse.Party1Name = i.Party1Name;
                                        objDCMARITALResponse.Party1UID = i.Party1UID;
                                        objDCMARITALResponse.Party1Nationality = i.Party1Nationality;
                                        objDCMARITALResponse.Party1BirthDate = i.Party1BirthDate;
                                        objDCMARITALResponse.Party2Name = i.Party2Name;
                                        objDCMARITALResponse.Party2UID = i.Party2UID;
                                        objDCMARITALResponse.Party2Nationality = i.Party2Nationality;
                                        objDCMARITALResponse.Party2BirthDate = i.Party2BirthDate;
                                        objDCMARITALResponse.BinaryFile = string.Empty;
                                        lstDCMARITALResponse.Add(objDCMARITALResponse);
                                    }

                                }
                                else
                                {
                                    flag = 2;
                                    ResponseDescription = objStatus.dcCode + " - " + objStatus.dcDescription;
                                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                }
                            }


                        }
                    }
                    else if (input.MSType == "3")
                    {
                        DCMaritalService.getMDinfoByDateRequestType objRequestType = new DCMaritalService.getMDinfoByDateRequestType();

                        if (input.DocumentType == "1")
                            objRequestType.DocumentType = DCMaritalService.getMDinfoByDateRequestTypeDocumentType.All;
                        else if (input.DocumentType == "2")
                            objRequestType.DocumentType = DCMaritalService.getMDinfoByDateRequestTypeDocumentType.Attestation;
                        else if (input.DocumentType == "3")
                            objRequestType.DocumentType = DCMaritalService.getMDinfoByDateRequestTypeDocumentType.Divorce;
                        else if (input.DocumentType == "4")
                            objRequestType.DocumentType = DCMaritalService.getMDinfoByDateRequestTypeDocumentType.Marriage;

                        objRequestType.fromDate = IEUtils.ToDate(input.FromDate);
                        objRequestType.toDate = IEUtils.ToDate(input.ToDate);

                        DCMaritalService.getMDinfoByDateRequest objRequest = new DCMaritalService.getMDinfoByDateRequest();
                        objRequest.getMDinfoByDate = objRequestType;
                        objRequest.inHeaderKey = objHeaderType;

                        DCMaritalService.getMDinfoByDateResponseType objResponseType = null;
                        DCMaritalService.outHeaderType objoutHeaderType = client.getMDinfoByDate(objHeaderType, objRequestType, out objResponseType);

                        if (objResponseType != null && objResponseType.result != null)
                        {
                            DCMaritalService.getMDinfoByDateResponseTypeResult objResult = objResponseType.result;
                            DCMaritalService.dcrequeststatusType objStatus = objResult.mdwsresponsefourthoptallType.requeststatus;
                            if (objStatus.dcCode == "DC-200")
                            {
                                foreach (DCMaritalService.mdwsrecType i in objResult.mdwsresponsefourthoptallType.response)
                                {
                                    objDCMARITALResponse = new DCMARITALDetails.DCMARITALResponse();
                                    objDCMARITALResponse.DocumentType = i.DocumentType;
                                    objDCMARITALResponse.CertifcateRef = i.CertifcateRef;
                                    objDCMARITALResponse.GregorianDate = i.GregorianDate;
                                    objDCMARITALResponse.Party1Name = i.Party1Name;
                                    objDCMARITALResponse.Party1UID = i.Party1UID;
                                    objDCMARITALResponse.Party1Nationality = i.Party1Nationality;
                                    objDCMARITALResponse.Party1BirthDate = i.Party1BirthDate;
                                    objDCMARITALResponse.Party2Name = i.Party2Name;
                                    objDCMARITALResponse.Party2UID = i.Party2UID;
                                    objDCMARITALResponse.Party2Nationality = i.Party2Nationality;
                                    objDCMARITALResponse.Party2BirthDate = i.Party2BirthDate;
                                    objDCMARITALResponse.BinaryFile = string.Empty;
                                    lstDCMARITALResponse.Add(objDCMARITALResponse);
                                }

                            }
                            else
                            {
                                flag = 2;
                                ResponseDescription = objStatus.dcCode + " - " + objStatus.dcDescription;
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }


                    }
                    else if (input.MSType == "4")
                    {
                        DCMaritalService.getMDinfoByEidaResponseType objResponseType = null;
                        DCMaritalService.getMDinfoByEidaRequestType objRequestType = new DCMaritalService.getMDinfoByEidaRequestType();

                        if (input.DocumentType == "1")
                            objRequestType.DocumentType = DCMaritalService.getMDinfoByEidaRequestTypeDocumentType.All;
                        else if (input.DocumentType == "2")
                            objRequestType.DocumentType = DCMaritalService.getMDinfoByEidaRequestTypeDocumentType.Attestation;
                        else if (input.DocumentType == "3")
                            objRequestType.DocumentType = DCMaritalService.getMDinfoByEidaRequestTypeDocumentType.Divorce;
                        else if (input.DocumentType == "4")
                            objRequestType.DocumentType = DCMaritalService.getMDinfoByEidaRequestTypeDocumentType.Marriage;

                        objRequestType.Party1UID = input.EmiratesIdHusband;
                        objRequestType.Party2UID = input.EmiratesIdWife;

                        DCMaritalService.getMDinfoByEidaRequest objRequest = new DCMaritalService.getMDinfoByEidaRequest();
                        objRequest.getMDinfoByEida = objRequestType;

                        DCMaritalService.outHeaderType objoutHeaderType = client.getMDinfoByEida(objHeaderType, objRequestType, out objResponseType);

                        if (objResponseType != null && objResponseType.result != null)
                        {
                            DCMaritalService.getMDinfoByEidaResponseTypeResult objResult = objResponseType.result;
                            DCMaritalService.dcrequeststatusType objStatus = objResult.mdwsresponsefirstoptallType.requeststatus;
                            if (objStatus.dcCode == "DC-200")
                            {
                                foreach (DCMaritalService.mdwsrecType i in objResult.mdwsresponsefirstoptallType.response)
                                {
                                    objDCMARITALResponse = new DCMARITALDetails.DCMARITALResponse();
                                    objDCMARITALResponse.DocumentType = i.DocumentType;
                                    objDCMARITALResponse.CertifcateRef = i.CertifcateRef;
                                    objDCMARITALResponse.GregorianDate = i.GregorianDate;
                                    objDCMARITALResponse.Party1Name = i.Party1Name;
                                    objDCMARITALResponse.Party1UID = i.Party1UID;
                                    objDCMARITALResponse.Party1Nationality = i.Party1Nationality;
                                    objDCMARITALResponse.Party1BirthDate = i.Party1BirthDate;
                                    objDCMARITALResponse.Party2Name = i.Party2Name;
                                    objDCMARITALResponse.Party2UID = i.Party2UID;
                                    objDCMARITALResponse.Party2Nationality = i.Party2Nationality;
                                    objDCMARITALResponse.Party2BirthDate = i.Party2BirthDate;
                                    objDCMARITALResponse.BinaryFile = string.Empty;
                                    lstDCMARITALResponse.Add(objDCMARITALResponse);
                                }

                            }
                            else
                            {
                                flag = 2;
                                ResponseDescription = objStatus.dcCode + " - " + objStatus.dcDescription;
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }

                    }
                    else if (input.MSType == "5" || input.MSType == "6")
                    {
                        if (input.MSType == "5")
                        {
                            DCMaritalService.getMDinfoByFullNameRequestType objRequestType = new DCMaritalService.getMDinfoByFullNameRequestType();

                            if (input.DocumentType == "1")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoByFullNameRequestTypeDocumentType.All;
                            else if (input.DocumentType == "2")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoByFullNameRequestTypeDocumentType.Attestation;
                            else if (input.DocumentType == "3")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoByFullNameRequestTypeDocumentType.Divorce;
                            else if (input.DocumentType == "4")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoByFullNameRequestTypeDocumentType.Marriage;

                            objRequestType.Party1Name = input.HusbandName;
                            objRequestType.Party2Name = input.WifeName;

                            DCMaritalService.getMDinfoByFullNameRequest objRequest = new DCMaritalService.getMDinfoByFullNameRequest();
                            objRequest.getMDinfoByFullName = objRequestType;
                            objRequest.inHeaderKey = objHeaderType;

                            DCMaritalService.getMDinfoByFullNameResponseType objResponseType = null;

                            DCMaritalService.outHeaderType objoutHeaderType = client.getMDinfoByFullName(objHeaderType, objRequestType, out objResponseType);

                            if (objResponseType != null && objResponseType.result != null)
                            {
                                DCMaritalService.getMDinfoByFullNameResponseTypeResult objResult = objResponseType.result;
                                DCMaritalService.dcrequeststatusType objStatus = objResult.mdwsresponsesecondoptallType.requeststatus;
                                if (objStatus.dcCode == "DC-200")
                                {
                                    foreach (DCMaritalService.mdwsrecType i in objResult.mdwsresponsesecondoptallType.response)
                                    {
                                        objDCMARITALResponse = new DCMARITALDetails.DCMARITALResponse();
                                        objDCMARITALResponse.DocumentType = i.DocumentType;
                                        objDCMARITALResponse.CertifcateRef = i.CertifcateRef;
                                        objDCMARITALResponse.GregorianDate = i.GregorianDate;
                                        objDCMARITALResponse.Party1Name = i.Party1Name;
                                        objDCMARITALResponse.Party1UID = i.Party1UID;
                                        objDCMARITALResponse.Party1Nationality = i.Party1Nationality;
                                        objDCMARITALResponse.Party1BirthDate = i.Party1BirthDate;
                                        objDCMARITALResponse.Party2Name = i.Party2Name;
                                        objDCMARITALResponse.Party2UID = i.Party2UID;
                                        objDCMARITALResponse.Party2Nationality = i.Party2Nationality;
                                        objDCMARITALResponse.Party2BirthDate = i.Party2BirthDate;
                                        objDCMARITALResponse.BinaryFile = string.Empty;
                                        lstDCMARITALResponse.Add(objDCMARITALResponse);
                                    }

                                }
                                else
                                {
                                    flag = 2;
                                    ResponseDescription = objStatus.dcCode + " - " + objStatus.dcDescription;
                                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                }
                            }

                        }

                        if (input.MSType == "6")
                        {
                            DCMaritalService.getMDinfoBySimilarNameRequestType objRequestType = new DCMaritalService.getMDinfoBySimilarNameRequestType();

                            if (input.DocumentType == "1")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoBySimilarNameRequestTypeDocumentType.All;
                            else if (input.DocumentType == "2")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoBySimilarNameRequestTypeDocumentType.Attestation;
                            else if (input.DocumentType == "3")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoBySimilarNameRequestTypeDocumentType.Divorce;
                            else if (input.DocumentType == "4")
                                objRequestType.DocumentType = DCMaritalService.getMDinfoBySimilarNameRequestTypeDocumentType.Marriage;

                            objRequestType.Party1Name = input.HusbandName;
                            objRequestType.Party2Name = input.WifeName;

                            DCMaritalService.getMDinfoBySimilarNameRequest objRequest = new DCMaritalService.getMDinfoBySimilarNameRequest();
                            objRequest.getMDinfoBySimilarName = objRequestType;
                            objRequest.inHeaderKey = objHeaderType;

                            DCMaritalService.getMDinfoBySimilarNameResponseType objResponseType = null;
                            DCMaritalService.outHeaderType objoutHeaderType = client.getMDinfoBySimilarName(objHeaderType, objRequestType, out objResponseType);
                            if (objResponseType != null && objResponseType.result != null)
                            {
                                DCMaritalService.getMDinfoBySimilarNameResponseTypeResult objResult = objResponseType.result;
                                DCMaritalService.dcrequeststatusType objStatus = objResult.mdwsresponsethirdoptallType.requeststatus;
                                if (objStatus.dcCode == "DC-200")
                                {
                                    foreach (DCMaritalService.mdwsrecType i in objResult.mdwsresponsethirdoptallType.response)
                                    {
                                        objDCMARITALResponse = new DCMARITALDetails.DCMARITALResponse();
                                        objDCMARITALResponse.DocumentType = i.DocumentType;
                                        objDCMARITALResponse.CertifcateRef = i.CertifcateRef;
                                        objDCMARITALResponse.GregorianDate = i.GregorianDate;
                                        objDCMARITALResponse.Party1Name = i.Party1Name;
                                        objDCMARITALResponse.Party1UID = i.Party1UID;
                                        objDCMARITALResponse.Party1Nationality = i.Party1Nationality;
                                        objDCMARITALResponse.Party1BirthDate = i.Party1BirthDate;
                                        objDCMARITALResponse.Party2Name = i.Party2Name;
                                        objDCMARITALResponse.Party2UID = i.Party2UID;
                                        objDCMARITALResponse.Party2Nationality = i.Party2Nationality;
                                        objDCMARITALResponse.Party2BirthDate = i.Party2BirthDate;
                                        objDCMARITALResponse.BinaryFile = string.Empty;
                                        lstDCMARITALResponse.Add(objDCMARITALResponse);
                                    }

                                }
                                else
                                {
                                    flag = 2;
                                    ResponseDescription = objStatus.dcCode + " - " + objStatus.dcDescription;
                                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                                }
                            }

                        }
                    }

                    if (lstDCMARITALResponse.Count > 0)
                    {
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstDCMARITALResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<DCMARITALDetails.DCMARITALResponse>>(lstDCMARITALResponse), ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        ResponseDescription = ResponseDescription == string.Empty ? "No Matchng Record Available" : ResponseDescription;
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }

            }
            catch (FaultException fex)
            {
                var fault = fex.CreateMessageFault();
                var doc = new XmlDocument();
                var innerdoc = new XmlDocument();
                var innersdoc = new XmlDocument();
                var nav = doc.CreateNavigator();

                string ResponseDescription = string.Empty;

                flag = 3;

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
                                    str += "Contact GSB Support.";
                                }
                            }

                            if (child.Name == "Detail")
                            {
                                //innerdoc.LoadXml(child.InnerXml);
                                //foreach (XmlNode chd in innerdoc.DocumentElement.ChildNodes)
                                //{
                                //    if (chd.Name == "arDesc")
                                //    {
                                //        str += chd.InnerText + " - ";
                                //    }
                                //    if (chd.Name == "enDesc")
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
                                ResponseDescription = child.InnerXml;
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }); ;
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    json = JsonConvert.SerializeObject(new { fault.Reason, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }); ;
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.StackTrace;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DCMARITALCode"].ToString(), ConfigurationManager.AppSettings["DCMARITAL"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}