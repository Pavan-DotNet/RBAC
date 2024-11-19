using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Utils
{
    public static class Utils
    {
        public static string FormatDate(DateTime dt)
        {
            string dtString = string.Empty;
            if (dt != DateTime.MinValue)
                dtString = dt.Day + "/" + dt.Month + "/" + dt.Year;
            return dtString;
        }
    }
}