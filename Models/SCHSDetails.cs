using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SCHSDetails
    {
        public class Data
        {
            public int id { get; set; }
            public DateTime issueDate { get; set; }
            public string studentId { get; set; }
            public string studDoReg { get; set; }
            public string studUaeStatusId { get; set; }
            public string studUaeStatus { get; set; }
            public string fullname { get; set; }
            public string academicYear { get; set; }
            public string studEmId { get; set; }
            public string studEidDe { get; set; }
            public string studNationality { get; set; }
            public string branchId { get; set; }
            public string branchName { get; set; }
            public string deptName { get; set; }
            public string studDisType { get; set; }
            public string disabilityType { get; set; }
            public string disabilityId { get; set; }
            public string studHealthType { get; set; }
            public string servname1 { get; set; }
            public string servname2 { get; set; }
            public string servname3 { get; set; }
            public string semester1Servname { get; set; }
            public string semester2Servname { get; set; }
            public string semester3Servname { get; set; }
            public string semester1Supportcenter { get; set; }
            public string semester2Supportcenter { get; set; }
            public string semester3Supportcenter { get; set; }
        }

        public class Root
        {
            public bool success { get; set; }
            public Data data { get; set; }
        }

    }
    public class ShrjahTokenDetails
    {
        public class Root
        {
            public string username { get; set; }
            public List<string> roles { get; set; }
            public string token_type { get; set; }
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
        }


    }
}