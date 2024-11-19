using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJRE
    {
        public class OwnerInfo
        {
            public string OwnerNameAR { get; set; }
            public string OwnerNameEN { get; set; }
            public string OwnerNationalityCode { get; set; }
            public string OwnerIdentityId { get; set; }
            public List<UnitsInfo> UnitsInfoList { get; set; }
            public List<LandsInfo> LandsInfoList { get; set; }
        }

        public class UnitsInfo
        {
            public string UnitPropertyId { get; set; }
            public string UnitProjectId { get; set; }
            public string UnitMainProjectNameAR { get; set; }
            public string UnitMainProjectNameEN { get; set; }
            public string UnitCreatedAt { get; set; }
            public string UnitShare { get; set; }
        }

        public class LandsInfo
        {
            public string LandDeedId { get; set; }
            public string LandCreatedAt { get; set; }
            public string LandOwnershipType { get; set; }
            public string LandShare { get; set; }
            public string LandId { get; set; }
            public string LandCityAR { get; set; }
            public string LandCityEN { get; set; }
            public string LandSectorAR { get; set; }
            public string LandSectorEN { get; set; }
            public string LandDistrictAR { get; set; }
            public string LandDistrictEN{ get; set; }
        }
               
    }
}