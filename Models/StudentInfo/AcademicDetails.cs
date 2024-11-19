using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace MOCDIntegrations.Models.StudentInfo
{
    public class AcademicDetails
    {
        public string StudentGrade { get; set; }
        public School School { get; set; }
        public Homeform Homeform { get; set; }
        public string WithdrawDate { get; set; }
    }
}