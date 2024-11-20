using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MOCDIntegrations.Auth;
using MOCDIntegrations.Business;
using MOCDIntegrations.Models;

namespace MOCDIntegrations.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly UserRoleDataAccess _userRoleDataAccess;

        public UserController()
        {
            _userManager = new UserManager();
            _roleManager = new RoleManager();
            _userRoleDataAccess = new UserRoleDataAccess();
        }

        // GET: User
        public ActionResult Index()
        {
            // Existing code...
            return View();
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var userRoles = _userRoleDataAccess.GetUserRoles(id);
            var availableRoles = _userManager.GetAvailableRoles();

            var viewModel = new UserRolesViewModel
            {
                UserId = id,
                Username = "User" + id, // You might want to fetch the actual username
                AssignedRoles = userRoles.ConvertAll(ur => ur.RoleName),
                AvailableRoles = availableRoles
            };

            return View(viewModel);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserRolesViewModel model)
        {
            // Handle the role assignments here
            // You might want to compare the assigned roles with the previous roles
            // and update the database accordingly

            return RedirectToAction("Index");
        }

        // Other actions...
    }
}
