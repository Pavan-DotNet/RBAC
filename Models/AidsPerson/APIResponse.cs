using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOCDIntegrations.Models.AidsPerson
{
    public class APIResponse
    {
        public string StatusCode { get; set; }
        public AidsPersons Data { get; set; }
    }
}
