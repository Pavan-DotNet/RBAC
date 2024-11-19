using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SPEADetails
    {
        // Root myDeserializedClass = JsonConvert.Deserializestring<Root>(myJsonResponse);
        public class Data
        {
            public int id { get; set; }
            public DateTime issueDate { get; set; }
            public string requestIdentityNo { get; set; }
            public string scosId { get; set; }
            public string scosTimeStamp { get; set; }
            public string studentId { get; set; }
            public string pupilId { get; set; }
            public string emiratesId { get; set; }
            public string nameEn { get; set; }
            public string nameAr { get; set; }
            public string currentAcademicYear { get; set; }
            public string isStillEnrolled { get; set; }
            public string currentSchoolId { get; set; }
            public string currentSchoolEn { get; set; }
            public string currentSchoolAr { get; set; }
            public string currentCurriculumId { get; set; }
            public string currentCurriculumEn { get; set; }
            public string currentCurriculumAr { get; set; }
            public string currentTutionFee { get; set; }
            public string currentGrade { get; set; }
            public string lastAcademicYear { get; set; }
            public string lastSchoolId { get; set; }
            public string lastSchoolEn { get; set; }
            public string lastSchoolAr { get; set; }
            public string lastCurriculumId { get; set; }
            public string lastCurriculumEn { get; set; }
            public string lastCurriculumAr { get; set; }
            public string lastGrade { get; set; }
            public string lastTutionFee { get; set; }
            public string notes { get; set; }
        }

        public class Root
        {
            public bool success { get; set; }
            public Data data { get; set; }
        }


    }
}