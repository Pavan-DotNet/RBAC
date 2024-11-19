using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MOCDIntegrations.Utils
{
    public class CustomJsonResult : JsonResult
    {
        public CustomJsonResult(object data, JsonRequestBehavior behavior, int maxJsonLength)
        {
            Data = data;
            JsonRequestBehavior = behavior;
            MaxJsonLength = maxJsonLength;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                var serializer = new JavaScriptSerializer { MaxJsonLength = (int)MaxJsonLength };
                response.Write(serializer.Serialize(Data));
            }
        }
    }
}