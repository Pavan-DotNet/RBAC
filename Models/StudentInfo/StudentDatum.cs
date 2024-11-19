using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models.StudentInfo
{
    public class StudentDatum
    {
        public BasicDetails BasicDetails { get; set; }
        public AcademicDetails AcademicDetails { get; set; }
        public AddressDetails AddressDetails { get; set; }
        public ContactDetails ContactDetails { get; set; }
    }
}