using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class ADPoliceDetails
    {

        public class VehicleRegistrationRequestParams
        {
            public string EmiratesID { get; set; }
        }

        public class VehicleRegistrationResponseParams
        {

            public string AxisCount                             { get; set; }
            public string ChairNumber                               { get; set; }
            public string ChassisNumber                         { get; set; }
            public string CylindersCount                             { get; set; }
            public string DateOfRegister                        { get; set; }
            public string DoorsCount                            { get; set; }
            public string EmiratesIDNumber                      { get; set; }
            public string EmptyWeight                           { get; set; }
            public string EngineNumber                          { get; set; }
            public string FuelType                              { get; set; }
            public string FullWeight                            { get; set; }
            public string GearType                       { get; set; }
            public int    ID                                       { get; set; }
            public string InsuranceCompany                              { get; set; }
            public string InsuranceExpiryDate                       { get; set; }
            public string InsuranceType                              { get; set; }
            public string ManufacturingCountry                                  { get; set; }
            public string ManufacturingYear                             { get; set; }
            public string MOIUnifiedNumber                      { get; set; }
            public string MortgageReferences                     { get; set; }
            public string MortgageSource                        { get; set; }
            public string OwnerArabicName                           { get; set; }
            public string OwnerCode                          { get; set; }
            public string OwnerEnglishName                       { get; set; }
            public string OwnerNationality                      { get; set; }
            public string PlateColor                         { get; set; }
            public string PlateKind                           { get; set; }
            public string PlateNumber                         { get; set; }
            public string PlateSource                               { get; set; }
            public string PlateType                           { get; set; }
            public string PolicyNumber                   { get; set; }
            public string SteeringType                       { get; set; }
            public string VehicleClass                       { get; set; }
            public string VehicleColor                       { get; set; }
            public string VehicleLicenseEndDate                  { get; set; }
            public string VehicleModel                          { get; set; }
            public string VehicleType                           { get; set; }
            public string WeightType                     { get; set; }
            public string WheelsCount                       { get; set; }
        }
    }
}