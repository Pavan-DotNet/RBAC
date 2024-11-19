using MOCDIntegrations.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Controllers
{
    public class DLDController : Controller
    {
        // GET: DLD
        public ActionResult Index()
        {
            return View("DLD");
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
        public JsonResult Search(string index, string postdata, string UserAgent)
        {

            var json = "";
            int flag = 0;
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                JsonHelper objHelper = new JsonHelper();
                DLDDetails.Credentials credentials = new DLDDetails.Credentials();
                credentials.UserName = ConfigurationManager.AppSettings["DLDUserName"].ToString();
                credentials.Password = ConfigurationManager.AppSettings["DLDPassword"].ToString();

                var processinput = new JavaScriptSerializer().Deserialize<DLDDetails.searchOwnerRequest>(postdata);

                DLDDetails.searchOwnerRequest searchOwnerRequest = new DLDDetails.searchOwnerRequest();
                searchOwnerRequest.Credentials = credentials;


                searchOwnerRequest.OwnerEmirateId = processinput.OwnerEmirateId;
                searchOwnerRequest.OwnerPassportNo = processinput.OwnerPassportNo;
                searchOwnerRequest.OwnerCountryID = processinput.OwnerCountryID != "-1" ? processinput.OwnerCountryID : string.Empty;
                searchOwnerRequest.OwnerFamilyNameAr = processinput.OwnerFamilyNameAr;
                searchOwnerRequest.OwnerFamilyNameEn = processinput.OwnerFamilyNameEn;
                searchOwnerRequest.OwnerNameAr = processinput.OwnerNameAr;
                searchOwnerRequest.OwnerNameEn = processinput.OwnerNameEn;
                searchOwnerRequest.OwnerMobile = processinput.OwnerMobile;
                searchOwnerRequest.OwnerCountryNameEn = string.Empty;
                searchOwnerRequest.OwnerCountryNameAr = string.Empty;
                searchOwnerRequest.OwnerNo = string.Empty;

                var input = new JavaScriptSerializer().Serialize(searchOwnerRequest);



                //searchOwner
                var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["DLD_searchOwner"].ToString());
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(input.ToString().ToArray());
                request.ContentLength = bytes.Length;

                request.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                 Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["CDAGSBUserName"].ToString() + ":" + ConfigurationManager.AppSettings["CDAGSBPassword"].ToString()));
                request.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["DLDGSBKey"].ToString();
                request.Headers["x-Gateway-APIKey"] = ConfigurationManager.AppSettings["DLDXGSBKey"].ToString();


                Stream newStream = request.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                string searchOwner = string.Empty;
                using (var response = request.GetResponse())
                using (var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    searchOwner = stream.ReadToEnd();


                flag = 1;
                json = JsonConvert.SerializeObject(new { searchOwner, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, searchOwner, ConfigurationManager.AppSettings["DLDCode"].ToString(), ConfigurationManager.AppSettings["DLD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);


            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                json = JsonConvert.SerializeObject(new { resp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, resp, ConfigurationManager.AppSettings["DLDCode"].ToString(), ConfigurationManager.AppSettings["DLD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["DLDCode"].ToString(), ConfigurationManager.AppSettings["DLD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchMoreData(string postdata, string UserAgent)
        {

            var json = "";
            int flag = 0;
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            try
            {
                JsonHelper objHelper = new JsonHelper();
                DLDDetails.Credentials credentials = new DLDDetails.Credentials();
                credentials.UserName = ConfigurationManager.AppSettings["DLDUserName"].ToString();
                credentials.Password = ConfigurationManager.AppSettings["DLDPassword"].ToString();

                var processinput = new JavaScriptSerializer().Deserialize<DLDDetails.searchOwnerPropertiesRequest>(postdata);

                DLDDetails.searchOwnerPropertiesRequest searchOwnerPropertiesRequest = new DLDDetails.searchOwnerPropertiesRequest();
                searchOwnerPropertiesRequest.Credentials = credentials;


                searchOwnerPropertiesRequest.OwnerNo = processinput.OwnerNo;

                var input = new JavaScriptSerializer().Serialize(searchOwnerPropertiesRequest);


                try
                {
                    //searchOwnerproperties
                    var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["DLD_searchOwnerProperties"].ToString());
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    byte[] bytes = Encoding.UTF8.GetBytes(input.ToString().ToArray());
                    request.ContentLength = bytes.Length;
                    request.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                  Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["CDAGSBUserName"].ToString() + ":" + ConfigurationManager.AppSettings["CDAGSBPassword"].ToString()));
                    request.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["DLDGSBKey"].ToString();
                    request.Headers["x-Gateway-APIKey"] = ConfigurationManager.AppSettings["DLDXGSBKey"].ToString();

                    Stream newStream = request.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    string searchOwnerProperties = string.Empty;
                    using (var response = request.GetResponse())
                    using (var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        searchOwnerProperties = stream.ReadToEnd();

                    //getOwnerContractDetails
                    var requestContract = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["DLD_getOwnerContractDetails"].ToString());
                    requestContract.Method = "POST";
                    requestContract.ContentType = "application/json";
                    byte[] bytesContract = Encoding.UTF8.GetBytes(input.ToString().ToArray());
                    requestContract.ContentLength = bytes.Length;
                    requestContract.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
           Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["CDAGSBUserName"].ToString() + ":" + ConfigurationManager.AppSettings["CDAGSBPassword"].ToString()));
                    requestContract.Headers["GSB-APIKey"] = ConfigurationManager.AppSettings["DLDGSBKey"].ToString();
                    requestContract.Headers["x-Gateway-APIKey"] = ConfigurationManager.AppSettings["DLDXGSBKey"].ToString();


                    Stream newStreamContract = requestContract.GetRequestStream();
                    newStreamContract.Write(bytesContract, 0, bytesContract.Length);
                    string getOwnerContractDetails = string.Empty;
                    using (var responseContract = requestContract.GetResponse())
                    using (var streamContract = new StreamReader(responseContract.GetResponseStream(), Encoding.UTF8))
                        getOwnerContractDetails = streamContract.ReadToEnd();


                    flag = 1;
                    json = JsonConvert.SerializeObject(new { getOwnerContractDetails, searchOwnerProperties, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                    LogIntegrationDetails.LogSerilog(postdata, getOwnerContractDetails, ConfigurationManager.AppSettings["DLDCode"].ToString(), ConfigurationManager.AppSettings["DLD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        reader.ReadToEnd();

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                json = JsonConvert.SerializeObject(new { resp, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, resp, ConfigurationManager.AppSettings["DLDCode"].ToString(), ConfigurationManager.AppSettings["DLD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            catch (Exception ex)
            {
                json = JsonConvert.SerializeObject(new { ex.Message, flag }, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                LogIntegrationDetails.LogSerilog(postdata, ex.Message, ConfigurationManager.AppSettings["DLDCode"].ToString(), ConfigurationManager.AppSettings["DLD"].ToString(), DateTime.Now.ToString(), string.Empty, UserAgent, User.Identity.Name);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}