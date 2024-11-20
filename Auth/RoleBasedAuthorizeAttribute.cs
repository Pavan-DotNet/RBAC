using System;
using System.Web;
using System.Web.Mvc;
using MOCDIntegrations.Business;
using System.Linq;

namespace MOCDIntegrations.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RoleBasedAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _requiredPermissions;

        public RoleBasedAuthorizeAttribute(params string[] permissions)
        {
            _requiredPermissions = permissions;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var user = httpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var userManager = new UserManager();
                var userId = Convert.ToInt32(user.Identity.Name); // Assuming the Name property stores the UserId
                
                var userPermissions = userManager.GetUserPermissions(userId);
                authorize = _requiredPermissions.All(p => userPermissions.Contains(p));
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Unauthorized.cshtml"
                };
            }
        }
    }
}
