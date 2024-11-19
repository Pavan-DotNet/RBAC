using System.Collections.Generic;

namespace MOCDIntegrations.Models.ADPEmployeeInfo
{
    public class Root
    {
        public List<ADPEmployeeInfo.Row> Rows { get; set; }
        public OutParameters OutParameters { get; set; }
    }
}