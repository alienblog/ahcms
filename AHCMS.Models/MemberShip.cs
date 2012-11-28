using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    /// <summary>
    /// 账户信息
    /// </summary>
    public class MemberShip : Entity
    {
        public MemberShip()
        {
            Roles = new List<Role>();
            Contents = new List<Content>();
        }

        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 账户确认Token
        /// </summary>
        public virtual string ConfirmationToken { get; set; }

        /// <summary>
        /// 是否已经确认账户
        /// </summary>
        public virtual bool IsConfirmed { get; set; }

        /// <summary>
        /// 最后密码失败日期
        /// </summary>
        public virtual DateTime LastPasswordFailureDate { get; set; }

        /// <summary>
        /// 安全问题
        /// </summary>
        public virtual string PasswordQuestion { get; set; }

        /// <summary>
        /// 安全问题答案
        /// </summary>
        public virtual string QuestionAnswer { get; set; }

        /// <summary>
        /// 最后安全问题错误日期
        /// </summary>
        public virtual DateTime LastAnswerFailureDate { get; set; }

        /// <summary>
        /// 安全答案错误次数
        /// </summary>
        public virtual int AnswerFailureCount { get; set; }

        /// <summary>
        /// 最后一次成功输入密码后错误次数
        /// </summary>
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 密码修改日期
        /// </summary>
        public virtual DateTime PasswordChangedDate { get; set; }

        /// <summary>
        /// 加密字符串
        /// </summary>
        public virtual string PasswordSalt { get; set; }

        /// <summary>
        /// 密码验证Token
        /// </summary>
        public virtual string PasswordVerificationToken { get; set; }

        /// <summary>
        /// 密码验证Token过期时间
        /// </summary>
        public virtual DateTime PasswordVerificationTokenExpirationDate { get; set; }

        /// <summary>
        /// 帐号是否锁定
        /// </summary>
        public virtual bool IsLockOut { get; set; }

        public virtual DateTime LastActivityDate { get; set; }

        public virtual DateTime LastLoginDate { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// 发表内容
        /// </summary>
        public virtual ICollection<Content> Contents { get; set; }
    }
}
