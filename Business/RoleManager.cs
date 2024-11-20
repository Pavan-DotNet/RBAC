
using System;
using System.Collections.Generic;
using MOCDIntegrations.Models;
using MOCDIntegrations.Auth;

namespace MOCDIntegrations.Business
{
    public class RoleManager
    {
        private readonly RoleDataAccess _roleDataAccess;

        public RoleManager(string connectionString)
        {
            _roleDataAccess = new RoleDataAccess(connectionString);
        }

        public List<Role> GetAllRoles()
        {
            return _roleDataAccess.GetAllRoles();
        }

        public Role GetRoleById(int roleId)
        {
            return _roleDataAccess.GetRoleById(roleId);
        }

        public int CreateRole(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new ArgumentException("Role name cannot be empty or null.");
            }

            return _roleDataAccess.CreateRole(role);
        }

        public void UpdateRole(Role role)
        {
            if (role.RoleId <= 0)
            {
                throw new ArgumentException("Invalid role ID.");
            }

            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new ArgumentException("Role name cannot be empty or null.");
            }

            _roleDataAccess.UpdateRole(role);
        }

        public void DeleteRole(int roleId)
        {
            if (roleId <= 0)
            {
                throw new ArgumentException("Invalid role ID.");
            }

            _roleDataAccess.DeleteRole(roleId);
        }
    }
}
