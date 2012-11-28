using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AHCMS.Core.Repository;
using AHCMS.Models;

namespace AHCMS.Core.Security
{
    public class UserService
    {
        IRepository repository;

        public UserService()
        {
            repository = new Repository.Repository();
        }

        public void CreateUser(User user)
        {
            repository.Save<User>(user);
        }

        public void UpdateUser(User user)
        {
            repository.Update<User>(user);
        }

        public void DeleteUser(User user)
        {
            repository.Delete<User>(user);
        }

        public User GetUser(int userID)
        {
            return repository.Get<User>(userID);
        }

        public User GetUser(string userName)
        {
            return repository.Query<User>().Where(x => x.UserName == userName).FirstOrDefault();
        }

        public bool ValidateUser(string username, string password)
        {

            return false;
        }

        public int ConfirmAccount(string accountConfirmationToken)
        {
            int status = 99;
            var membership = repository.Query<MemberShip>()
                .Where(x => x.ConfirmationToken == accountConfirmationToken)
                .FirstOrDefault();

            if (membership == null)
            {
                return 1;
            }

            membership.IsConfirmed = true;
            try
            {
                repository.Update<MemberShip>(membership);
                status = 0;
            }
            catch (System.Exception)
            {
            	    
            }

            return status;
        }

        public int ConfirmAccount(string username, string accountConfirmationToken)
        {
            int status = 99;
            var user = repository.Query<User>()
                .Where(x=>x.UserName == username)
                .Where(x=>x.MemberShip.ConfirmationToken == accountConfirmationToken)
                .FirstOrDefault();

            if (user == null)
            {
                return 1;
            }

            user.MemberShip.IsConfirmed = true;
            try
            {
                repository.Update<User>(user);
                status = 0;
            }
            catch (System.Exception)
            {

            }

            return status;
        }
    }
}
