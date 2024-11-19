using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SSSD
    {
        public class SSSDRequest
        {
            public string EmiratesID { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class AidDetail
        {
            public string helpCategory { get; set; }
            public string region { get; set; }
            public int amount { get; set; }
            public string nationality { get; set; }
            public string issueDate { get; set; }
        }

        public class Data
        {
            public bool success { get; set; }
            public Data data { get; set; }
            public int fileId { get; set; }
            public string name { get; set; }
            public string region { get; set; }
            public string identityNo { get; set; }
            public string familyCard { get; set; }
            public string nationality { get; set; }
            public string aidStatus { get; set; }
            public List<AidDetail> aidDetails { get; set; }
        }

        public class Root
        {
            public Data data { get; set; }
            public string StatusCode { get; set; }
            public string StatusMessage { get; set; }
        }



    }
}