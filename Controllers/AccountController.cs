
using System.Web.Mvc;
using System.Web.Security;
using MOCDIntegrations.Auth;

namespace MOCDIntegrations.Controllers
{
    public class AccountController : Controller
    {
        private SqlServerAuthProvider _authProvider;

        public AccountController()
        {
            _authProvider = new SqlServerAuthProvider();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            if (_authProvider.ValidateUser(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
