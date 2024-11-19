using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SharjahCharityDetails
    {
        public class data
        {
            public string id { get; set; }
            public string issueDate { get; set; }
            public string requestIdentityNo { get; set; }
            public List<Details> details { get; set; }
            public string notes { get; set; }
        }

        public class Details
        {
            public int id { get; set; }
            public string fullName { get; set; }
            public string helpKind { get; set; }
            public string helpType { get; set; }
            public double amount { get; set; }
            public DateTime approvalDate { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public string noOfInstallment { get; set; }
            public string helpTypeAr { get; set; }
        }

        public class Rooot
        {
            public bool success { get; set; }
            public data data { get; set; }
        }
    }
}