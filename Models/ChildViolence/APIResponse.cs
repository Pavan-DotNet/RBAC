using MOCDIntegrations.Models.AidsPerson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOCDIntegrations.Models.ChildViolence
{
    public class APIResponse
    {
        public string StatusCode { get; set; }
        public ChildViolenceCase Data { get; set; }
    }
}
