
using System;
using System.Web;
using System.Web.Mvc;
using MOCDIntegrations.Business;

namespace MOCDIntegrations.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RoleBasedAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _allowedRoles;

        public RoleBasedAuthorizeAttribute(params string[] roles)
        {
            _allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var user = httpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var userManager = new UserManager(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                var userId = Convert.ToInt32(user.Identity.Name); // Assuming the Name property stores the UserId
                
                foreach (var role in _allowedRoles)
                {
                    var roleManager = new RoleManager(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    var roleId = roleManager.GetRoleByName(role)?.RoleId;
                    
                    if (roleId.HasValue && userManager.UserHasRole(userId, roleId.Value))
                    {
                        authorize = true;
                        break;
                    }
                }
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
