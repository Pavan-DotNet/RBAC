using MOCDIntegrations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using MOCDIntegrations.SCMAFBeneficiary;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Microsoft.ClearScript;
using MOCDIntegrations.SCMAF;
using System.Web.Script.Serialization;
using static MOCDIntegrations.Controllers.SCMAFModel;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Remoting;

namespace MOCDIntegrations.Controllers
{
    public class SCMAFController : Controller
    {
        // GET: SCMAF
        public ActionResult Index()
        {
            return View("SCMAF");
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            SCMAFResponseModel responseModel = new SCMAFResponseModel();

            try
            {



                // inheritorStatementInfoType.FileNumber = "33232320";

                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<ADDEDDetails.ADDEDDetailsRequestParams>(postdata);
                var statementListResponse = FetchStatementInfoList(input.EmiratesId);
                if (statementListResponse.InheritorStatementListResponse.ResponseStatus.StatusCode != "SCMAF-ERROR-01")
                {
                    var documentDetails = FetchStatementDocument("");
                    var r = responseModel.AttachmentDetailsType;

                    if (documentDetails.InheritorStatementDocumentResponse.ResponseStatus.StatusCode != "SCMAF-ERROR-07")
                    {
                        json = documentDetails.InheritorStatementDocumentResponse.ResponseStatus.ToString();
                        var res = documentDetails.InheritorStatementDocumentResponse.AttachmentDetails;
                        foreach (var item in documentDetails.InheritorStatementDocumentResponse.AttachmentDetails)
                        {
                            responseModel.AttachmentDetailsType.Add(new AttachmentDetailsType
                            {
                                Comments = item.Comments,
                                DocType = item.DocType,
                                FileBuffer = item.FileBuffer,
                                FileExt = item.FileExt,
                                FileName = item.FileName,
                                FileSize = item.FileSize
                            });
                        }

                        // responseModel.AttachmentDetailsType = documentDetails.InheritorStatementDocumentResponse.AttachmentDetails;
                    }
                    var statementDetailsInfo = FetchStatementDetails("");
                    if (statementDetailsInfo.InheritorStatementDetailsResponse.ResponseStatus.StatusCode != "SCMAF-ERROR-07")
                    {
                        var res = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo;

                        responseModel.InheritorStatementInfoType.SRNumber = "";
                        responseModel.InheritorStatementInfoType = new InheritorStatementInfoType
                        {
                            BankAccountNo = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.BankAccountNo,
                            BankAccountOwnerNameEnglish = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.BankAccountOwnerNameEnglish,
                            BankAccountOwnerNameArabic = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.BankAccountOwnerNameArabic,
                            DateFrom = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.DateFrom,
                            DateTo = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.DateTo,
                            FileNumber = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.FileNumber,
                            FileOwnerNameArabic = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.FileOwnerNameArabic,
                            FileOwnerNameEnglish = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.FileOwnerNameEnglish,
                            InheritorNameArabic = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.InheritorNameArabic,
                            InheritorNameEnglish = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.InheritorNameEnglish,
                            InheritorNumber = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.InheritorNumber,
                            SRNumber = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo.SRNumber
                        };
                        responseModel.InheritorStatementInfoType = statementDetailsInfo.InheritorStatementDetailsResponse.Contents.InheritorStatementInfo;

                    }
                    flag = 1;
                    json = JsonConvert.SerializeObject(new { responseModel, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["SCMAFCode"].ToString(), ConfigurationManager.AppSettings["SCMAF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }
                else
                {
                    flag = 2;
                    string ResponseDescription = "No Matching Records Found";
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SCMAFCode"].ToString(), ConfigurationManager.AppSettings["SCMAF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                }

            }
            catch (WebException ex)
            {
                flag = 3;
                // var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SCMAFCode"].ToString(), ConfigurationManager.AppSettings["SCMAF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["SCMAFCode"].ToString(), ConfigurationManager.AppSettings["SCMAF"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static string GenerateToken()
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

        public static string CallWebService(string EmiratesId, string url, string actionname, string servicename)
        {
            string soapEnvelopeXml = "";
            if (servicename == "StatementList")
                soapEnvelopeXml = CreateSoapEnvelopeList(EmiratesId);

            else if (servicename == "StatementDetails")
                soapEnvelopeXml = CreateSoapEnvelopeDetails(EmiratesId);

            else
                soapEnvelopeXml = CreateSoapEnvelopeDocuments(EmiratesId);

            HttpWebRequest webRequest = CreateWebRequest(url, actionname);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            //IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            //// suspend this thread until call is complete. You might want to
            //// do something usefull here like update your UI.
            //asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            WebResponse webResponse;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            using (webResponse = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                //Console.Write(soapResult);
            }
            return soapResult;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add(@"SOAPAction:" + action + "");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            //webRequest.ContentLength = 873;
            String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["sclient_id"].ToString() + ":" + ConfigurationManager.AppSettings["sclient_secret"].ToString()));
            webRequest.Headers["Authorization"] = "Basic " + authInfo;
            return webRequest;
        }

        private static string CreateSoapEnvelopeList(string EmiratesId)
        {
            string soapEnvelopeDocument;
            soapEnvelopeDocument =
            @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:ae:gov:abudhabi:DigitalDocuments:InheritorStatementMessages:1"" xmlns:urn1=""urn:ae:gov:abudhabi:DigitalDocuments:SocialMinor:1"">
   <soapenv:Body>
      <urn:InheritorStatementListRequest>
         <urn:EmiratesID>" + EmiratesId + @"</urn:EmiratesID>
      </urn:InheritorStatementListRequest>
   </soapenv:Body>
</soapenv:Envelope>";
            return soapEnvelopeDocument;
        }

        private static string CreateSoapEnvelopeDetails(string srno)
        {
            string soapEnvelopeDocument;
            soapEnvelopeDocument =
            @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:ae:gov:abudhabi:DigitalDocuments:InheritorStatementMessages:1"" xmlns:urn1=""urn:ae:gov:abudhabi:DigitalDocuments:SocialMinor:1"">
   <soapenv:Body>
      <urn:InheritorStatementDetailsRequest>
         <urn:SRNumber>" + srno + @"</urn:SRNumber>
      </urn:InheritorStatementDetailsRequest>
   </soapenv:Body>
</soapenv:Envelope>";
            return soapEnvelopeDocument;
        }

        private static string CreateSoapEnvelopeDocuments(string srno)
        {
            string soapEnvelopeDocument;
            soapEnvelopeDocument =
            @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:ae:gov:abudhabi:DigitalDocuments:InheritorStatementMessages:1"" xmlns:urn1=""urn:ae:gov:abudhabi:DigitalDocuments:SocialMinor:1"">
   <soapenv:Body>
      <urn:InheritorStatementDocumentRequest>
         <urn:SRNumber>" + srno + @"</urn:SRNumber>
      </urn:InheritorStatementDocumentRequest>
   </soapenv:Body>
</soapenv:Envelope>";
            return soapEnvelopeDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(string soapEnvelopeXml, HttpWebRequest webRequest)
        {
            //webRequest.ContentLength = soapEnvelopeXml.Length;
            using (Stream webStream = webRequest.GetRequestStream())
            using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
            {
                requestWriter.Write(soapEnvelopeXml);
            }
            //    using (Stream stream = webRequest.GetRequestStream())
            //{
            //    soapEnvelopeXml.Save(stream);
            //}
        }

        public static DataSet stam(string xmlData, string servicename)
        {
            StringReader theReader = new StringReader(xmlData);
            DataSet theDataSet = new DataSet();
            DataSet theDataSets = new DataSet();
            theDataSet.ReadXml(theReader);
            if (servicename == "ListDetails")
            {
                if (theDataSet.Tables[4].Rows[0]["StatusCode"].ToString() == "SCMAF-SUCCESS-0001")
                {
                    theDataSets.Tables.Add(theDataSet.Tables[6].Copy());
                    theDataSets.Tables.Add(theDataSet.Tables[7].Copy());
                    return theDataSets;
                }
            }
            else if (servicename == "ListDocuments")
            {
                if (theDataSet.Tables[4].Rows[0]["StatusCode"].ToString() == "SCMAF-SUCCESS-0001")
                    return theDataSet;
            }
            else
                return null;
            return null;
        }

        private static getInheritorStatementListResponse FetchStatementInfoList(string emirateID)
        {
            SCMAF.InheritorStatementPortClient client = new SCMAF.InheritorStatementPortClient();
            SCMAF.InheritorStatementListRequestType request = new SCMAF.InheritorStatementListRequestType();
            SCMAF.getInheritorStatementListResponse response = new SCMAF.getInheritorStatementListResponse();
            SCMAF.RequestTrnHeaderType requestTrnHeader = new SCMAF.RequestTrnHeaderType();
            ConfigurationManager.AppSettings["Uri"].ToString();

            request.EmiratesID = emirateID;

            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                response = client.getInheritorStatementListAsync(requestTrnHeader, request).Result;
                return response;
            }
        }


        private static getInheritorStatementDetailsResponse FetchStatementDetails(string SRNumber)
        {
            SCMAF.InheritorStatementPortClient client = new SCMAF.InheritorStatementPortClient();
            SCMAF.InheritorStatementDetailsRequestType request = new SCMAF.InheritorStatementDetailsRequestType();
            SCMAF.getInheritorStatementDetailsResponse response = new SCMAF.getInheritorStatementDetailsResponse();
            SCMAF.RequestTrnHeaderType requestTrnHeader = new SCMAF.RequestTrnHeaderType();
            ConfigurationManager.AppSettings["Uri"].ToString();

            request.SRNumber = SRNumber;
            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                response = client.getInheritorStatementDetailsAsync(requestTrnHeader, request).Result;
                return response;
            }
        }
        private static getInheritorStatementDocumentResponse FetchStatementDocument(string SRNumber)
        {
            SCMAF.InheritorStatementPortClient client = new SCMAF.InheritorStatementPortClient();
            SCMAF.InheritorStatementDocumentRequestType request = new SCMAF.InheritorStatementDocumentRequestType();
            SCMAF.getInheritorStatementDocumentResponse response = new SCMAF.getInheritorStatementDocumentResponse();
            SCMAF.RequestTrnHeaderType requestTrnHeader = new SCMAF.RequestTrnHeaderType();
            ConfigurationManager.AppSettings["Uri"].ToString();

            request.SRNumber = SRNumber;
            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                response = client.getInheritorStatementDocumentAsync(requestTrnHeader, request).Result;
                return response;
            }
        }

    }
    public class SCMAFModel
    {
        public class SCMAFResponseModel
        {
            public List<AttachmentDetailsType> AttachmentDetailsType { get; set; }
            public InheritorStatementInfoType InheritorStatementInfoType { get; set; }
        }
    }
}