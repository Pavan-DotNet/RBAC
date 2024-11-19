using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using static MOCDIntegrations.Models.DMATDetails;
using Root = MOCDIntegrations.Models.DMATDetails.Root;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace MOCDIntegrations.Controllers
{
    public class OwnerProfileController : Controller
    {
        public ActionResult Index()
        {
            return View("OwnerProfile");
        }

        private static string GenerateToken()
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();
                TokenDetails tknDetails = obj.GenerateTokenWebMethods(ConfigurationManager.AppSettings["ICATokenUri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["OwnerClientId"].ToString(), ConfigurationManager.AppSettings["OwnerClientSecret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                return tknDetails.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string LoadTransId()
        {
            return "MOCD_ADM_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            List<Root> plotList = null;
            List<PlotOwnershipDetails> listPlot = null;
            Root objresp = null;
            try
            {
                JsonHelper objHelper = new JsonHelper();
                var input = new JavaScriptSerializer().Deserialize<OwnerProfileDetails.OwnerProfileDetailsRequest>(postdata);
                ADMOwnerProfile.OwnerProfileServiceContractClient client = new ADMOwnerProfile.OwnerProfileServiceContractClient();
                ADMOwnerProfile.GetOwnerIdByOwnerIDNResponse objResp = null;
                ADMOwnerProfile.GetPlotOwnershipResponse objRespo = null;

                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken();
                    string apiKey = ConfigurationManager.AppSettings["OwnerAPIKEY"].ToString();
                    httpRequestProperty.Headers["MOCD-APIKey"] = apiKey;

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    ADMOwnerProfile.GetOwnerIdByOwnerIDNRequest oreq = new ADMOwnerProfile.GetOwnerIdByOwnerIDNRequest();
                    oreq.EmployeeId = "MOCD";
                    oreq.NationalIdNumber = input.EmiratesId;
                    oreq.UseEnglish = false;

                    ADMOwnerProfile.SoapHeaderReq objSoapHeader = new ADMOwnerProfile.SoapHeaderReq();
                    objSoapHeader.transactionId = LoadTransId();
                    objSoapHeader.serviceProviderEntity = "MOCD";

                    objResp = client.GetOwnerIdByOwnerIDN(objSoapHeader, oreq);
                    if (objResp.ownerId <= 0)
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    }
                    else
                    {
                        ADMOwnerProfile.GetPlotOwnershipRequest ownr = new ADMOwnerProfile.GetPlotOwnershipRequest();
                        ownr.EmployeeId = "MOCD";
                        ownr.OwnerId = Convert.ToInt64(objResp.ownerId);
                        Session["SesOwnerID"] = Convert.ToInt64(objResp.ownerId);
                        ownr.UseEnglish = false;
                        ADMOwnerProfile.SoapHeaderReq objSoapHeadero = new ADMOwnerProfile.SoapHeaderReq();
                        objSoapHeader.transactionId = LoadTransId();
                        objSoapHeadero.serviceProviderEntity = "MOCD";
                        objRespo = client.GetPlotOwnership(objSoapHeader, ownr);
                        if (objRespo != null && objRespo.PlotList.Length > 0)
                        {
                            plotList = new List<Root>();
                            listPlot = new List<PlotOwnershipDetails>();
                            foreach (ADMOwnerProfile.PlotDetails plot in objRespo.PlotList)
                            {
                                if (plot != null)
                                {
                                    RestResponse response = dmatApiCall(plot.PlotId);
                                    if (response.Content.Contains("\"responseMessage\":\"Success\""))
                                    {
                                        objresp = JsonConvert.DeserializeObject<Root>(response.Content);
                                        if (objresp.plotOwnershipDetails != null)
                                        {
                                            listPlot.Add(objresp.plotOwnershipDetails);

                                        }
                                    }
                                }
                            }
                            if (objresp != null && objresp.plotOwnershipDetails != null)
                            {
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { objRespo, listPlot, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<Root>(objresp), ConfigurationManager.AppSettings["DMATCode"].ToString(), ConfigurationManager.AppSettings["DMAT"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                            else if (objRespo != null && objRespo.PlotList != null && objRespo.PlotList.Length > 0)
                            {
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { objRespo, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                                LogIntegrationDetails.LogSerilog(postdata, json, ConfigurationManager.AppSettings["DMATCode"].ToString(), ConfigurationManager.AppSettings["DMAT"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = "No Matching Records Found";
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            }
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records Found";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DMATCode"].ToString(), ConfigurationManager.AppSettings["DMAT"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }

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

                        string str = string.Empty;
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

                                ResponseDescription = child.InnerXml;
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }); ;
                                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADOPCode"].ToString(), ConfigurationManager.AppSettings["ADOP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                    }
                }
                else
                {
                    ResponseDescription = fault.Reason.ToString();
                    json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                     LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADOPCode"].ToString(), ConfigurationManager.AppSettings["ADOP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
            }
            catch (WebException ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADOPCode"].ToString(), ConfigurationManager.AppSettings["ADOP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["ADOPCode"].ToString(), ConfigurationManager.AppSettings["ADOP"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        private static string GenerateToken(bool value)
        {
            try
            {
                if (value)
                {
                    oAuthTokenGeneration obj = new oAuthTokenGeneration();
                    TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["dmat_uri"].ToString(), ConfigurationManager.AppSettings["dmat_grant_type"].ToString(), ConfigurationManager.AppSettings["dmat_client_id"].ToString(), ConfigurationManager.AppSettings["dmat_client_secret"].ToString(), ConfigurationManager.AppSettings["dmat_scope"].ToString());
                    return tknDetails.access_token;
                }
                else
                {
                    oAuthTokenGeneration obj = new oAuthTokenGeneration();
                    TokenDetails tknDetails = obj.GenerateToken(ConfigurationManager.AppSettings["uri"].ToString(), ConfigurationManager.AppSettings["grant_type"].ToString(), ConfigurationManager.AppSettings["client_id"].ToString(), ConfigurationManager.AppSettings["client_secret"].ToString(), ConfigurationManager.AppSettings["scope"].ToString());
                    return tknDetails.access_token;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string AccessToken(string accessTokenAPIUrl, string securityKey, string userName, string password)
        {
            ResponseAccessToken accessTokenResponse = null;
            try
            {

                var client = new RestClient(accessTokenAPIUrl);
                string uname = "mocd";
                string paswrd = "m0Cd!@o&$#";
                //client.Timeout = -1;
                var request = new RestRequest(accessTokenAPIUrl, Method.Post);
                //request.AddHeader("Authorization", "Bearer b9cd567f-9806-460a-8383-c2c93c0c19fb");
                client.Authenticator = new HttpBasicAuthenticator(userName, password);

                request.AddHeader("Authorization", "Basic " + "bDc0MmE3YjJhYjUyMjU0OWRkODFiMzM3Nzk1NjM4YmRhMDowNzYyZTViOWVkOWE0YzgxYTc5NzRjNDQ4MjQ1ZGRhZA==");
                request.AddHeader("Content-Type", "text/plain");
                request.AddHeader("Cookie", "TS014a97b0=018666c34a163d2608fbc6bdf11fea5fd8be28f44ffd0e780b4af32f99d182142c2ffa6a90d4cf754b3ffdd93a2ce1f1f21902466a");
                var body = @"{" + "\n" + @"""username"":" + '\u0022' + uname + '\u0022' + "," + "\n" + @"    ""password"":" + '\u0022' + paswrd + '\u0022' + "\n" + @"}";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                RestResponse response = client.Execute(request);
                if (!response.Content.Contains("{\"errorCode\": \"MOCD-500\""))
                {
                    string jsonContent = response.Content;
                    accessTokenResponse = JsonSerializer.Deserialize<ResponseAccessToken>(jsonContent);
                }
                return accessTokenResponse != null ? accessTokenResponse.token : null;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        private static RestResponse dmatApiCall(long plotID)
        {
            try
            {
                oAuthTokenGeneration obj = new oAuthTokenGeneration();

                string userName = ConfigurationManager.AppSettings["dmat_client_id"].ToString();
                string password = ConfigurationManager.AppSettings["dmat_client_secret"].ToString();
                string accessTokenApiURL = ConfigurationManager.AppSettings["dmat_login_api_url"].ToString();
                string apiURL = ConfigurationManager.AppSettings["dmat_api_url"].ToString();
                string securityKey = GenerateToken(true);
                var accessToken = AccessToken(accessTokenApiURL, securityKey, userName, password);
                var client = new RestClient(apiURL + "/" + plotID + "/");
                var request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("Authorization", "Bearer " + securityKey);
                request.AddHeader("CustomAuth", "Bearer " + accessToken);
                var response = client.Execute(request);
                Console.WriteLine(response.Content);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}