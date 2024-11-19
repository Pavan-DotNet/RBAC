using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJMOwnerPropertiesDetails
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class List
        {
            public string code { get; set; }
            public string nameAr { get; set; }
            public string nameEn { get; set; }
        }

        public class Root
        {
            public string status { get; set; }
            public string message { get; set; }
            public List<List> list { get; set; }
        }




    }
    public class OwnerDetails
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class EmiratesId
        {
            public OwnerInfo OwnerInfo { get; set; }
            public List<UnitsInfo> UnitsInfo { get; set; }
            public List<LandsInfo> landsInfo { get; set; }
            public PreviousProperties PreviousProperties { get; set; }
        }

        public class LandsInfo
        {
            public string DeedId                { get; set; }
            public string CreatedAt                      { get; set; }
            public string OwnershipType                         { get; set; }
            public string Share                     { get; set; }
            public string LandId                { get; set; }
            public string CityAr                            { get; set; }
            public string CityEn                            { get; set; }
            public string SectorAr                           { get; set; }
            public string SectorEn                           { get; set; }
            public string DistrictAr                                 { get; set; }
            public string DistrictEn                                 { get; set; }
        }

        public class OwnerInfo
        {
            public string NameArabic                    { get; set; }
            public string NameEnglish                   { get; set; }
            public string NationalityCode               { get; set; }
            public string IdentityId                     { get; set; }
        }

        public class PreviousProperties
        {
            public List<UnitsInfo> UnitsInfo { get; set; }
            public List<LandsInfo> landsInfo { get; set; }
        }

        public class Root
        {
            public List<EmiratesId> Emirates { get; set; }
            public string status { get; set; }
            public string message { get; set; }
        }

        public class UnitsInfo
        {
            public string PropertyId                 { get; set; }
            public string ProjectId                     { get; set; }
            public string MainProjectNameAr                 { get; set; }
            public string MainProjectNameEn                 { get; set; }
            public string CreatedAt                 { get; set; }
            public string Share              { get; set; }
        }



    }
}