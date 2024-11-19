using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class QueryCustomer
    {
        public string CaseID { get; set; }
        public string PersonID { get; set; }
        public string NameArabic { get; set; }
        public string NameEnglish { get; set; }
        public string Category { get; set; }
        public string NationalId { get; set; }
        public string PassportNo { get; set; }
        public string TownNo { get; set; }
        public string FamilyNo { get; set; }
        public string DOB { get; set; }
        public string UnifiedNo { get; set; }
        public string UserType { get; set; }
    }
}