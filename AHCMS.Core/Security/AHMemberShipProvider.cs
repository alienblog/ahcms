using AHCMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using WebMatrix.WebData;

namespace AHCMS.Core.Security
{
    public class AHMemberShipProvider : ExtendedMembershipProvider
    {
        #region 字段

        private bool enablePasswordReset;
        private bool enablePasswordRetrieval;
        private int maxInvalidPasswordAttempts;
        private int minRequiredNonAlphanumericCharacters;
        private int minRequiredPasswordLength;
        private int passwordAttemptWindow;
        private System.Web.Security.MembershipPasswordFormat passwordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
        private string passwordStrengthRegularExpression;
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;

        private string s_HashAlgorithm;

        #endregion

        #region 属性

        public override bool EnablePasswordReset
        {
            get { return enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return enablePasswordRetrieval; }
        }

        public override string ApplicationName
        {
            get
            {
                return "ahcms";
            }
            set
            {

            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return passwordAttemptWindow; }
        }

        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get { return passwordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return passwordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return requiresUniqueEmail; }
        }

        #endregion

        /// <summary>
        /// 使用 ASP.NET 应用程序配置文件中指定的属性值初始化成员资格提供程序。
        /// 此方法不应从代码直接使用。
        /// </summary>
        /// <param name="name">要初始化的 ExtendedMembershipProvider 实例的名称。</param>
        /// <param name="config">
        /// 一个 NameValueCollection，其中包含成员资格提供程序配置选项的值和名称。 
        /// </param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
            {
                name = "AHMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "AHCMS.Framework.Security Membership Provider");
            }

            base.Initialize(name, config);

            this.enablePasswordRetrieval = SecUtility.GetBooleanValue(
                config, "enablePasswordRetrieval", false);
            this.enablePasswordReset = SecUtility.GetBooleanValue(
                config, "enablePasswordReset", true);
            this.requiresQuestionAndAnswer = SecUtility.GetBooleanValue(
                config, "requiresQuestionAndAnswer", true);
            this.requiresUniqueEmail = SecUtility.GetBooleanValue(
                config, "requiresUniqueEmail", true);
            this.maxInvalidPasswordAttempts = SecUtility.GetIntValue(
                config, "maxInvalidPasswordAttempts", 5, false, 0);
            this.passwordAttemptWindow = SecUtility.GetIntValue(
                config, "passwordAttemptWindow", 10, false, 0);
            this.minRequiredPasswordLength = SecUtility.GetIntValue(
                config, "minRequiredPasswordLength", 7, false, 128);
            this.minRequiredNonAlphanumericCharacters = SecUtility.GetIntValue(
                config, "minRequiredNonalphanumericCharacters", 1, true, 128);

            this.passwordStrengthRegularExpression =
                config["passwordStrengthRegularExpression"];
            if (this.passwordStrengthRegularExpression != null)
            {
                this.passwordStrengthRegularExpression =
                    this.passwordStrengthRegularExpression.Trim();
                if (this.passwordStrengthRegularExpression.Length != 0)
                {
                    try
                    {
                        new Regex(this.passwordStrengthRegularExpression);
                    }
                    catch (ArgumentException e)
                    {
                        throw new ProviderException(e.Message, e);
                    }
                }
            }
            else
            {
                this.passwordStrengthRegularExpression = String.Empty;
            }

            string strTemp = config["passwordFormat"];
            if (strTemp == null)
            {
                strTemp = "Hashed";
            }

            switch (strTemp)
            {
                case "Clear":
                    this.passwordFormat =
                        System.Web.Security.MembershipPasswordFormat.Clear;
                    break;
                case "Encrypted":
                    this.passwordFormat =
                        System.Web.Security.MembershipPasswordFormat.Encrypted;
                    break;
                case "Hashed":
                    this.passwordFormat =
                        System.Web.Security.MembershipPasswordFormat.Hashed;
                    break;
                default:
                    throw new ProviderException("Bad password format.");
            }

            if (this.passwordFormat == System.Web.Security.MembershipPasswordFormat.Hashed
                && this.enablePasswordRetrieval)
            {
                throw new ProviderException("Provider cannot retrieve hashed password.");
            }

            config.Remove("repository");
            config.Remove("applicationName");
            config.Remove("enablePasswordRetrieval");
            config.Remove("enablePasswordReset");
            config.Remove("requiresQuestionAndAnswer");
            config.Remove("requiresUniqueEmail");
            config.Remove("maxInvalidPasswordAttempts");
            config.Remove("passwordAttemptWindow");
            config.Remove("passwordFormat");
            config.Remove("name");
            config.Remove("description");
            config.Remove("minRequiredPasswordLength");
            config.Remove("minRequiredNonalphanumericCharacters");
            config.Remove("passwordStrengthRegularExpression");

            if (config.Count > 0)
            {
                string attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                {
                    throw new ProviderException(
                        "Provider unrecognized attribute: " + attribUnrecognized);
                }
            }
        }

        #region 方法成员

        /// <summary>
        /// 确认账户
        /// </summary>
        /// <param name="accountConfirmationToken"></param>
        /// <returns></returns>
        public override bool ConfirmAccount(string accountConfirmationToken)
        {
            int status = new UserService().ConfirmAccount(accountConfirmationToken);
            if (status != 0)
            {
                return false;
            }
            return true;
        }

        public override bool ConfirmAccount(string userName, string accountConfirmationToken)
        {
            int status = new UserService().ConfirmAccount(userName, accountConfirmationToken);
            if (status != 0)
            {
                return false;
            }
            return true;
        }

        public override bool HasLocalAccount(int userId)
        {
            return new UserService().HasLocalAccount(userId);
        }

        public override string CreateAccount(string userName, string password, bool requireConfirmationToken)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
            }
            var salt = this.GenerateSalt();
            string hashedPassword = this.EncodePassword(password, this.PasswordFormat, salt);
            if (hashedPassword.Length > 0x80)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
            }
            if (string.IsNullOrEmpty(userName))
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidUserName);
            }
            return new UserService().CreateAccount(userName, password, salt, requireConfirmationToken);
        }

        public override string CreateUserAndAccount(string userName, string password, bool requireConfirmation, IDictionary<string, object> values)
        {
            var user = new User();
            user.UserName = userName;
            foreach (var value in values)
            {
                user.Profiles.Add(new UserProfile
                {
                    ProfileKey = value.Key,
                    ProfileValue = value.Value.ToString()
                });
            }
            new UserService().CreateUser(user);
            return this.CreateAccount(userName, password);
        }

        public override bool DeleteAccount(string userName)
        {
            return new UserService().DeleteAccount(userName);
        }

        public override string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow)
        {
            var user = new UserService().GetUser(userName);
            if (user == null || user.MemberShip == null)
            {
                throw new ProviderException("本地帐号不存在");
            }
            if (user.MemberShip.PasswordVerificationTokenExpirationDate > DateTime.UtcNow)
            {
                return user.MemberShip.PasswordVerificationToken;
            }
            string token = UserService.GenerateToken();
            user.MemberShip.PasswordVerificationToken = token;
            user.MemberShip.PasswordVerificationTokenExpirationDate = DateTime.UtcNow.AddMinutes((double)tokenExpirationInMinutesFromNow);
            new UserService().UpdateMemberShip(user.MemberShip);
            return token;
        }

        public override ICollection<OAuthAccountData> GetAccountsForUser(string userName)
        {
            var user = new UserService().GetUser(userName);
            if (user == null)
            {
                throw new ProviderException("用户不存在");
            }
            List<OAuthAccountData> oalist = new List<OAuthAccountData>();
            user.OAuthMemberShips.ToList().ForEach(o =>
            {
                oalist.Add(new OAuthAccountData(o.Provider, o.ProviderUserId));
            });
            return oalist;
        }

        public override DateTime GetCreateDate(string userName)
        {
            var user = new UserService().GetUser(userName);
            return user.MemberShip.CreateDate;
        }

        public override DateTime GetLastPasswordFailureDate(string userName)
        {
            var user = new UserService().GetUser(userName);
            return user.MemberShip.LastPasswordFailureDate;
        }

        public override DateTime GetPasswordChangedDate(string userName)
        {
            var user = new UserService().GetUser(userName);
            return user.MemberShip.PasswordChangedDate;
        }

        public override int GetPasswordFailuresSinceLastSuccess(string userName)
        {
            var user = new UserService().GetUser(userName);
            return user.MemberShip.PasswordFailuresSinceLastSuccess;
        }

        public override int GetUserIdFromPasswordResetToken(string token)
        {
            return new UserService().GetUserIdFromPasswordResetToken(token);
        }

        public override bool IsConfirmed(string userName)
        {
            return new UserService().IsConfirmed(userName);
        }

        public override bool ResetPasswordWithToken(string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (ValidateUser(username, oldPassword))
            {
                var user = new UserService().GetUser(username);
                user.MemberShip.Password = EncodePassword(newPassword, this.PasswordFormat, user.MemberShip.PasswordSalt);
                new UserService().UpdateMemberShip(user.MemberShip);
            }
            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            if (ValidateUser(username, password))
            {
                var user = new UserService().GetUser(username);
                user.MemberShip.PasswordQuestion = newPasswordQuestion;
                user.MemberShip.QuestionAnswer = EncodePassword(newPasswordAnswer, this.PasswordFormat, user.MemberShip.PasswordSalt);
                new UserService().UpdateMemberShip(user.MemberShip);
                return true;
            }
            return false;
        }

        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            throw new NotImplementedException();    
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var service = new UserService();
            var user = service.GetUser(username);
            if (user == null)
                return false;
            if (deleteAllRelatedData)
                service.DeleteAccount(username);
            service.DeleteUser(user);
            return true;
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        { 
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            var user = new UserService().GetUser(email);
            if(user==null)
                return string.Empty;
            return user.UserName;
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            var user = new UserService().GetUser(userName);
            if (user == null)
                return false;
            user.MemberShip.IsLockOut = false;
            new UserService().UpdateMemberShip(user.MemberShip);
            return true;
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var user = new UserService().GetUser(username);
            if (user == null) return false;
            string pwd = EncodePassword(password, this.PasswordFormat, user.MemberShip.PasswordSalt);
            return pwd == user.MemberShip.Password;
        }
        #endregion

        #region 辅助方法

        private bool IsStatusDueToBadPassword(int status)
        {
            if ((status >= 2) && (status <= 6))
            {
                return true;
            }
            return (status == 99);
        }

        private string GenerateSalt()
        {
            byte[] buffer = new byte[16];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        private DateTime RoundToSeconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        /// <summary>
        /// 获取加密后的安全答案
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="passwordAnswer">安全答案</param>
        /// <returns></returns>
        private string GetEncodedPasswordAnswer(string username, string passwordAnswer)
        {
            if (passwordAnswer != null)
            {
                passwordAnswer = passwordAnswer.Trim();
            }

            if (!String.IsNullOrEmpty(passwordAnswer))
            {
                int status;
                string password;
                string salt;
                int failedPasswordAttemptCount;
                int failedPasswordAnswerAttemptCount;
                DateTime lastLoginDate;
                DateTime lastActivityDate;

                GetPasswordWithFormat(username, false, out status, out password,
                   out salt, out failedPasswordAttemptCount, out failedPasswordAnswerAttemptCount,
                     out lastLoginDate, out lastActivityDate);

                if (status != 0)
                {
                    throw new ProviderException(GetExceptionText(status));
                }

                return EncodePassword(passwordAnswer.ToLower(
                    System.Globalization.CultureInfo.InvariantCulture), PasswordFormat, salt);
            }

            return passwordAnswer;
        }

        /// <summary>
        /// 生成随即密码
        /// </summary>
        /// <returns></returns>
        private string GeneratePassword()
        {
            return System.Web.Security.Membership.GeneratePassword(
                (this.minRequiredPasswordLength < 14) ? 14 : this.minRequiredPasswordLength,
                this.minRequiredNonAlphanumericCharacters);
        }

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="pass">密码</param>
        /// <param name="passwordFormat">加密方式</param>
        /// <param name="salt">加密字符串</param>
        /// <returns></returns>
        private string EncodePassword(string pass,
            System.Web.Security.MembershipPasswordFormat passwordFormat, string salt)
        {
            if (passwordFormat == System.Web.Security.MembershipPasswordFormat.Clear)
            {
                return pass;
            }

            byte[] bIn = System.Text.Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bRet = null;

            if (passwordFormat == System.Web.Security.MembershipPasswordFormat.Hashed)
            {
                System.Security.Cryptography.HashAlgorithm hashAlgorithm = this.GetHashAlgorithm();
                if (hashAlgorithm is System.Security.Cryptography.KeyedHashAlgorithm)
                {
                    System.Security.Cryptography.KeyedHashAlgorithm keyedHashAlgorithm =
                        (System.Security.Cryptography.KeyedHashAlgorithm)hashAlgorithm;
                    if (keyedHashAlgorithm.Key.Length == bSalt.Length)
                    {
                        keyedHashAlgorithm.Key = bSalt;
                    }
                    else
                    {
                        if (keyedHashAlgorithm.Key.Length < bSalt.Length)
                        {
                            byte[] bKey = new byte[keyedHashAlgorithm.Key.Length];
                            Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
                            keyedHashAlgorithm.Key = bKey;
                        }
                        else
                        {
                            byte[] bKey = new byte[keyedHashAlgorithm.Key.Length];
                            int num;
                            for (int i = 0; i < bKey.Length; i += num)
                            {
                                num = Math.Min(bSalt.Length, bKey.Length - i);
                                Buffer.BlockCopy(bSalt, 0, bKey, i, num);
                            }
                            keyedHashAlgorithm.Key = bKey;
                        }
                    }
                    bRet = keyedHashAlgorithm.ComputeHash(bIn);
                }
                else
                {
                    byte[] bAll = new byte[bSalt.Length + bIn.Length];
                    Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
                    Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
                    bRet = hashAlgorithm.ComputeHash(bAll);
                }
            }

            else //System.Web.Security.MembershipPasswordFormat.Encrypted
            {
                byte[] bAll = new byte[bSalt.Length + bIn.Length];
                Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
                Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
                bRet = this.EncryptPassword(bAll);
            }

            return Convert.ToBase64String(bRet);
        }

        private System.Security.Cryptography.HashAlgorithm GetHashAlgorithm()
        {
            if (this.s_HashAlgorithm != null)
            {
                return System.Security.Cryptography.HashAlgorithm.Create(this.s_HashAlgorithm);
            }
            string type = System.Web.Security.Membership.HashAlgorithmType;

            System.Security.Cryptography.HashAlgorithm hashAlgorithm =
                System.Security.Cryptography.HashAlgorithm.Create(type);
            if (hashAlgorithm == null)
            {
                throw new ProviderException("无法创建哈希算法。");
            }
            this.s_HashAlgorithm = type;
            return hashAlgorithm;
        }


        private string UnEncodePassword(string pass,
            System.Web.Security.MembershipPasswordFormat passwordFormat)
        {
            switch (passwordFormat)
            {
                case System.Web.Security.MembershipPasswordFormat.Clear:
                    return pass;

                case System.Web.Security.MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Provider 不能解密哈希加密的密码。");
            }

            byte[] buffer = this.DecryptPassword(Convert.FromBase64String(pass));

            if (buffer == null)
            {
                return null;
            }

            return System.Text.Encoding.Unicode.GetString(buffer, 16, buffer.Length - 16);
        }

        private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate,
            bool failIfNotApproved)
        {
            string salt;
            return CheckPassword(username, password, updateLastLoginActivityDate, failIfNotApproved,
                out salt);
        }

        private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved,
            out string salt)
        {
            int status;
            string pass;
            int failedPasswordAttemptCount;
            int failedPasswordAnswerAttemptCount;
            DateTime lastLoginDate;
            DateTime lastActivityDate;


            GetPasswordWithFormat(username, updateLastLoginActivityDate, out status,
                out pass, out salt, out failedPasswordAttemptCount,
                out failedPasswordAnswerAttemptCount, out lastLoginDate, out  lastActivityDate);

            if (status != 0)
            {
                return false;
            }
            if (failIfNotApproved)
            {
                return false;
            }

            password = EncodePassword(password, passwordFormat, salt);

            bool isPasswordCorrect = password.Equals(pass);

            if ((isPasswordCorrect && (failedPasswordAttemptCount == 0)) && (failedPasswordAnswerAttemptCount == 0))
            {
                return true;
            }

            //////////////////////////////////////////////////////////////////////////

            return isPasswordCorrect;

        }

        private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status,
             out string password,
            out string passwordSalt,
             out int failedPasswordAttemptCount, out int failedPasswordAnswerAttemptCount, out DateTime lastActivityDate,
             out DateTime lastLoginDate)
        {
            User user = new UserService().GetUser(username);

            if (user != null && user.MemberShip != null)
            {
                if (updateLastLoginActivityDate)
                {
                    user.MemberShip.LastActivityDate = DateTime.UtcNow;
                    user.MemberShip.LastLoginDate = DateTime.UtcNow;

                    new UserService().UpdateUser(user);
                }

                password = user.MemberShip.Password;
                passwordSalt = user.MemberShip.PasswordSalt;
                failedPasswordAttemptCount = user.MemberShip.PasswordFailuresSinceLastSuccess;
                failedPasswordAnswerAttemptCount = user.MemberShip.AnswerFailureCount;
                lastLoginDate = user.MemberShip.LastLoginDate;
                lastActivityDate = user.MemberShip.LastActivityDate;

                status = 0;
            }
            else
            {
                password = null;
                passwordFormat = System.Web.Security.MembershipPasswordFormat.Clear;
                passwordSalt = null;
                failedPasswordAttemptCount = 0;
                failedPasswordAnswerAttemptCount = 0;
                lastLoginDate = DateTime.UtcNow;
                lastActivityDate = DateTime.UtcNow;

                status = 1;
            }
        }

        public User MemberShipUserToUserEntity(MembershipUser mu)
        {
            User u = new User();
            u.UserName = mu.UserName;
            u.UserId = (int)mu.ProviderUserKey;
            u.MemberShip.PasswordQuestion = mu.PasswordQuestion;
            u.MemberShip.CreateDate = mu.CreationDate;
            u.MemberShip.PasswordChangedDate = mu.LastPasswordChangedDate;
            u.MemberShip.LastActivityDate = mu.LastActivityDate;
            u.MemberShip.LastLoginDate = mu.LastLoginDate;
            u.MemberShip.IsLockOut = mu.IsLockedOut;
            
            return u;
        }

        public MembershipUser UserEntityToMemberShipUser(User u)
        {
            MembershipUser user = new MembershipUser(u.UserName, u.UserName, u.UserId, u.UserName,
                u.MemberShip.PasswordQuestion, "", true, u.MemberShip.IsLockOut, u.MemberShip.CreateDate,
                u.MemberShip.LastLoginDate, u.MemberShip.LastActivityDate, u.MemberShip.PasswordChangedDate,
                u.MemberShip.LastActivityDate);
            return user;
        }

        private string GetExceptionText(int status)
        {
            string errText;
            switch (status)
            {
                case 0:
                    return String.Empty;

                case 1:
                    errText = "用户账户不存在。";
                    break;

                case 2:
                    errText = "账户密码错误。";
                    break;

                case 3:
                    errText = "安全问题错误。";
                    break;

                case 4:
                    errText = "无效的账户密码。";
                    break;

                case 5:
                    errText = "无效的安全问题。";
                    break;

                case 6:
                    errText = "无效的安全问题答案。";
                    break;

                case 7:
                    errText = "无效的邮箱地址";
                    break;

                case 99:
                    errText = "账户已经锁定。";
                    break;

                default:
                    errText = "Provider 错误。";
                    break;
            }
            return errText;
        }

        #endregion
    }
}
