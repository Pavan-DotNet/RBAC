

using System;
using System.Web.Mvc;
using MOCDIntegrations.Auth;

namespace MOCDIntegrations.Controllers
{
    [RoleAuthorize("Admin")]
    public class RoleManagementController : Controller
    {
        private readonly RoleManager _roleManager;

        public RoleManagementController()
        {
            _roleManager = new RoleManager();
        }

        // GET: RoleManagement
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        // GET: RoleManagement/Manage
        public ActionResult Manage()
        {
            var roles = _roleManager.GetAllRoles();
            return View(roles);
        }

        // POST: RoleManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string roleName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _roleManager.CreateRole(roleName);
                    return RedirectToAction("Manage");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating role: " + ex.Message);
                }
            }
            return RedirectToAction("Manage");
        }

        // GET: RoleManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            return View(id);
        }

        // POST: RoleManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                _roleManager.DeleteRole(id);
                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting role: " + ex.Message);
                return View(id);
            }
        }
    }
}

