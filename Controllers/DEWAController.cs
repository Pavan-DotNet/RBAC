using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static MOCDIntegrations.Models.ADAFSADetails.ADAFSAResponse;

namespace MOCDIntegrations.Controllers
{
    public class DEWAController : Controller
    {
        // GET: DEWA
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



        public JsonResult Search(string stype, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            var Idtype = "";
            DataTable dt = null; DataTable dt1 = null;


            DEWADetails.DEWAResponse objDEWAResponse = null;
            List<DEWADetails.DEWAResponse> lstDEWAResponse = new List<DEWADetails.DEWAResponse>();

            DEWADetails.Address objAddresses = null;
            List<DEWADetails.Address> lstAddresses = new List<DEWADetails.Address>();

            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<DEWADetails.DEWARequest>(postdata);

                if (stype == "1")
                    Idtype = "02";
                else if (stype == "2")
                    Idtype = "01";
                else if (stype == "3")
                    Idtype = "04";
                else if (stype == "4")
                    Idtype = "00";

                string respList = CallWebService(input.EmiratesId, "GetPremiseDetails", "https://integrate.gsb.government.ae/ws/getPremiseAccountDetails_DEWA.getPremiseAccountDetails_DEWAsoaphttps/1.0", Idtype);
                DataSet DataSetList = FillDataSet_FromXML(respList, "GetPremiseDetails");
                if (DataSetList.Tables.Count > 0)
                {
                    if (DataSetList.Tables["record"] != null)
                    {
                        if (DataSetList != null)
                        {
                            if (DataSetList.Tables.Count > 0)
                            {
                                dt = DataSetList.Tables["record"];
                                if (DataSetList.Tables.Contains("record"))
                                {
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        objDEWAResponse = new DEWADetails.DEWAResponse();
                                        objDEWAResponse.inputIdType = row["InputIdType"].ToString() != null ? row["InputIdType"].ToString() : string.Empty;
                                        objDEWAResponse.inputIdNumber = row["InputIdNumber"].ToString() != null ? row["InputIdNumber"].ToString() : string.Empty;
                                        objDEWAResponse.premiseNo = row["PremiseNo"].ToString() != null ? row["PremiseNo"].ToString() : string.Empty;
                                        objDEWAResponse.premiseType = row["PremiseType"].ToString() != null ? row["PremiseType"].ToString() : string.Empty;
                                        objDEWAResponse.contractAccount = row["ContractAccount"].ToString() != null ? row["ContractAccount"].ToString() : string.Empty;
                                        objDEWAResponse.contractAccountType = row["ContractAccountType"].ToString() != null ? row["ContractAccountType"].ToString() : string.Empty;
                                        objDEWAResponse.ownerName = row["OwnerName"].ToString() != null ? row["OwnerName"].ToString() : string.Empty;
                                        objDEWAResponse.uaeNational = row["UaeNational"].ToString() != null ? row["UaeNational"].ToString() : string.Empty;
                                        objDEWAResponse.moveInDate = row["MoveInDate"].ToString() != null ? row["MoveInDate"].ToString() : string.Empty;
                                        objDEWAResponse.moveOutDate = row["MoveOutDate"].ToString() != null ? row["MoveOutDate"].ToString() : string.Empty;
                                        objDEWAResponse.makaniNumber = row["MakaniNumber"].ToString() != null ? row["MakaniNumber"].ToString() : string.Empty;
                                        objDEWAResponse.eidNumber = row["EidNumber"].ToString() != null ? row["EidNumber"].ToString() : string.Empty;
                                        objDEWAResponse.socialbenefit = row["SocialBenifit"].ToString() != null ? row["SocialBenifit"].ToString() : string.Empty;
                                        objDEWAResponse.communityNumber = row["CommunityNumber"].ToString() != null ? row["CommunityNumber"].ToString() : string.Empty;
                                        objDEWAResponse.billingCycle = row["BillingCycle"].ToString() != null ? row["BillingCycle"].ToString() : string.Empty;
                                        objDEWAResponse.recInflationAllowance = row["RecInflationAllowance"].ToString() != null ? row["RecInflationAllowance"].ToString() : string.Empty;

                                        if (DataSetList.Tables.Contains("Address"))
                                        {
                                            dt1 = DataSetList.Tables["Address"];
                                            objDEWAResponse.Address = new DEWADetails.Address();
                                            DEWAService.GetPremiseDetailsRespTypeAddress objAddress = new DEWAService.GetPremiseDetailsRespTypeAddress();
                                            foreach (DataRow row1 in dt1.Rows)
                                            {
                                                objAddresses = new DEWADetails.Address();
                                                objAddresses.houseNumber = row1["houseNumber"].ToString() != null ? row1["houseNumber"].ToString() : string.Empty;
                                                objAddresses.street = row1["street"].ToString() != null ? row1["street"].ToString() : string.Empty;
                                                objAddresses.district = row1["district"].ToString() != null ? row1["district"].ToString() : string.Empty;
                                                objAddresses.postalCode = row1["postalCode"].ToString() != null ? row1["postalCode"].ToString() : string.Empty;
                                                objAddresses.city = row1["city"].ToString() != null ? row1["city"].ToString() : string.Empty;
                                                objDEWAResponse.Address = objAddresses;
                                            }
                                        }
                                        lstDEWAResponse.Add(objDEWAResponse);
                                    }
                                }
                            }
                        }
                        flag = 1;
                        json = JsonConvert.SerializeObject(new { lstDEWAResponse, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<List<DEWADetails.DEWAResponse>>(lstDEWAResponse), ConfigurationManager.AppSettings["DEWACode"].ToString(), ConfigurationManager.AppSettings["DEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Record Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DEWACode"].ToString(), ConfigurationManager.AppSettings["DEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, resp, ConfigurationManager.AppSettings["DEWACode"].ToString(), ConfigurationManager.AppSettings["DEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["DEWACode"].ToString(), ConfigurationManager.AppSettings["DEWA"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static string CallWebService(string EmiratesId, string actionname, string url, string IdType = "")
        {
            string soapEnvelopeXml = "";
            soapEnvelopeXml = CreateSoapEnvelope(EmiratesId, IdType);



            HttpWebRequest webRequest = CreateWebRequest(url, actionname);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            string soapResult;
            WebResponse webResponse;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            using (webResponse = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
            }



            return soapResult;
        }
        private static string CreateSoapEnvelopeLiscenceNumber(string LiscenceNumber)
        {
            string soapEnvelopeDocument = "";
            try
            {
                soapEnvelopeDocument =
                //@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:ae:gov:abudhabi:RealEstateAndHousing:HousingTypes:1"" xmlns:urn1=""urn:ae:gov:abudhabi:DigitalDocuments:AttestationTypes:1"">
                @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:trad=""http://TradeLicenseSearchService/"">
   <soapenv:Header/>
   <soapenv:Body>
      <trad:ListTradeLicenseAllData>
         <trad:LicenseID>" + LiscenceNumber + @"</trad:LicenseID>
      </trad:ListTradeLicenseAllData>
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
        private static string CreateSoapEnvelope(string EmiratesId, string IdType)
        {
            string datetime = DateTime.Now.ToString("o");

            string soapEnvelopeDocument = "";
            try
            {
                soapEnvelopeDocument =
              @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:mov=""http://dewa.gov.ae/MovementProcess"">
   <soapenv:Header/>
     <soapenv:Body>
      <mov:GetPremiseDetailsRequest>
         <!--Optional:-->
         <Header>
            <!--Optional:-->
            <consumerID>MOCD</consumerID>
            <!--Optional:-->
            <serviceName>GET_PREMISE_DETAILS</serviceName>
            <!--Optional:-->
            <serviceVersion>1.0</serviceVersion>
            <!--Optional:-->
            <serviceLang>EN</serviceLang>
            <!--Optional:-->
            <userName>CCbS6daProhxZ3Y6jpGuShDUjzKtrBsxO/It6uzn1vU=</userName>
            <!--Optional:-->
            <password>G7/CWrLXW1/GTXgEffHNFdURK6cfJJJXeIncrTc0e/g=</password>
            <!--Optional:-->
            <hashvalue></hashvalue>
            <!--Optional:-->
            <attributelist>
               <!--Zero or more repetitions:-->
               <attribute name=""?"">
                  <!--Zero or more repetitions:-->
                  <value></value>
               </attribute>
            </attributelist>
         </Header>
         <!--Optional:-->
         <Body>
            <!--Optional:-->
            <keyData>
               <!--Optional:-->
               <transactionRefNo>" + GenerateTRefNo() + @"</transactionRefNo>
               <!--Optional:-->
               <timeStamp>" + datetime + @"</timeStamp>
               <!--Optional:-->
               <dewaRefNo></dewaRefNo>
            </keyData>
            <!--Optional:-->
            <premiseDetailsReq>
               <!--Zero or more repetitions:-->
               <record>
                  <!--Optional:-->
                  <idType>" + IdType + @"</idType>
                  <!--Optional:-->
                  <idNumber>" + EmiratesId + @"</idNumber>
               </record>
            </premiseDetailsReq>
         </Body>
      </mov:GetPremiseDetailsRequest>
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
        private static DataSet FillDataSet_FromXML(string xmlData, string servicename)
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
            catch (Exception)
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
                webRequest.Headers.Add(@"GSB-APIKey:" + ConfigurationManager.AppSettings["DEWAGSBKey"].ToString());
                webRequest.Headers.Add(@"X-Gateway-APIKey:" + ConfigurationManager.AppSettings["DEWASDGKey"].ToString());

                webRequest.Method = "POST";
                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["DEWAGSBUserName"].ToString() + ":" + ConfigurationManager.AppSettings["DEWAGSBPassword"].ToString()));
                webRequest.Headers["Authorization"] = "Basic " + authInfo;
                return webRequest;
            }
            catch (Exception)
            {
                return null;
            }

        }
        private static string GenerateRandomNumber()
        {
            int rnd = new Random().Next(10000, 99999);

            if (rnd.ToString().Length == 4)
                return rnd.ToString() + "9";
            else if (rnd.ToString().Length == 3)
                return rnd.ToString() + "99";
            else if (rnd.ToString().Length == 2)
                return rnd.ToString() + "999";
            else if (rnd.ToString().Length == 1)
                return rnd.ToString() + "9999";

            return rnd.ToString();
        }

        private static string GenerateTRefNo()
        {
            return ConfigurationManager.AppSettings["DEWATransactionRefNo"].ToString() + DateTime.Now.ToString("ddMMyy") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + GenerateRandomNumber();
        }
    }
}