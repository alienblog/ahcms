using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace AHCMS.Core.Security
{
    public class AHRoleProvider : RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var service = new RoleService();
            foreach (var item in usernames)
            {
                service.AddRolesToUser(roleNames, item);
            }
        }

        public override string ApplicationName
        {
            get;
            set;
        }

        public override void CreateRole(string roleName)
        {
            new RoleService().CreateRole(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return new RoleService().DeleteRole(roleName, throwOnPopulatedRole);
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return new RoleService().FindUsersInRole(roleName, usernameToMatch);
        }

        public override string[] GetAllRoles()
        {
            return new RoleService().GetAllRoles();
        }

        public override string[] GetRolesForUser(string username)
        {
            return new RoleService().GetRolesFromUser(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return new RoleService().GetUsersFromRole(roleName);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return new RoleService().IsUserInRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            new RoleService().RemoveUsersFromRoles(usernames, roleNames);
        }

        public override bool RoleExists(string roleName)
        {
            return new RoleService().RoleExists(roleName);
        }
    }
}
