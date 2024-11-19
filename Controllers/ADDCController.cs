using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MOCDIntegrations.Models;
using System.Configuration;
using Newtonsoft.Json.Converters;
using System.Data.Odbc;
using System.ServiceModel;

namespace MOCDIntegrations.Controllers
{
    public class ADDCController : Controller
    {
        // GET: ADDC
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            string DATA = "";
            ADDCDetails.Return objResponse = new ADDCDetails.Return();
            try
            {

                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                DATA = EmiratesId; // "784194025164941";//
                if (DATA.Length == 15)
                {
                    string respList = CallWebService(DATA, ConfigurationManager.AppSettings["ADDC_Url"].ToString(), "urn:ae:gov:abudhabi:RealEstateAndHousing:HousingTypes:1", "searchAppByEmirateId");
                    if (respList != "")
                    {
                        DataSet DataSetList = FillDataSet_FromXML(respList, "getAttestationList");
                        if (DataSetList.Tables.Count > 0)
                        {
                            if (DataSetList.Tables.Contains("return"))
                            {
                                if (DataSetList.Tables["return"].Rows[0].Table.Columns.Contains("caseCreationDate"))
                                {
                                    objResponse.CaseCreationDate = DataSetList.Tables["return"].Rows[0]["caseCreationDate"].ToString();
                                }

                                if (DataSetList.Tables["return"].Rows[0].Table.Columns.Contains("caseNo"))
                                {
                                    objResponse.CaseNo = DataSetList.Tables["return"].Rows[0]["caseNo"].ToString();

                                }

                                if (DataSetList.Tables["return"].Rows[0].Table.Columns.Contains("caseSource"))
                                {
                                    objResponse.CaseSource = DataSetList.Tables["return"].Rows[0]["caseSource"].ToString();

                                }

                                if (DataSetList.Tables["return"].Rows[0].Table.Columns.Contains("caseStatus"))
                                {
                                    objResponse.CaseStatus = DataSetList.Tables["return"].Rows[0]["caseStatus"].ToString();

                                }

                                if (DataSetList.Tables["return"].Rows[0].Table.Columns.Contains("caseSubType"))
                                {
                                    objResponse.CaseSubType = DataSetList.Tables["return"].Rows[0]["caseSubType"].ToString();

                                }

                                if (DataSetList.Tables["return"].Rows[0].Table.Columns.Contains("caseType"))
                                {

                                    objResponse.CaseType = DataSetList.Tables["return"].Rows[0]["caseType"].ToString();

                                }

                                if (DataSetList.Tables["return"].Rows[0].Table.Columns.Contains("partyName"))
                                {
                                    objResponse.PartyName = DataSetList.Tables["return"].Rows[0]["partyName"].ToString();

                                }
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { objResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["ADDCCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                            }
                            else
                            {

                                flag = 2;
                                string ResponseDescription = "No Matching Records available";
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADDCCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }

                    }
                    else
                    {

                        flag = 2;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADDCCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }


            }

            catch (FaultException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADDCCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (WebException ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["ADDCCode"].ToString(), ConfigurationManager.AppSettings["ADDC"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public static string CallWebService(string EmiratesId, string url, string actionname, string servicename, string Documentnumber = default)
        {
            string soapEnvelopeXml = "";
            soapEnvelopeXml = CreateSoapEnvelope(EmiratesId);

            HttpWebRequest webRequest = CreateWebRequest(url, actionname);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            string soapResult;
            WebResponse webResponse;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            try
            {
                using (webResponse = webRequest.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }
                    //Console.Write(soapResult);
                }
            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }

                return "";
            }

            return soapResult;
        }
        private static string CreateSoapEnvelope(string EmiratesId)
        {
            string soapEnvelopeDocument = "";
            try
            {
                soapEnvelopeDocument =
            @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ser=""http://service.adjd.gov/"">
               <soapenv:Header/>
               <soapenv:Body>
                  <ser:getDefendantCases>
                    <emiratesIDNo>" + EmiratesId + @"</emiratesIDNo>
                  </ser:getDefendantCases>
               </soapenv:Body>
            </soapenv:Envelope>";
                return soapEnvelopeDocument;
            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }

                return "";
            }

        }
        private static void InsertSoapEnvelopeIntoWebRequest(string soapEnvelopeXml, HttpWebRequest webRequest)
        {
            try
            {
                using (Stream webStream = webRequest.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
                {
                    requestWriter.Write(soapEnvelopeXml);
                }
                using (Stream webStream = webRequest.GetRequestStream())
                    try
                    {
                        using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.UTF8))
                        {
                            requestWriter.Write(soapEnvelopeXml);
                        }
                    }
                    catch (Exception)
                    {
                    }

            }
            catch (WebException ex)
            {
                string APIResponse = "";
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        APIResponse = reader.ReadToEnd();
                    }
                }
            }

        }

        public static DataSet FillDataSet_FromXML(string xmlData, string servicename)
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
                return null;
            }

        }
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add(@"SOAPAction:" + action + "");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["ADDC_client_id"].ToString() + ":" + ConfigurationManager.AppSettings["ADDC_client_secret"].ToString()));
                webRequest.Headers["Authorization"] = "Basic " + authInfo;
                return webRequest;


            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //private static void SaveADDCDatat(DataSet DRitem, string EmirateID)
        //{
        //    SqlConnection connection;
        //    if (DRitem != null)
        //    {
        //        if (DRitem.Tables.Count > 0)
        //        {
        //            DataTable dt = new DataTable();

        //            using (connection = new SqlConnection(ConfigurationManager.AppSettings["SERVICE_Save"].ToString()))
        //            {
        //                try
        //                {
        //                    connection.Open();
        //                    using (SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings["InsertInto"].ToString(), connection))
        //                    {
        //                        command.CommandType = CommandType.StoredProcedure;
        //                        command.Parameters.Clear();
        //                        if (DRitem.Tables.Contains("return"))
        //                        {
        //                            if (DRitem.Tables["return"].Rows[0].Table.Columns.Contains("caseCreationDate"))
        //                            {
        //                                command.Parameters.AddWithValue("@caseCreationDate", SqlDbType.NVarChar).Value = DRitem.Tables["return"].Rows[0]["caseCreationDate"] != DBNull.Value ? DRitem.Tables["return"].Rows[0]["caseCreationDate"].ToString() : "";
        //                            }
        //                            else
        //                            {
        //                                command.Parameters.AddWithValue("@caseCreationDate", SqlDbType.NVarChar).Value = "";
        //                            }
        //                            if (DRitem.Tables["return"].Rows[0].Table.Columns.Contains("caseNo"))
        //                            {
        //                                command.Parameters.AddWithValue("@caseNo", SqlDbType.NVarChar).Value = DRitem.Tables["return"].Rows[0]["caseNo"] != DBNull.Value ? DRitem.Tables["return"].Rows[0]["caseNo"].ToString() : "";
        //                            }
        //                            else
        //                            {
        //                                command.Parameters.AddWithValue("@caseNo", SqlDbType.NVarChar).Value = "";
        //                            }
        //                            if (DRitem.Tables["return"].Rows[0].Table.Columns.Contains("caseSource"))
        //                            {
        //                                command.Parameters.AddWithValue("@caseSource", SqlDbType.NVarChar).Value = DRitem.Tables["return"].Rows[0]["caseSource"] != DBNull.Value ? DRitem.Tables["return"].Rows[0]["caseSource"].ToString() : "";
        //                            }
        //                            else
        //                            {
        //                                command.Parameters.AddWithValue("@caseSource", SqlDbType.NVarChar).Value = "";
        //                            }
        //                            if (DRitem.Tables["return"].Rows[0].Table.Columns.Contains("caseStatus"))
        //                            {
        //                                command.Parameters.AddWithValue("@caseStatus", SqlDbType.NVarChar).Value = DRitem.Tables["return"].Rows[0]["caseStatus"] != DBNull.Value ? DRitem.Tables["return"].Rows[0]["caseStatus"].ToString() : "";
        //                            }
        //                            else
        //                            {
        //                                command.Parameters.AddWithValue("@caseStatus", SqlDbType.NVarChar).Value = "";
        //                            }
        //                            if (DRitem.Tables["return"].Rows[0].Table.Columns.Contains("caseSubType"))
        //                            {
        //                                command.Parameters.AddWithValue("@caseSubType", SqlDbType.NVarChar).Value = DRitem.Tables["return"].Rows[0]["caseSubType"] != DBNull.Value ? DRitem.Tables["return"].Rows[0]["caseSubType"].ToString() : "";
        //                            }
        //                            else
        //                            {
        //                                command.Parameters.AddWithValue("@caseSubType", SqlDbType.NVarChar).Value = "";
        //                            }
        //                            if (DRitem.Tables["return"].Rows[0].Table.Columns.Contains("caseType"))
        //                            {
        //                                command.Parameters.AddWithValue("@caseType", SqlDbType.NVarChar).Value = DRitem.Tables["return"].Rows[0]["caseType"] != DBNull.Value ? DRitem.Tables["return"].Rows[0]["caseType"].ToString() : "";
        //                            }
        //                            else
        //                            {
        //                                command.Parameters.AddWithValue("@caseType", SqlDbType.NVarChar).Value = "";
        //                            }
        //                            if (DRitem.Tables["return"].Rows[0].Table.Columns.Contains("partyName"))
        //                            {
        //                                command.Parameters.AddWithValue("@partyName", SqlDbType.NVarChar).Value = DRitem.Tables["return"].Rows[0]["partyName"] != DBNull.Value ? DRitem.Tables["return"].Rows[0]["partyName"].ToString() : "";
        //                            }
        //                            else
        //                            {
        //                                command.Parameters.AddWithValue("@partyName", SqlDbType.NVarChar).Value = "";
        //                            }
        //                            command.Parameters.AddWithValue("@EmiratesID", SqlDbType.NVarChar).Value = EmirateID;

        //                            command.CommandTimeout = 0;
        //                            command.ExecuteNonQuery();
        //                        }
        //                        else
        //                        {
        //                            CommonHelper.CommonFunctions.GetInstance().ErrorLogs(connection, "ADDC", "SaveADDCDetails", "Table not Exists in the Response, No Record Found", EmirateID, "", "");
        //                        }

        //                    }
        //                    connection.Close();
        //                }
        //                catch (Exception ex)
        //                {
        //                    CommonHelper.CommonFunctions.GetInstance().ErrorLogs(connection, "ADDC", "SaveADDCDetails", "" + ex.Message.ToString() + "", EmirateID, "", "");
        //                }
        //            }
        //        }
        //    }
        //}
        public static TokenDetails GenerateAndGetToken(string uri, string grant_type, string client_id, string client_secret, string scope)
        {
            TokenDetails objToken = new TokenDetails();
            StringBuilder tokenUri = new StringBuilder();
            tokenUri.Append(uri);
            tokenUri.AppendFormat("?grant_type=" + grant_type + "&client_id=" + client_id + "&client_secret=" + client_secret + "&scope=" + scope);
            String responseBody;

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUri.ToString());
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader responseStream = new StreamReader(response.GetResponseStream());
                responseBody = responseStream.ReadToEnd();
                objToken = JsonConvert.DeserializeObject<TokenDetails>(responseBody);
                return objToken;
            }
            catch (WebException ex)
            {
                StreamReader responseStream = new StreamReader(ex.Response.GetResponseStream());
                responseBody = responseStream.ReadToEnd();
                objToken = JsonConvert.DeserializeObject<TokenDetails>(responseBody);
                return objToken;
            }
        }
        private static string GenerateToken()
        {
            try
            {
                string Result = GenerateAndGetToken(ConfigurationManager.AppSettings["Uri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["client_id"].ToString(), ConfigurationManager.AppSettings["client_secret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString()).access_token;
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}