using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AHCMS.Core.Repository;
using AHCMS.Models;
using System.Security.Cryptography;
using System.Web;

namespace AHCMS.Core.Security
{
    public class UserService
    {
        IRepository repository;

        public UserService()
        {
            repository = Container.AHSContainer.ResolverRepository();
        }

        public void CreateUser(User user)
        {
            repository.Save<User>(user);
        }

        public void UpdateUser(User user)
        {
            repository.Update<User>(user);
        }

        public void UpdateMemberShip(MemberShip membership)
        {
            repository.Update<MemberShip>(membership);
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
                .Where(x => x.UserName == username)
                .Where(x => x.MemberShip.ConfirmationToken == accountConfirmationToken)
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

        public bool IsConfirmed(string username)
        {
            var user = GetUser(username);
            if (user == null || user.MemberShip == null)
            {
                return false;
            }
            return user.MemberShip.IsConfirmed;
        }

        public bool HasLocalAccount(int userId)
        {
            var user = repository.Query<User>()
                .FirstOrDefault(x => x.UserId == userId);
            return user.MemberShip != null;
        }

        public string CreateAccount(string username, string password, string salt, bool requireConfirmationToken)
        {
            var user = GetUser(username);
            if (user == null)
            {
                throw new ArgumentNullException("不存在此用户");
            }
            string token = null;
            if (requireConfirmationToken)
            {
                token = GenerateToken();
            }
            var membership = new MemberShip
            {
                UserId = user.UserId,
                User = user,
                Password = password,
                PasswordSalt = salt,
                CreateDate = DateTime.UtcNow,
                ConfirmationToken = token,
                PasswordChangedDate = DateTime.UtcNow,
                PasswordFailuresSinceLastSuccess = 0
            };
            repository.Save<MemberShip>(membership);
            return token;
        }

        public bool DeleteAccount(string username)
        {
            var user = GetUser(username);
            if (user.MemberShip == null)
                return false;
            repository.Delete<MemberShip>(user.MemberShip);
            return false;
        }

        public int GetUserIdFromPasswordResetToken(string token)
        {
            var user = repository.Query<User>()
                .Where(x => x.MemberShip.PasswordVerificationToken == token).FirstOrDefault();
            if (user != null||user.MemberShip!=null)
            {
                return user.UserId;
            }
            return -1;
        }

        #region Helpers

        internal static string GenerateToken()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                return GenerateToken(provider);
            }
        }

        internal static string GenerateToken(RandomNumberGenerator generator)
        {
            var data = new byte[0x10];
            generator.GetBytes(data);
            return HttpServerUtility.UrlTokenEncode(data);
        }

        #endregion
    }
}
