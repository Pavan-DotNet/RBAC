using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class FUJRENTALDetails
    {

        public class FUJRENTALDetailsRequestParams
        {
            public string EmiratesID { get; set; }
        }

        public class RentalDetails
        {
            public string EmiratesID { get; set; }
            public string RealEstateOwner { get; set; }
            public string OwnershipPercentage { get; set; }
            public string RealEstateLocation { get; set; }
            public string RealEstateNumber { get; set; }
            public string OwnershipDate { get; set; }
            public string RealEstateType { get; set; }
            public string RealEstateStatus { get; set; }
            public string RentAmount { get; set; }
            public string RentalStartDate { get; set; }
            public string RentalEndDate { get; set; }
            public string LastRefreshDate { get; set; }
        }
    }
}