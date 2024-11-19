using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class RAKLEASEDetails
    {
        public class RAKLEASEDetailsRequestParams
        {
            public string EmiratesId { get; set; }
        }

        public class RAKLEASEDetailsResponseParams
        {
            public string Contractor_Number { get; set; }
            public string Person_Name { get; set; }
            public string Property_Type { get; set; }
            public string Location { get; set; }
            public string Annual_Lease_Amount { get; set; }
            public string Lease_Duration { get; set; }
            public string Start_Date { get; set; }
            public string End_Date { get; set; }
            public string Contractor_Type { get; set; }

        }
    }
}