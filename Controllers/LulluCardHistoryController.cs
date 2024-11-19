using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using static MOCDIntegrations.Models.LulluCardHistoryDetails.AuthenticateRequestModel;
using static MOCDIntegrations.Models.LulluCardHistoryDetails;
using System.Configuration;
using MOCDIntegrations.Models;
using static MOCDIntegrations.Models.DPPRISONSTATUSDetails;
using Newtonsoft.Json.Converters;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace MOCDIntegrations.Controllers
{
    public class LulluCardHistoryController : Controller
    {
        // GET: LulluCardHistory
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string EmiratesId, string UserAgent)
        {
            var json = "";
            int flag = 0;
            try
            {
                if (EmiratesId.Length == 15)
                {
                    var apiResult = LULUCardHistoryAPICall(EmiratesId);
                    if (apiResult != null)
                    {
                        if (apiResult.ResponseCode == "0")
                        {
                            SaveDetails(apiResult, EmiratesId);
                            flag = 1;

                            json = JsonConvert.SerializeObject(new { apiResult, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["LulluTranCode"].ToString(), ConfigurationManager.AppSettings["LulluTran"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);

                        }
                        else
                        {
                            flag = 2;
                            string ResponseDescription = "No Matching Records Found";
                            json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                            LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["LulluTranCode"].ToString(), ConfigurationManager.AppSettings["LulluTran"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                        }
                    }
                    else
                    {
                        flag = 2;
                        string ResponseDescription = "No Matching Records Found";
                        json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                        LogIntegrationDetails.LogSerilog(EmiratesId, json, ConfigurationManager.AppSettings["LulluTranCode"].ToString(), ConfigurationManager.AppSettings["LulluTran"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                    }

                }
            }
            catch (WebException ex)
            {
                flag = 3;

                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["LulluTranCode"].ToString(), ConfigurationManager.AppSettings["LulluTran"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                flag = 3;
                //var resp = new StreamReader(ex.Message).ReadToEnd();
                string ResponseDescription = ex.Message.ToString();
                json = JsonConvert.SerializeObject(new { ResponseDescription, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(EmiratesId, ResponseDescription, ConfigurationManager.AppSettings["LulluTranCode"].ToString(), ConfigurationManager.AppSettings["LulluTran"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }

        private static LulluCardHistoryDetails.Root LULUCardHistoryAPICall(string emirateID)
        {
            LulluCardHistoryDetails.Root objResponse = null;
            var token = AuthenticateToken();
            string apiURL = "";
            apiURL = ConfigurationManager.AppSettings["Lullu_CardHistoryUrl"].ToString();
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = "";

            body = JsonConvert.SerializeObject(new CardTransactionHistoryRequest.Root
            {
                CardNumber = emirateID,
                DateAtClient = DateTime.Now.ToString(),
                terminalId = ConfigurationManager.AppSettings["Lullu_TerminalId"].ToString(),
                TransactionId = token.TransactionId != null ? token.TransactionId.ToString() : "",

            }); ;

            String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(ConfigurationManager.AppSettings["Lullu_client_id"].ToString() + ":" + ConfigurationManager.AppSettings["Lullu_client_secret"].ToString()));
            request.AddHeader("Authorization", "Basic " + authInfo);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.AddHeader("JWT_Token", "Bearer " + token.authToken);

            RestResponse response = client.Execute(request);
            if (response.Content != null)
            {
                if (response.Content.Contains("\"ResponseCode\":0,") && response.StatusCode == HttpStatusCode.OK)
                {
                    objResponse = JsonConvert.DeserializeObject<LulluCardHistoryDetails.Root>(response.Content);

                }
                else
                {
                    objResponse = JsonConvert.DeserializeObject<LulluCardHistoryDetails.Root>(response.Content);

                    //return new LulluCardHistoryDetails.Root();
                }
            }
            else
            {
                //objResponse = JsonConvert.DeserializeObject<LulluCardHistoryDetails.Root>(response.Content);

                return new LulluCardHistoryDetails.Root();
            }


            return objResponse;
        }
        private static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        private static AuthenticateResponsetModel.Root AuthenticateToken()
        {
            AuthenticateResponsetModel.Root responseModel = null;
            string apiURL = "";
            apiURL = ConfigurationManager.AppSettings["Lullu_AuthURL"].ToString();
            var token = GenerateAndGetToken(ConfigurationManager.AppSettings["Lullu_Uri"].ToString(), ConfigurationManager.AppSettings["Lullu_grant_type"].ToString(), ConfigurationManager.AppSettings["Lullu_client_id"].ToString(), ConfigurationManager.AppSettings["Lullu_client_secret"].ToString(), ConfigurationManager.AppSettings["Lullu_scope"].ToString());
            var client = new RestClient(apiURL);
            var request = new RestRequest(apiURL, Method.Post);
            //request.AddHeader("Content-Type", "application/json");
            var body = "";
            Random random = new Random();
            int randomNumber = random.Next();
            body = JsonConvert.SerializeObject(new AuthenticateRequestModel.Root
            {
                dateAtClient = DateTime.Now.ToString(),
                //Password = ConfigurationManager.AppSettings["Lullu_Password"].ToString(),
                //terminalId = ConfigurationManager.AppSettings["Lullu_TerminalId"].ToString(),
                transactionId = randomNumber.ToString(),
                //UserName = ConfigurationManager.AppSettings["Lullu_Username"].ToString()
            });


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.AddHeader("Authorization", "Bearer " + token.access_token);

            RestResponse response = client.Execute(request);
            if (response.Content != null)
            {
                if (response.Content.Contains("\"responseCode\":0"))
                {
                    responseModel = JsonConvert.DeserializeObject<AuthenticateResponsetModel.Root>(response.Content);

                    responseModel.TransactionId = randomNumber.ToString();
                }
                else
                {
                    return new AuthenticateResponsetModel.Root();
                }
            }
            else
            {
                return new AuthenticateResponsetModel.Root();

            }


            return responseModel;
        }
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
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;
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

        private static bool SaveDetails(LulluCardHistoryDetails.Root objResponse, string emiratesId)
        {
            List<LulluCardHistoryDetails.Root> modelList = new List<LulluCardHistoryDetails.Root>();

            modelList.Add(objResponse);
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Unified"].ToString()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("LulluCardTransactionHistory_Save", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@EmiratesId", emiratesId != null ? emiratesId.ToString() : "");
                        var dt = ToDataTable(modelList);
                        dt.Columns.Remove("SVRecentTransactions");
                        command.Parameters.AddWithValue("@DTTransactionHistory", dt);
                        command.Parameters.AddWithValue("@SVRecent", ToDataTable(objResponse.SVRecentTransactions));

                        command.CommandTimeout = 0;
                        int res = command.ExecuteNonQuery();
                        if (res > 0) return true;


                        connection.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;



        }

    }
}