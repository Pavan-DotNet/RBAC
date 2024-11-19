using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using static MOCDIntegrations.Models.SEDD;
using RestSharp.Authenticators;

namespace MOCDIntegrations.Models
{
    public class oAuthTokenGeneration
    {
        public TokenDetails GetTheAccessTokenwithBody(string vToken, string uri)
        {
            try
            {
                TokenDetails objResponsee = new TokenDetails();
                string Username = "user90216";
                string Password = "c%rcq0";
                string requestBody = "{" + "\"username\":\"" + Username + "\"," + "\"password\":\"" + Password + "\"}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri.ToString());
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = requestBody.Length;
                request.Headers.Add("Authorization", "Bearer " + vToken);
                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(requestBody);
                }

                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response1 = responseReader.ReadToEnd();

                    objResponsee = JsonConvert.DeserializeObject<TokenDetails>(response1);
                }
                return objResponsee;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public TokenDetails GenerateAndGetToken(string uri, string grant_type, string client_id, string client_secret, string scope)
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

        public TokenDetails GenerateToken(string uri, string grant_type, string client_id, string client_secret, string scope)
        {
            TokenDetails objToken = new TokenDetails();
            StringBuilder tokenUri = new StringBuilder();
            tokenUri.Append(uri);
            tokenUri.AppendFormat("?grant_type=" + grant_type + "&client_id=" + client_id + "&client_secret=" + client_secret + "&scope=" + scope);
            String responseBody;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

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


        public TokenDetails GenerateTokenWebMethods(string uri, string grant_type, string client_id, string client_secret, string scope=null)
        {
            TokenDetails objToken = new TokenDetails();
            StringBuilder tokenUri = new StringBuilder();
            tokenUri.Append(uri);
            String responseBody;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUri.ToString());
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                // Create the POST data
                string postData = $"grant_type={grant_type}&client_id={client_id}&client_secret={client_secret}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // Write the data to the request body
                request.ContentLength = byteArray.Length;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
                {
                    responseBody = responseStream.ReadToEnd();
                }

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

        public string AccessToken(string accessTokenAPIUrl, string securityKey, string userName, string password)
        {
            ResponseAccessToken accessTokenResponse = null;
            try
            {

                var client = new RestClient(accessTokenAPIUrl);
                //client.Timeout = -1;
                var request = new RestRequest(accessTokenAPIUrl, Method.Post);
                //request.AddHeader("Authorization", "Bearer b9cd567f-9806-460a-8383-c2c93c0c19fb");
                request.AddHeader("Authorization", "Bearer " + securityKey);
                request.AddHeader("Content-Type", "text/plain");
                var body = @"{" + "\n" + @"""username"":" + '\u0022' + userName + '\u0022' + "," + "\n" + @"    ""password"":" + '\u0022' + password + '\u0022' + "\n" + @"}";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                RestResponse response = client.Execute(request);
                string jsonContent = response.Content;
                accessTokenResponse = JsonSerializer.Deserialize<ResponseAccessToken>(jsonContent);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return accessTokenResponse != null ? accessTokenResponse.access_token : null;
        }
    }

}
