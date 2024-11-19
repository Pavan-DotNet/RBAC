using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class RAKLandDetails
    {
        public string EmiratesId { get; set; }
        public string Email { get; set; }       
    }

    public class RAKLandDocuments
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public byte[] Base64String { get; set; }

    }

    public class RAKLandResponse
    {
        public string type { get; set; }
        public string id { get; set; }
        public string number { get; set; }
        public string message { get; set; }
        public string log_no { get; set; }
        public string log_msg_no { get; set; }
        public string message_v1 { get; set; }
        public string message_v2 { get; set; }
        public string message_v3 { get; set; }
        public string message_v4 { get; set; }
        public string parameter { get; set; }
        public string row { get; set; }
        public string field { get; set; }
        public string System { get; set; }

    }
}