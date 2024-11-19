using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SHJDHDetails
    {
        public class SHJDHRequest
        {
            public string EmiratesId { get; set; }
        }
        public class Root
        {
            public data data { get; set; }
        }
        public class data
        {
            public int id { get; set; }
            public DateTime issueDate { get; set; }
            public string requestIdentityNo { get; set; }
            public string personName { get; set; }
            public string eid { get; set; }
            public string familyNo { get; set; }
            public string townNo { get; set; }
            public string emiratesName { get; set; }
            public string cityName { get; set; }
            public string regionName { get; set; }
            public string mobile { get; set; }
            public string finalAmount { get; set; }
            public DateTime endDate { get; set; }
            public string appState { get; set; }
            public DateTime apiRequestDate { get; set; }
            public string statusCode { get; set; }
            public string statusDescription { get; set; }
            public string notes { get; set; }

        }

    }
}