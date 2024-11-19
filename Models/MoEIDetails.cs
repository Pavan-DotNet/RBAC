using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MoEIDetails
    {
        public class MoEIDetailsRequestParams
        {
            public string EmiratesID { get; set; }
            public string TNo { get; set; }
            public string FNo { get; set; }
        }

        public class MoEIDetailsResponseParams
        {
            public string NameAra { get; set; }
            public string Emirate { get; set; } 
            public string Email { get; set; }
            public string TotalSalary { get; set; }
            public string ReqUseOfAid { get; set; }
            public string DecreeDate { get; set; }
            public string ApplicationDate { get; set; }
            public string BirthDate { get; set; }
            public string Employer { get; set; }
            public string HouseOwnNameAra { get; set; }
            public string ApplicationStateNameAra { get; set; }
            public string FileDate { get; set; }
            public string AssistantStateNameAra { get; set; }
            public string Amount { get; set; }
            public string FamilyNo { get; set; }
            public string TownNo { get; set; }
            public string ApplicationNo { get; set; }
            public string Mobileno { get; set; }
            public string DecreeSubTypeNameAra { get; set; }
            public string ReqAidType { get; set; }
            public string HLCEast { get; set; }
            public string HLCNorth { get; set; }
            public string EmirateIdCardNo { get; set; }
            public string RegionName { get; set; }
            public string HasLand { get; set; }

        }
    }
}