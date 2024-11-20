
using System;
using System.Web.Mvc;
using MOCDIntegrations.Auth;
using MOCDIntegrations.Business;
using MOCDIntegrations.Models;
using System.Collections.Generic;

namespace MOCDIntegrations.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public UserController()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _userManager = new UserManager(connectionString);
            _roleManager = new RoleManager(connectionString);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Index()
        {
            var users = _userManager.GetAllUsers();
            return View(users);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Details(int id)
        {
            var user = _userManager.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RoleBasedAuthorize("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userManager.CreateUser(user);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating user: " + ex.Message);
                }
            }
            return View(user);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Edit(int id)
        {
            var user = _userManager.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [RoleBasedAuthorize("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userManager.UpdateUser(user);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating user: " + ex.Message);
                }
            }
            return View(user);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Delete(int id)
        {
            var user = _userManager.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [RoleBasedAuthorize("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _userManager.DeleteUser(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting user: " + ex.Message);
                return View(_userManager.GetUserById(id));
            }
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult ManageRoles(int id)
        {
            var user = _userManager.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = _userManager.GetUserRoles(id);
            var availableRoles = _userManager.GetAvailableRoles(id);

            var viewModel = new UserRolesViewModel
            {
                UserId = id,
                Username = user.Username,
                UserRoles = userRoles,
                AvailableRoles = availableRoles
            };

            return View(viewModel);
        }

        [HttpPost]
        [RoleBasedAuthorize("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRole(int userId, int roleId)
        {
            try
            {
                _userManager.AssignRoleToUser(userId, roleId);
                return RedirectToAction("ManageRoles", new { id = userId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error assigning role: " + ex.Message);
                return RedirectToAction("ManageRoles", new { id = userId });
            }
        }

        [HttpPost]
        [RoleBasedAuthorize("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveRole(int userId, int roleId)
        {
            try
            {
                _userManager.RemoveRoleFromUser(userId, roleId);
                return RedirectToAction("ManageRoles", new { id = userId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error removing role: " + ex.Message);
                return RedirectToAction("ManageRoles", new { id = userId });
            }
        }
    }
}
