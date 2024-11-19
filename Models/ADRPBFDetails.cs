using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class ADRPBFDetails
    {
        public class ADRPBFDetailsRequestParams
        {
            public string EmiratesID { get; set; }
            public string TNo { get; set; }
            public string FNo { get; set; }
        }

        public class ADRPBFDetailsResponseParams
        {
            public string Address { get; set; }
            public string City { get; set; }
            public string DOB { get; set; }
            public string Email { get; set; }
            public string Emirate { get; set; }
            public string FamilyBookNo { get; set; }
            public string Fax { get; set; }
            public string FullName { get; set; }
            public string FullNameEnglish { get; set; }
            public string MemberType { get; set; }
            public string Mobile { get; set; }
            public string NationalID { get; set; }
            public string POBOX { get; set; }
            public string MonthlyPension { get; set; }
            public string BeneficiaryId { get; set; }
            public string PensionNumber { get; set; }
            public string PensionStartDate { get; set; }
            public string Phone { get; set; }
            public string Relationship { get; set; }         
        }

    }
}