using MOCDIntegrations.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static MOCDIntegrations.Models.RAKCourtsMarriageDetailsModel;
using System.Xml.Serialization;
using System.Xml;
using System.Configuration;

namespace MOCDIntegrations.Controllers
{
    public class RAKCourtsDivorceDetailsController : Controller
    {
        // GET: RAKDivorceDetails
        public ActionResult Index()
        {
            return View();
        }
        public static HttpWebRequest CreateWebRequest(string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["Rak_M_api_url"].ToString());
            webRequest.Headers.Add(@"SOAPAction:" + action + "");
            webRequest.ContentType = "text/xml;charset=\"UTF-8\"";
            webRequest.Accept = "gzip,deflate";
            webRequest.Method = "POST";
            String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["Rak_M_client_id"].ToString() + ":" + ConfigurationManager.AppSettings["Rak_M_client_secret"].ToString()));
            webRequest.Headers["Authorization"] = "Basic " + authInfo;
            return webRequest;
        }
        public ActionResult Search(string EmiratesId, string PassportNo, string Type, string UserAgent)
        {
            var json = "";
            int flag = 0;
            //EmirateID = "784198300001153";
            DataTable md = new DataTable();
            List<DocucmentDetailsReponse> listDocument = new List<DocucmentDetailsReponse>();

            try
            {
                if (EmiratesId.Length == 15 && Type=="2")
                {
                    EmiratesId = FormatEmiratesIdNumber(EmiratesId);
                }
               
                string soapResult = string.Empty;
                HttpWebRequest request = CreateWebRequest("http://sap.com/xi/WebService/soap1.1");
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                request.UseDefaultCredentials = true;
                request.PreAuthenticate = true;
                string @body = CreateSoapEnvelope(Type, EmiratesId, PassportNo);
                byte[] byteArray = Encoding.UTF8.GetBytes(@body);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        soapResult = reader.ReadToEnd();

                        RAKDivorceDetailsModel.RAKCourtsDivorceEnvelope.Envelope envelope = FromXml<RAKDivorceDetailsModel.RAKCourtsDivorceEnvelope.Envelope>(soapResult);
                        var divourceDetails = envelope.Body.MT_DivorceDetails_Res.Divorce.ToList();

                        if (divourceDetails.Count > 0)
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { divourceDetails, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                            // LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["RAKDivorceCode"].ToString(), ConfigurationManager.AppSettings["RAKDivorce"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Record Found";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
                            // LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["RAKDivorceCode"].ToString(), ConfigurationManager.AppSettings["RAKDivorce"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }

                    }

                }

            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["RAKDivorceCode"].ToString(), ConfigurationManager.AppSettings["RAKDivorce"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["RAKDivorceCode"].ToString(), ConfigurationManager.AppSettings["RAKDivorce"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public static string FormatEmiratesIdNumber(string input)
        {
            if (input.Length != 15)
                throw new FormatException("Emirates Id  number. Must be 15 characters");

            return String.Format("{0}-{1}-{2}-{3}",
                             input.Substring(0, 3),
                             input.Substring(3, 4),
                             input.Substring(7, 7),
                             input.Substring(14, 1));


        }
        //public ActionResult ViewMoreDetails(string transactionId, string UserAgent)
        //{
        //    List<DocucmentDetailsReponse> listDocument = new List<DocucmentDetailsReponse>();
        //    RAKCourtsMarriageDetailsModel.DocumentEnvelope.Root objResponse = new RAKCourtsMarriageDetailsModel.DocumentEnvelope.Root();

        //    var json = "";
        //    int flag = 0;
        //    try
        //    {
        //        objResponse = CallMarrigeDocumentWebService(transactionId);
        //        if (objResponse.SOAPEnvelope?.SOAPBody?.ns1MT_ContractDocuments_Res?.Attachment?.item != null)
        //        {
        //            objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Attachment.item.ZTransactionId = transactionId;
        //            listDocument.Add(new DocucmentDetailsReponse
        //            {
        //                ZTransactionId = transactionId,
        //                ZDocTypeCode = objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Attachment.item.ZDocTypeCode,
        //                ZDocArabicDesc = objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Attachment.item.ZDocArabicDesc,
        //                ZDocEnglishDesc = objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Attachment.item.ZDocEnglishDesc,
        //                ZDocName = objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Attachment.item.ZDocName,
        //                ZDocCreationAt = objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Attachment.item.ZDocCreationAt,
        //                ZDocument = objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Attachment.item.ZDocument,
        //                Response_Code = objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Response_Code.Response_Code,
        //                Response_Description_Ar = objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Response_Code.Response_Description_Ar,
        //                Response_Description_En = objResponse.SOAPEnvelope.SOAPBody.ns1MT_ContractDocuments_Res.Response_Code.Response_Description_En,
        //            });
        //            if (listDocument.Count > 0)
        //            {
        //                flag = 1;
        //                json = JsonConvert.SerializeObject(new { listDocument, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
        //                // LogIntegrationDetails.LogSerilog(transactionId, json, ConfigurationManager.AppSettings["RAKDivorceCode"].ToString(), ConfigurationManager.AppSettings["RAKDivorce"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

        //            }
        //            else
        //            {
        //                flag = 2;
        //                string ResponseDescription = "No Matching Record Found";
        //                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
        //                // LogIntegrationDetails.LogSerilog(transactionId, ResponseDescription, ConfigurationManager.AppSettings["RAKDivorceCode"].ToString(), ConfigurationManager.AppSettings["RAKDivorce"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

        //            }
        //        }
        //        else
        //        {
        //            flag = 2;
        //            string ResponseDescription = "No Matching Record Found";
        //            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-mm-dd hh:mm:ss" });
        //            // LogIntegrationDetails.LogSerilog(transactionId, ResponseDescription, ConfigurationManager.AppSettings["RAKDivorceCode"].ToString(), ConfigurationManager.AppSettings["RAKDivorce"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        flag = 3;

        //        //var resp = new StreamReader(ex.Message).ReadToEnd();
        //        string ResponseDescription = ex.Message.ToString();
        //        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        //        LogIntegrationDetails.LogSerilog(transactionId, ResponseDescription, ConfigurationManager.AppSettings["RAKDivorceCode"].ToString(), ConfigurationManager.AppSettings["RAKDivorce"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = 3;
        //        //var resp = new StreamReader(ex.Message).ReadToEnd();
        //        string ResponseDescription = ex.Message.ToString();
        //        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        //        LogIntegrationDetails.LogSerilog(transactionId, ResponseDescription, ConfigurationManager.AppSettings["RAKDivorceCode"].ToString(), ConfigurationManager.AppSettings["RAKDivorce"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
        //    }
        //    return Json(json, JsonRequestBehavior.AllowGet);

        //}


        private static T FromXml<T>(string soapResult)
        {
            TextReader reader = new StringReader(soapResult);
            return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
        }

        //public static RAKCourtsMarriageDetailsModel.DocumentEnvelope.Root CallMarrigeDocumentWebService(string TransactionID)
        //{
        //    TransactionID = "2020600331";
        //    DataTable dt = new DataTable();
        //    RAKCourtsMarriageDetailsModel.DocumentEnvelope.Root objResponse;
        //    try
        //    {
        //        //TransactionID = "2022600261";
        //        string soapResult = string.Empty;
        //        HttpWebRequest request = CreateWebRequest("SI_ContractDocuments_OB");
        //        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        //        request.UseDefaultCredentials = true;
        //        request.PreAuthenticate = true;
        //        XmlDocument soapEnvelopeXml = new XmlDocument();
        //        string @body = CreateSoapEnvelope("marriageDoc", TransactionID);
        //        byte[] byteArray = Encoding.UTF8.GetBytes(@body);

        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(byteArray, 0, byteArray.Length);
        //        }

        //        using (WebResponse response = request.GetResponse())
        //        {
        //            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                soapResult = reader.ReadToEnd();
        //                StringReader StringStream = new StringReader(soapResult);

        //                XmlDocument doc = new XmlDocument();
        //                doc.LoadXml(soapResult);

        //                string jsonNew = JsonConvert.SerializeXmlNode(doc);
        //                if (jsonNew.Contains("\"Response_Code\":\"005\""))
        //                {
        //                    return objResponse = JsonConvert.DeserializeObject<RAKCourtsMarriageDetailsModel.DocumentEnvelope.Root>(jsonNew);

        //                }



        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return new RAKCourtsMarriageDetailsModel.DocumentEnvelope.Root();
        //}
        public static DataSet FillDataSet_FromXML(string xmlData)
        {
            try
            {
                StringReader theReader = new StringReader(xmlData);
                DataSet theDataSet = new DataSet();
                if (theReader != null)
                {
                    theDataSet.ReadXml(theReader);
                    return theDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        private static string CreateSoapEnvelope(string soap_type/* = "divourceDetails"*/, string EmiratesId/* = "784-1983-0000115-3"*/, string Passport)
        {
            string soapEnvelopeDocument = "";
            switch (soap_type)
            {
                case "2":
                    soapEnvelopeDocument =
                    @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:mar=""http://ega.rak.ae/xi/MarriageandDivorceInquiry"">
   <soapenv:Header/>
   <soapenv:Body>
      <mar:MT_DivorceDetails>
         <!--Optional:-->
         <Search>
            <!--Optional:-->
            <ZContractNum></ZContractNum>
            <!--Optional:-->
            <ZContractYear></ZContractYear>
            <!--Optional:-->
            <ZPersonEmiratesID>" + EmiratesId + @"</ZPersonEmiratesID>
            <!--Optional:-->
            <ZPassportNumber></ZPassportNumber>
            <!--Optional:-->
            <ZPassportCountry></ZPassportCountry>
            <!--Optional:-->
            <ZDivorceDateFrom></ZDivorceDateFrom>
            <!--Optional:-->
            <ZDivorceDateTo></ZDivorceDateTo>
         </Search>
      </mar:MT_DivorceDetails>
   </soapenv:Body>
</soapenv:Envelope>";
                    break;
                case "3":   // need to change the soap
                    soapEnvelopeDocument =
                      @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:mar=""http://ega.rak.ae/xi/MarriageandDivorceInquiry"">
   <soapenv:Header/>
   <soapenv:Body>
      <mar:MT_DivorceDetails>
         <!--Optional:-->
         <Search>
            <!--Optional:-->
            <ZContractNum></ZContractNum>
            <!--Optional:-->
            <ZContractYear></ZContractYear>
            <!--Optional:-->
            <ZPersonEmiratesID></ZPersonEmiratesID>
            <!--Optional:-->
            <ZPassportNumber>" + Passport + @"</ZPassportNumber>
            <!--Optional:-->
            <ZPassportCountry></ZPassportCountry>
            <!--Optional:-->
            <ZDivorceDateFrom></ZDivorceDateFrom>
            <!--Optional:-->
            <ZDivorceDateTo></ZDivorceDateTo>
         </Search>
      </mar:MT_DivorceDetails>
   </soapenv:Body>
</soapenv:Envelope>";
                    break;

                default:
                    break;
            }
            return soapEnvelopeDocument;

        }

    }
}