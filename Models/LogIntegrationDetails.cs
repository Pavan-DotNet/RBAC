using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Newtonsoft.Json;
using System.Text;

namespace MOCDIntegrations.Models
{
    public class objDetails
    {
        public string Input { get; set; }
        public string Output { get; set; }
    }
    public class LogIntegrationDetails
    {

        public static void LogSerilog(string input, string output, string serviceCode, string serviceName, string LogDate, string MachineName, string UserAgent, string UserId)
        {
            try
            {
                string Hash = string.Empty;
                try
                {
                    string UserName = Regex.Replace(UserId, @"^(?<domain>.*)\\(?<username>.*)|(?<username>[^\@]*)@(?<domain>.*)?$", "${username}", RegexOptions.None);
                    //Hash = LogToBlockChain(input, output, serviceName, LogDate, UserName);
                }
                catch { }


                Log.Logger = new LoggerConfiguration()
                      .MinimumLevel.Information()
                      .WriteTo
                      .MSSqlServer(
                           connectionString: ConfigurationManager.AppSettings["INTEGRATION_CONN"].ToString(),
                           sinkOptions: new MSSqlServerSinkOptions { TableName = ConfigurationManager.AppSettings["TableName"].ToString() })
                      .CreateLogger();

                LogRequestParameters objlog = new LogRequestParameters();
                JsonHelper objHelper = new JsonHelper();

                objlog.ServiceCode = serviceCode;
                objlog.ServiceName = serviceName;
                objlog.Input = input;
                objlog.LogDate = LogDate;
                objlog.MachineName = MachineName.ToString();
                objlog.Output = output;
                objlog.UserAgent = UserAgent;
                objlog.UserId = Regex.Replace(UserId, @"^(?<domain>.*)\\(?<username>.*)|(?<username>[^\@]*)@(?<domain>.*)?$", "${username}", RegexOptions.None);
                objlog.BlockchainHash = Hash;

                var logdata = objHelper.ConvertObjectToJSon<LogRequestParameters>(objlog);
                Log.Information(logdata);
                Log.CloseAndFlush();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string LogToBlockChain(string input, string output, string serviceName, string LogDate, string UserName)
        {
            var content = string.Empty;

            try
            {
                JsonHelper objHelper = new JsonHelper();
                BlockChainLog objLog = new BlockChainLog();
                objLog.Entity = serviceName;
                objLog.Type = ConfigurationManager.AppSettings["BlockchainType"].ToString();
                objLog.URL = ConfigurationManager.AppSettings["BlochainAppUrl"].ToString();

                objDetails details = new objDetails();
                details.Input = input;
                details.Output = output;

                var det = JsonConvert.SerializeObject(details);


                // objLog.Details = "{ \"INPUT\":  " + input + " , " + "\"OUTPUT\": \"" + output.Replace("\n", " ").Replace("\"","'") + "\"}";

                objLog.Details = det; // "{ \"INPUT\":  " + input + " , " + "\"OUTPUT\": \"" + output.Replace("<", " ").Replace(">", " ").Replace("\n", " ") + "\"}";
                objLog.TimeStamp = LogDate;
                objLog.UserId = UserName;

                var logdata = objHelper.ConvertObjectToJSon<BlockChainLog>(objLog);

                byte[] bytes = Encoding.UTF8.GetBytes(logdata.ToCharArray());


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["BlockchainUrl"].ToString());
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bytes.Length;


                using (Stream webStream = request.GetRequestStream())
                {
                    webStream.Write(bytes, 0, bytes.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                content = Ex.InnerException.Message;
            }


            return content;
        }
     
    }
}