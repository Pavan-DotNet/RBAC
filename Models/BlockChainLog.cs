using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class BlockChainLog
    {
        public string Entity { get; set; }
        public string Type { get; set; }
        public string URL { get; set; }
        public string Details { get; set; }
        public string TimeStamp { get; set; }
        public string UserId { get; set; }
    }

    public class Details
    {
        public string Input { get; set; }
        public string Output { get; set; }
    }
}