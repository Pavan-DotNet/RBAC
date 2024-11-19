using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models.StudentInfo
{
    public class AddressDetails
    {
        public string MailingAddress { get; set; }
        public string RegionId { get; set; }
        public string SectorId { get; set; }
        public string RoadNo { get; set; }
        public string PlotId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}