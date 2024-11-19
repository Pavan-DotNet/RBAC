using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOCDIntegrations.Models.NRC_Client
{
    public class APIResponse
    {
        public string StatusCode { get; set; }
        public NRCClientDetails Data { get; set; }
    }
}
