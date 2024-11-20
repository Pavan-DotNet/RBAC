
using System;
using System.Collections.Generic;
using MOCDIntegrations.Models;
using MOCDIntegrations.Auth;

namespace MOCDIntegrations.Business
{
    public class UserManager
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly UserRoleDataAccess _userRoleDataAccess;

        public UserManager(string connectionString)
        {
            _userDataAccess = new UserDataAccess(connectionString);
            _userRoleDataAccess = new UserRoleDataAccess(connectionString);
        }

        public User GetUserById(int userId)
        {
            return _userDataAccess.GetUserById(userId);
        }

        public List<User> GetAllUsers()
        {
            return _userDataAccess.GetAllUsers();
        }

        public int CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException("Username cannot be empty or null.");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Email cannot be empty or null.");
            }

            // TODO: Add password validation and hashing
            return _userDataAccess.CreateUser(user);
        }

        public void UpdateUser(User user)
        {
            if (user.UserId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException("Username cannot be empty or null.");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Email cannot be empty or null.");
            }

            _userDataAccess.UpdateUser(user);
        }

        public void DeleteUser(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            _userDataAccess.DeleteUser(userId);
        }

        public List<UserRole> GetUserRoles(int userId)
        {
            return _userRoleDataAccess.GetUserRoles(userId);
        }

        public void AssignRoleToUser(int userId, int roleId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            if (roleId <= 0)
            {
                throw new ArgumentException("Invalid role ID.");
            }

            _userRoleDataAccess.AssignRoleToUser(userId, roleId);
        }

        public void RemoveRoleFromUser(int userId, int roleId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            if (roleId <= 0)
            {
                throw new ArgumentException("Invalid role ID.");
            }

            _userRoleDataAccess.RemoveRoleFromUser(userId, roleId);
        }

        public bool UserHasRole(int userId, int roleId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            if (roleId <= 0)
            {
                throw new ArgumentException("Invalid role ID.");
            }

            return _userRoleDataAccess.UserHasRole(userId, roleId);
        }
    }
}
