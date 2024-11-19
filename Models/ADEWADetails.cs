namespace MOCDIntegrations.Models
{
    public class ADEWADetails
    {
        public class ADEWADetailsRequestParams
        {
            public string EmiratesID { get; set; }
        }

        public class ADEWADetailsResponseParams
        {
            public string AccountID { get; set; }
            public string AccountStatus { get; set; }
            public string CurrentBalance { get; set; }
            public string CustomerClassArabicName { get; set; }
            public string CustomerClassCode { get; set; }
            public string CustomerClassEnglishName { get; set; }
            public string CustomerMobileNumber { get; set; }
            public string CustomerNameArabic { get; set; }
            public string CustomerNameEnglish { get; set; }
            public string CustomerPremiseTypeArabicName { get; set; }
            public string CustomerPremiseTypeCode { get; set; }
            public string CustomerPremiseTypeEnglishName { get; set; }
            public string EmiratesIDNumber { get; set; }
            public string LandlordName { get; set; }
            public string PersonPrimaryAddress { get; set; }
            public string PlotNumber { get; set; }
            public string PremiseAddress { get; set; }
            public string PremiseArea { get; set; }
            public string PremiseCityPostal { get; set; }
            public string PremiseID { get; set; }
            public string SectorCommunityCode { get; set; }
            public string TradeLicense { get; set; }
            public string ZoneDistrictCode { get; set; }
        }
    }
}