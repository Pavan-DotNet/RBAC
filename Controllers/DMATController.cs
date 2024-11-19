using MOCDIntegrations.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel;

using System.Web.Mvc;
using System.Web.Script.Serialization;
using RestSharp.Authenticators;
using System.Text.Json;
using Newtonsoft.Json;
using static MOCDIntegrations.Models.DMATDetails;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Root = MOCDIntegrations.Models.DMATDetails.Root;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Net;

namespace MOCDIntegrations.Controllers
{
    public class DMATController : Controller
    {
        // GET: DMAT
        public ActionResult DMAT()
        {
            return View("DMAT");
        }
        public ActionResult Search(string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            Root objresp = null;
            JsonHelper objHelper = new JsonHelper();
            List<Root> plotList = null;
            List<PlotOwnershipDetails> listPlot = null;



            flag++;
            try
            {
                var input = new JavaScriptSerializer().Deserialize<OwnerProfileDetails.OwnerProfileDetailsRequest>(postdata);

                ADMOwnerProfile.OwnerProfileServiceContractClient client = new ADMOwnerProfile.OwnerProfileServiceContractClient();
                ADMOwnerProfile.GetOwnerIdByOwnerIDNResponse objResp = null;
                ADMOwnerProfile.GetPlotOwnershipResponse objRespo = null;
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Bearer " + GenerateToken(false);

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    ADMOwnerProfile.GetOwnerIdByOwnerIDNRequest oreq = new ADMOwnerProfile.GetOwnerIdByOwnerIDNRequest();
                    oreq.EmployeeId = "MOCD";
                    oreq.NationalIdNumber = input.EmiratesId; //"784193032742970";
                    oreq.UseEnglish = false;

                    ADMOwnerProfile.SoapHeaderReq objSoapHeader = new ADMOwnerProfile.SoapHeaderReq();
                    objSoapHeader.transactionId = LoadTransId();
                    objSoapHeader.serviceProviderEntity = "MOCD";

                    objResp = client.GetOwnerIdByOwnerIDN(objSoapHeader, oreq);
                    if (objResp!=null && objResp.ownerId > 0)
                    {
                        ADMOwnerProfile.GetPlotOwnershipRequest ownr = new ADMOwnerProfile.GetPlotOwnershipRequest();
                        ownr.EmployeeId = "MOCD";
                        ownr.OwnerId = Convert.ToInt64(objResp.ownerId);
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
                            if (objresp != null && objresp.plotOwnershipDetails!=null)
                            {
                                flag = 1;
                                json = JsonConvert.SerializeObject(new { listPlot, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                //LogIntegrationDetails.LogSerilog(postdata, objHelper.ConvertObjectToJSon<Root>(objresp), ConfigurationManager.AppSettings["DMATCode"].ToString(), ConfigurationManager.AppSettings["DMAT"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                            else
                            {
                                flag = 2;
                                string ResponseDescription = "No Matching Records Found";
                                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                                // LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DMATCode"].ToString(), ConfigurationManager.AppSettings["DMAT"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                            }
                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records Found";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            // LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DMATCode"].ToString(), ConfigurationManager.AppSettings["DMAT"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }

                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        // LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DMATCode"].ToString(), ConfigurationManager.AppSettings["DMAT"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
            }
            catch (WebException ex)
            {
                flag = 3;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                string ResponseDescription = resp;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                // LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DMATCode"].ToString(), ConfigurationManager.AppSettings["DMAT"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                string ResponseDescription = ex.Message;
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                //LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["DMATCode"].ToString(), ConfigurationManager.AppSettings["DMAT"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public static string LoadTransId()
        {
            return "MOCD_ADM_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
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
                string uname = "adhaoss";
                string paswrd = "adh@o$$";
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
                if(!response.Content.Contains("{\"errorCode\": \"MOCD-500\""))
                {
                    string jsonContent = response.Content;
                    accessTokenResponse = JsonSerializer.Deserialize<ResponseAccessToken>(jsonContent);
                }
                else
                {

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
