using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MOCDIntegrations.Models.OwnerDetails;

namespace MOCDIntegrations.Controllers
{
    public class MBHREController : Controller
    {
        // GET: MBHRE
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string EmiratesId, string postdata, string UserAgent)
        {
            var json = "";
            int flag = 0;
            string EmirateID = "";
            RestResponse response = null;
            try
            {

                if (EmiratesId.Length == 15)
                {
                    var token = GenerateAceessTokenAsync();
                    var exemptionsResponse = MBHREExceptionsAPICALL(EmiratesId, token).Content;
                    exemptionsResponse = exemptionsResponse.Replace("{\r\n  \"payload\": ", "{\r\n  \"excemptions\": ");

                    var govenmentHelpresponse = MBHREGovernmentHelpAPICALL(EmiratesId, token).Content;
                    govenmentHelpresponse = govenmentHelpresponse.Replace("{\r\n  \"payload\": ", "{\r\n  \"governmentHelp\": ");

                    var proprtiesResponse = MBHREPropertiesAPICALL(EmiratesId, token).Content;
                    proprtiesResponse = proprtiesResponse.Replace("{\r\n  \"payload\": ", "{\r\n  \"properties\": ");

                    var customerInfoResponse = MBHRECustomerInfoAPICALL(EmiratesId, token).Content;
                    customerInfoResponse = customerInfoResponse.Replace("{\r\n  \"payload\": ", "{\r\n  \"customerInfo\": ");

                    var servicesResponse = MBHREServicesAPICALL(EmiratesId, token).Content;
                    servicesResponse = servicesResponse.Replace("{\r\n  \"payload\": ", "{\r\n  \"CustomerServices\": ");

                    if (!exemptionsResponse.Contains("\"success\\\": true") || !govenmentHelpresponse.Contains("\"success\\\": true") || !proprtiesResponse.Contains("\"success\\\": true") || !customerInfoResponse.Contains("\"success\\\": true") || !servicesResponse.Contains("\"success\\\": true"))
                    {
                        MBHREDetails.Root objresp = JsonConvert.DeserializeObject<MBHREDetails.Root>(exemptionsResponse);

                        var govHelp = JsonConvert.DeserializeObject<MBHREDetails.Root>(govenmentHelpresponse);
                        objresp.governmentHelp = govHelp.governmentHelp;

                        var proprties = JsonConvert.DeserializeObject<MBHREDetails.Root>(proprtiesResponse);
                        objresp.properties = proprties.properties;


                        var customer = JsonConvert.DeserializeObject<MBHREDetails.Root>(customerInfoResponse);
                        objresp.customerInfo = customer.customerInfo;


                        var services = JsonConvert.DeserializeObject<MBHREDetails.Root>(servicesResponse);
                        objresp.CustomerServices = services.CustomerServices;

                        if (objresp?.excemptions != null && objresp?.excemptions?.Count > 0)
                        {
                            flag = 1;
                            json = JsonConvert.SerializeObject(new { objresp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                        }
                        else
                        {
                            flag = 3;
                            string ResponseDescription = "No Matching Records available";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MBHRECode"].ToString(), ConfigurationManager.AppSettings["MBHRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    else
                    {
                        flag = 3;
                        string ResponseDescription = "No Matching Records available";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MBHRECode"].ToString(), ConfigurationManager.AppSettings["MBHRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }
                }

            }
            catch (WebException ex)
            {
                flag = 4;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MBHRECode"].ToString(), ConfigurationManager.AppSettings["MBHRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


            }
            catch (Exception ex)
            {
                flag = 4;
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                LogIntegrationDetails.LogSerilog(postdata, ResponseDescription, ConfigurationManager.AppSettings["MBHRECode"].ToString(), ConfigurationManager.AppSettings["MBHRE"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        private static RestResponse MBHREServicesAPICALL(string emirateID, string token)
        {
            try
            {
                string apiURL = ConfigurationManager.AppSettings["MBHRE_Services_URL"].ToString();

                var client = new RestClient(apiURL);

                var request = new RestRequest(apiURL, Method.Post);

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MBHRE_username"].ToString() + ":" + ConfigurationManager.AppSettings["MBHRE_password"].ToString())));

                request.AddHeader("X-gateway-APIKey", ConfigurationManager.AppSettings["MBHRE_APIKey"].ToString());
                request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["MBHRE_GsbKey"].ToString());
                request.AddHeader("x-nv-header", "Bearer " + token);

                request.AddHeader("Content-Type", "application/json");

                var body = @"{" + "\n" + @"""page"":" + '\u0022' + "0" + '\u0022' + "," + "\n" + @"    ""pagesize"":" + '\u0022' + "10" + '\u0022' + "\n" + "," + "\n" + @"    ""emiratesId"":" + '\u0022' + emirateID + '\u0022' + "\n" + @"}";

                request.AddParameter("application/json", body, ParameterType.RequestBody);

                RestResponse response = client.Execute(request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static RestResponse MBHREGovernmentHelpAPICALL(string emirateID, string token)
        {
            try
            {
                string apiURL = ConfigurationManager.AppSettings["MBHRE_Government_URL"].ToString();

                var client = new RestClient(apiURL);

                var request = new RestRequest(apiURL, Method.Post);

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MBHRE_username"].ToString() + ":" + ConfigurationManager.AppSettings["MBHRE_password"].ToString())));

                request.AddHeader("X-gateway-APIKey", ConfigurationManager.AppSettings["MBHRE_APIKey"].ToString());
                request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["MBHRE_GsbKey"].ToString());
                request.AddHeader("x-nv-header", "Bearer " + token);

                request.AddHeader("Content-Type", "application/json");

                var body = @"{" + "\n" + @"""page"":" + '\u0022' + "0" + '\u0022' + "," + "\n" + @"    ""pagesize"":" + '\u0022' + "10" + '\u0022' + "\n" + "," + "\n" + @"    ""emiratesId"":" + '\u0022' + emirateID + '\u0022' + "\n" + @"}";

                request.AddParameter("application/json", body, ParameterType.RequestBody);

                RestResponse response = client.Execute(request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static RestResponse MBHRECustomerInfoAPICALL(string emirateID, string token)
        {
            try
            {
                string apiURL = ConfigurationManager.AppSettings["MBHRE_Customer_URL"].ToString();

                var client = new RestClient(apiURL);

                var request = new RestRequest(apiURL, Method.Post);

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MBHRE_username"].ToString() + ":" + ConfigurationManager.AppSettings["MBHRE_password"].ToString())));

                request.AddHeader("X-gateway-APIKey", ConfigurationManager.AppSettings["MBHRE_APIKey"].ToString());
                request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["MBHRE_GsbKey"].ToString());
                request.AddHeader("x-nv-header", "Bearer " + token);

                request.AddHeader("Content-Type", "application/json");

                var body = @"{" + "\n" + @"""page"":" + '\u0022' + "0" + '\u0022' + "," + "\n" + @"    ""pagesize"":" + '\u0022' + "10" + '\u0022' + "\n" + "," + "\n" + @"    ""emiratesId"":" + '\u0022' + emirateID + '\u0022' + "\n" + @"}";

                request.AddParameter("application/json", body, ParameterType.RequestBody);

                RestResponse response = client.Execute(request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static RestResponse MBHREPropertiesAPICALL(string emirateID, string token)
        {
            try
            {
                string apiURL = ConfigurationManager.AppSettings["MBHRE_Properties_URL"].ToString();

                var client = new RestClient(apiURL);

                var request = new RestRequest(apiURL, Method.Post);

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MBHRE_username"].ToString() + ":" + ConfigurationManager.AppSettings["MBHRE_password"].ToString())));

                request.AddHeader("X-gateway-APIKey", ConfigurationManager.AppSettings["MBHRE_APIKey"].ToString());
                request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["MBHRE_GsbKey"].ToString());
                request.AddHeader("x-nv-header", "Bearer " + token);

                request.AddHeader("Content-Type", "application/json");

                var body = @"{" + "\n" + @"""page"":" + '\u0022' + "0" + '\u0022' + "," + "\n" + @"    ""pagesize"":" + '\u0022' + "10" + '\u0022' + "\n" + "," + "\n" + @"    ""emiratesId"":" + '\u0022' + emirateID + '\u0022' + "\n" + @"}";

                request.AddParameter("application/json", body, ParameterType.RequestBody);

                RestResponse response = client.Execute(request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static RestResponse MBHREExceptionsAPICALL(string emirateID, string token)
        {
            try
            {
                string apiURL = ConfigurationManager.AppSettings["MBHRE_Exemption_URL"].ToString();

                var client = new RestClient(apiURL);

                var request = new RestRequest(apiURL, Method.Post);

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MBHRE_username"].ToString() + ":" + ConfigurationManager.AppSettings["MBHRE_password"].ToString())));

                request.AddHeader("X-gateway-APIKey", ConfigurationManager.AppSettings["MBHRE_APIKey"].ToString());
                request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["MBHRE_GsbKey"].ToString());
                request.AddHeader("x-nv-header", "Bearer " + token);

                request.AddHeader("Content-Type", "application/json");

                var body = @"{" + "\n" + @"""page"":" + '\u0022' + "0" + '\u0022' + "," + "\n" + @"    ""pagesize"":" + '\u0022' + "10" + '\u0022' + "\n" + "," + "\n" + @"    ""emiratesId"":" + '\u0022' + emirateID + '\u0022' + "\n" + @"}";

                request.AddParameter("application/json", body, ParameterType.RequestBody);

                RestResponse response = client.Execute(request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static string GenerateAceessTokenAsync()
        {
            try
            {


                string apiURL = ConfigurationManager.AppSettings["MBHRE_Access"].ToString();
                var client = new RestClient(apiURL);
                var request = new RestRequest(apiURL, Method.Post);

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["MBHRE_username"].ToString() + ":" + ConfigurationManager.AppSettings["MBHRE_password"].ToString())));
                request.AddHeader("X-gateway-APIKey", ConfigurationManager.AppSettings["MBHRE_APIKey"].ToString());
                request.AddHeader("GSB-APIKey", ConfigurationManager.AppSettings["MBHRE_Token_GsbKey"].ToString());
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");



                request.AddParameter("grant_type", "password");
                request.AddParameter("client_id", ConfigurationManager.AppSettings["MBHRE_ClientId"].ToString());
                request.AddParameter("client_secret", ConfigurationManager.AppSettings["MBHRE_Consumer"].ToString());
                RestResponse response = client.Execute(request);
                TokenDetails objToken = JsonConvert.DeserializeObject<TokenDetails>(response.Content);
                return objToken.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}