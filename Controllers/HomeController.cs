
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MOCDIntegrations.Auth;

namespace MOCDIntegrations.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [RoleBasedAuthorize("User")]
        public ActionResult Index()
        {
            return View();
        }
    }
}

