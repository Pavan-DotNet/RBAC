using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MOCDstringegrations.Models
{
    public class MOHDetails
    {


        public class DeathDetail
        {

            public string ApplicationStatusArabic { get; set; }

            public string ApplicationStatusCode { get; set; }

            public string ApplicationStatusEnglish { get; set; }

            public string City { get; set; }

            public string CityDescArabic { get; set; }

            public string CityDescEnglish { get; set; }

            public string DateOfBirth { get; set; }

            public string DateOfDeath { get; set; }

            public string Emirate { get; set; }

            public string EmirateDescArabic { get; set; }

            public string EmirateDescEnglish { get; set; }

            public string Gender { get; set; }

            public string HospitalNameArabic { get; set; }

            public string HospitalNameEnglish { get; set; }

            public string Nationality { get; set; }

            public string NationalityDescArabic { get; set; }

            public string NationalityDescEnglish { get; set; }

            public string PassportNumber { get; set; }

            public string PersonEmirateIDN { get; set; }

            public string PersonNameArabic { get; set; }

            public string PersonNameEnglish { get; set; }

            public string PlaceOfDeathArabic { get; set; }

            public string PlaceOfDeathEnglish { get; set; }

            public string QAIDNumber { get; set; }

            public string TransactionDate { get; set; }
        }




    }
}