using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SEDD
    {
        public class SEDDRequest
        {
            public string EmiratesId { get; set; }
        }
        public class Root
        {
            public data data { get; set; }
        }

        public class data
        {
            public string id { get; set; }
            public string IssueDate { get; set; }
            public string EmiratesId { get; set; }
            public string Notes { get; set; }
            public List<licenses> licenses { get; set; }

        }

        public class licenses
        {
            public string licId { get; set; }
            public string trdNameAr { get; set; }
            public string trdNameEn { get; set; }
            public string expiryDate { get; set; }
            public string invTypeAr { get; set; }
            public string invTypeEn { get; set; }
            public string licStatusAr { get; set; }
            public string licStatusEn { get; set; }
            public string invEmiratesId { get; set; }
            public string invNameAr { get; set; }

        }
        public class ResponseAccessToken
        {
            public string username { get; set; }
            public string token { get; set; }

            public List<string> roles { get; set; }
            public string token_type { get; set; }
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
        }
    }




}