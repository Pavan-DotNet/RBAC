
using System;
using System.Collections.Generic;
using MOCDIntegrations.Models;
using MOCDIntegrations.Auth;

namespace MOCDIntegrations.Business
{
    public class PermissionManager
    {
        private readonly PermissionDataAccess _permissionDataAccess;

        public PermissionManager(string connectionString)
        {
            _permissionDataAccess = new PermissionDataAccess(connectionString);
        }

        public List<Permission> GetAllPermissions()
        {
            return _permissionDataAccess.GetAllPermissions();
        }

        public Permission GetPermissionById(int permissionId)
        {
            return _permissionDataAccess.GetPermissionById(permissionId);
        }

        public int CreatePermission(Permission permission)
        {
            if (string.IsNullOrWhiteSpace(permission.PermissionName))
            {
                throw new ArgumentException("Permission name cannot be empty or null.");
            }

            return _permissionDataAccess.CreatePermission(permission);
        }

        public void UpdatePermission(Permission permission)
        {
            if (permission.PermissionId <= 0)
            {
                throw new ArgumentException("Invalid permission ID.");
            }

            if (string.IsNullOrWhiteSpace(permission.PermissionName))
            {
                throw new ArgumentException("Permission name cannot be empty or null.");
            }

            _permissionDataAccess.UpdatePermission(permission);
        }

        public void DeletePermission(int permissionId)
        {
            if (permissionId <= 0)
            {
                throw new ArgumentException("Invalid permission ID.");
            }

            _permissionDataAccess.DeletePermission(permissionId);
        }
    }
}
