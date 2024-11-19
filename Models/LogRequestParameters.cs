using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class LogRequestParameters
    {
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string LogDate { get; set; }
        public string MachineName { get; set; }
        public string UserAgent { get; set; }
        public string UserId { get; set; }
        public string BlockchainHash { get; set; }
    }
}