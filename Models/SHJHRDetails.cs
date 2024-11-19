using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SHJHRDetails
    {
        public class SHJHRRequest
        {
            public string EmiratesId { get; set; }
        }
        public class SHJHRResponse
        {
            public string EmiratesId { get; set; }
            public string EmployeeNumber { get; set; }
            public string EmployeeNameEn { get; set; }
            public string EmployeeNameAr { get; set; }
            public string EmployerName { get; set; }
            public string JobTitle { get; set; }
            public string JoiningDate { get; set; }
            public string EndingDate { get; set; }
            public string MonthlySalary { get; set; }
            public string LivingExpenses { get; set; }
        }
    }
}