using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class PPCaseDetails
    {

        public class PPCaseDetailsRequest
        {
            public string EmiratesId { get; set; }
        }
        public class CasesList
        {
            public int id { get; set; }
            public int eventNo { get; set; }
            public int eventYear { get; set; }
            public string prcUnitDesc { get; set; }
            public string eventStatusDesc { get; set; }
            public string partyName { get; set; }
            public string nationality { get; set; }
            public string gender { get; set; }
            public string partyStatus { get; set; }
            public string personNo { get; set; }
            public string emiratesId { get; set; }
            public string partyCharges { get; set; }
            public int partyAge { get; set; }
            public string judgementText { get; set; }
            public string reportType { get; set; }
            public string reportDate { get; set; }
        }

        public class Root
        {
            public List<CasesList> casesList { get; set; }
            public string casesCount { get; set; }
            public string statusCode { get; set; }
            public string message { get; set; }
        }


    }

}