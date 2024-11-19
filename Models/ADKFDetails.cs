using System.Collections.Generic;


namespace MOCDIntegrations.Models
{
    public class ADKFDetails
    {
        public class ADKFetailsRequestParams
        {
            public string EmiratesId { get; set; }
        }
        public class Company
        {
            public string Id { get; set; }
            public string TradeName { get; set; }
            public string TradeNameAr { get; set; }
            public string LicenseNumber { get; set; }
            public string LicenseExpiryDate { get; set; }
            public string DateEstablished { get; set; }
            public string BusinessActivity { get; set; }
            public string YearOfEstablishment { get; set; }
        }

        public class Content
        {
            public string Id { get; set; }
            public string FullNameEn { get; set; }
            public string FullNameAr { get; set; }
            public string EmaritesId { get; set; }
            public string Email { get; set; }
            public string JobTitle { get; set; }
            public string MaritalStatus { get; set; }
            public bool IsNational { get; set; }
            public string Emirate { get; set; }
            public string EmaritesIdExpiryDate { get; set; }
            public string EducationLevel { get; set; }
            public string EmployerLocation { get; set; }
            public string EmployerName { get; set; }
            public string EmploymentStatus { get; set; }
            public List<Company> Companies { get; set; }
        }

        public class Root
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public string MessageAR { get; set; }
            public Content Content { get; set; }
        }
    }
}