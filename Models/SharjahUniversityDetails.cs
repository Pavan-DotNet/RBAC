using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SharjahUniversityDetails
    {
        public class SharjahUniversityRequestParams
        {
            public string EmiratesID { get; set; }

        }
        public class TokenDetails
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string scope { get; set; }
            public string error { get; set; }
            public string error_description { get; set; }
        }
        public class Data
        {
            public int id { get; set; }
            public string issueDate { get; set; }
            public string emid { get; set; }
            public string stid { get; set; }
            public string name { get; set; }
            public string status { get; set; }
            public string notes { get; set; }
        }

        public class vRoot
        {
            public bool success { get; set; }
            public Data data { get; set; }
        }
    }
}