using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class DEWADetails
    {
        public class DEWARequest
        {
            public string EmiratesId { get; set; }
        }

        public class DEWAResponse
        {
            public string inputIdType { get; set; }
            public string inputIdNumber { get; set; }
            public string premiseNo { get; set; }
            public string premiseType { get; set; }
            public string contractAccount { get; set; }
            public string contractAccountType { get; set; }
            public string ownerName { get; set; }
            public string uaeNational { get; set; }
            public string moveInDate { get; set; }
            public string moveOutDate { get; set; }
            public string makaniNumber { get; set; }
            public string eidNumber { get; set; }
            public string socialbenefit { get; set; }
            public string communityNumber { get; set; }
            public string billingCycle { get; set; }

            public string recInflationAllowance { get; set; }

            public Address Address { get; set; }
        }
        public class Address
        {
            public string houseNumber { get; set; }
            public string street { get; set; }
            public string district { get; set; }
            public string postalCode { get; set; }
            public string city { get; set; }

        }
    }
}