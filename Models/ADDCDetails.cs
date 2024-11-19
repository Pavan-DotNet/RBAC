using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MOCDIntegrations.Models
{
    public class ADDCDetails
    {
        public class Return
        {
            public string CaseCreationDate { get; set; }
            public string CaseNo { get; set; }
            public string CaseSource { get; set; }
            public string CaseStatus { get; set; }
            public string CaseSubType { get; set; }
            public string CaseType { get; set; }
            public string PartyName { get; set; }
        }

       

    }
}