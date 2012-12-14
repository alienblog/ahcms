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

            }
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
    }
}
