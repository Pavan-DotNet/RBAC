using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SSSDSewaDetails
    {
        public class Root
        {
            public bool success { get; set; }
            public Data data { get; set; }
        }
        public class Data
        {
            public int id { get; set; }
            public string issueDate { get; set; }
            public string emiratesId { get; set; }
            public string personName { get; set; }
            public string consumerNo { get; set; }
            public string currentBalance { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string premise { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string address3 { get; set; }
            public string address4 { get; set; }
            public string areaName { get; set; }
            public string notes { get; set; }

            public List<accounts> accounts { get; set; }
        }
        public class accounts
        {
            public string AccountID { get; set; }
            public string Acccount { get; set; }
        }
    }
}