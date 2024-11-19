using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models.StudentInfo
{
    public class Root
    {
        public List<StudentDatum> StudentData { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public PageDetails PageDetails { get; set; }
    }
}