using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class CDADetailsNew
    {
        // Root myDeserializedClass = JsonConvert.Deserializestring<Root>(myJsonResponse);

        public class CDARequest
        {
            public string BenficiaryEID { get; set; }
            public string HoushouldEID { get; set; }
            public string IsActive { get; set; }
            public string MonthBatch { get; set; }

        }
        public class HouseHoldMember
        {
            public string FullName { get; set; }
            public string Nationality { get; set; }
            public string Gender { get; set; }
            public string IDNcase { get; set; }
            public string DOBcase { get; set; }
        }

        public class Root
        {
            public Value Value { get; set; }
            public List<string> Formatters { get; set; }
            public List<string> ContentTypes { get; set; }
            public string DeclaredType { get; set; }
            public int StatusCode { get; set; }
        }

        public class Value
        {
            public bool IsActive { get; set; }
            public DateTime StoppedCaseDate { get; set; }
            public double BenefitAmount { get; set; }
            public string FullNameAr { get; set; }
            public string FullNameEn { get; set; }
            public string Nationality { get; set; }
            public string Gender { get; set; }
            public string EID { get; set; }
            public string Relation { get; set; }
            public string DateOfBirth { get; set; }
            public string DewaAcc { get; set; }
            public string age { get; set; }
            public List<HouseHoldMember> houseHoldMembers { get; set; }
        }


    }

}