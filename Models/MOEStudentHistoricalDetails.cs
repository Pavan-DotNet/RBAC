using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MOEStudentHistoricalDetails
    {

        public string academic_Year { get; set; }
        public string studentID { get; set; }
        public string full_Name_AR { get; set; }
        public string full_Name_EN { get; set; }
        public string emirates_ID { get; set; }
        public string gender { get; set; }
        public string nationality { get; set; }
        public string date_of_Birth { get; set; }
        public string student_Age { get; set; }
        public string schoolID { get; set; }
        public string grade_level { get; set; }
        public string exit_date { get; set; }
        public string exitreason_ar { get; set; }
        public string finalAVG { get; set; }
        public bool Success { get; set; }

    }
}