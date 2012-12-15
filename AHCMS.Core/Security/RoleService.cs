using AHCMS.Core.Repository;
using AHCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Core.Security
{
    public class RoleService
    {
        IRepository repository;

        public RoleService()
        {
            this.repository = Container.AHSContainer.ResolverRepository();
        }

        public void AddRolesToUser(string[] roleNames, string userName)
        {
            var userService = new UserService();

            var user = userService.GetUser(userName);
            if (user != null)
            {
                var roles = user.MemberShip.Roles;
                foreach (var item in roleNames)
                {
                    if (!roles.Select(x => x.RoleName).Contains(item))
                    {
                        var role = GetRole(item);
                        if (role != null)
                            user.MemberShip.Roles.Add(role);
                    }
                }
                userService.UpdateMemberShip(user.MemberShip);
            }
        }

        public void CreateRole(string rolename)
        {
            bool exists = RoleExists(rolename);
            if (!exists)
            {
                Role r = new Role();
                r.RoleName = rolename;
                repository.Save(r);
            }
        }

        public bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            var role = GetRole(rolename);

            if (role == null) return false;

            if (throwOnPopulatedRole && role.MemberShips.Count > 0) return false;

            repository.Delete<Role>(role);
            return true;
        }

        public bool RoleExists(string roleName)
        {
            return repository.Query<Role>()
                .Where(x => x.RoleName.Equals(roleName)).Count() > 0 ? true : false;
        }

        public Role GetRole(string roleName)
        {
            return repository.Query<Role>()
                .FirstOrDefault(x => x.RoleName.Equals(roleName, StringComparison.CurrentCulture));
        }

        public Role[] GetRoles(string[] roleNames)
        {
            return repository.Query<Role>()
                .Where(x => roleNames.Contains(x.RoleName)).ToArray();
        }

        public string[] GetAllRoles()
        {
            return repository.Query<Role>().Select(x => x.RoleName).ToArray();
        }

        public string[] GetRolesFromUser(string userName)
        {
            return repository.Query<User>().FirstOrDefault(x => x.UserName.Equals(userName))
                .MemberShip.Roles.Select(x => x.RoleName).ToArray();
        }

        public string[] GetUsersFromRole(string roleName)
        {
            return GetRole(roleName).MemberShips.Select(x => x.User)
                .Select(x => x.UserName).ToArray();
        }

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return GetRole(roleName).MemberShips.Select(x => x.User)
                .Where(x => x.UserName.Contains(usernameToMatch))
                .Select(x => x.UserName).ToArray();
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            return GetRole(roleName).MemberShips.Select(x=>x.User)
                .Where(x=>x.UserName.Equals(userName))
                .Count() > 0 ? true : false;
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var roles = GetRoles(roleNames);
            var users = repository.Query<User>().Where(x => usernames.Contains(x.UserName));
            foreach (var role in roles)
            {
                foreach (var user in users)
                {
                    if (role.MemberShips.Contains(user.MemberShip))
                    {
                        role.MemberShips.Remove(user.MemberShip);
                    }
                }
                repository.Update<Role>(role);
            }
        }
    }
}
