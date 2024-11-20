
using System;
using System.Web.Mvc;
using MOCDIntegrations.Auth;
using MOCDIntegrations.Business;
using MOCDIntegrations.Models;

namespace MOCDIntegrations.Controllers
{
    [Authorize]
    public class PermissionController : Controller
    {
        private readonly PermissionManager _permissionManager;

        public PermissionController()
        {
            _permissionManager = new PermissionManager(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Index()
        {
            var permissions = _permissionManager.GetAllPermissions();
            return View(permissions);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RoleBasedAuthorize("Admin")]
        public ActionResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _permissionManager.CreatePermission(permission);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating permission: " + ex.Message);
                }
            }
            return View(permission);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Edit(int id)
        {
            var permission = _permissionManager.GetPermissionById(id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            return View(permission);
        }

        [HttpPost]
        [RoleBasedAuthorize("Admin")]
        public ActionResult Edit(Permission permission)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _permissionManager.UpdatePermission(permission);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating permission: " + ex.Message);
                }
            }
            return View(permission);
        }

        [RoleBasedAuthorize("Admin")]
        public ActionResult Delete(int id)
        {
            var permission = _permissionManager.GetPermissionById(id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            return View(permission);
        }

        [HttpPost, ActionName("Delete")]
        [RoleBasedAuthorize("Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _permissionManager.DeletePermission(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting permission: " + ex.Message);
                return View(_permissionManager.GetPermissionById(id));
            }
        }
    }
}
