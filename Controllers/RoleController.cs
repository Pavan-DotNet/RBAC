
using System;
using System.Web.Mvc;
using MOCDIntegrations.Auth;
using MOCDIntegrations.Business;
using MOCDIntegrations.Models;

namespace MOCDIntegrations.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager _roleManager;

        public RoleController()
        {
            _roleManager = new RoleManager(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Index()
        {
            var roles = _roleManager.GetAllRoles();
            return View(roles);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RoleBasedAuthorize("Admin")]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _roleManager.CreateRole(role);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating role: " + ex.Message);
                }
            }
            return View(role);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Edit(int id)
        {
            var role = _roleManager.GetRoleById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost]
        [RoleBasedAuthorize("Admin")]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _roleManager.UpdateRole(role);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating role: " + ex.Message);
                }
            }
            return View(role);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Delete(int id)
        {
            var role = _roleManager.GetRoleById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [RoleBasedAuthorize("Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _roleManager.DeleteRole(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting role: " + ex.Message);
                return View(_roleManager.GetRoleById(id));
            }
        }
    }
}
