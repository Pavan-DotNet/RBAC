using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOCDIntegrations.Utils
{
    public class CustomJsonFilter : ActionFilterAttribute
    {
        public int MaxJsonLength { get; set; } = int.MaxValue;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var jsonResult = filterContext.Result as JsonResult;
            if (jsonResult != null)
            {
                filterContext.Result = new CustomJsonResult(
                    jsonResult.Data,
                    jsonResult.JsonRequestBehavior,
                    MaxJsonLength);
            }
            base.OnActionExecuted(filterContext);
        }
    }
}