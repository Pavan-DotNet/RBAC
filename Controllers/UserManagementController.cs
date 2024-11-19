
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MOCDIntegrations.Auth;
using MOCDIntegrations.Models;

namespace MOCDIntegrations.Controllers
{
    [RoleAuthorize("Admin", "Manager")]
    public class UserManagementController : Controller
    {
        private readonly RoleManager _roleManager;
        private readonly SqlServerAuthProvider _authProvider;

        public UserManagementController()
        {
            _roleManager = new RoleManager();
            _authProvider = new SqlServerAuthProvider();
        }

        // GET: UserManagement
        public ActionResult Index()
        {
            var users = _authProvider.GetAllUsers();
            return View(users);
        }

        // GET: UserManagement/Edit/5
        public ActionResult Edit(int id)
        {
            var user = _authProvider.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var allRoles = _roleManager.GetAllRoles();
            var userRoles = _roleManager.GetUserRoles(id);

            var viewModel = new UserRolesViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                AllRoles = allRoles,
                UserRoles = userRoles
            };

            return View(viewModel);
        }

        // POST: UserManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, List<string> selectedRoles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _roleManager.UpdateUserRoles(id, selectedRoles);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating user roles: " + ex.Message);
                }
            }

            var user = _authProvider.GetUserById(id);
            var allRoles = _roleManager.GetAllRoles();
            var userRoles = _roleManager.GetUserRoles(id);

            var viewModel = new UserRolesViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                AllRoles = allRoles,
                UserRoles = userRoles
            };

            return View(viewModel);
        }
    }
}
