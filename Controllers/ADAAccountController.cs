using System;
using System.Web.Mvc;
using MOCDIntegrations.Auth;

namespace MOCDIntegrations.Controllers
{
    [RoleBasedAuthorize(Permissions.AccessADAAccount)]
    public class ADAAccountController : Controller
    {
        // GET: ADAAccount
        public ActionResult Index()
        {
            return View();
        }

        // Other action methods...
    }
}
