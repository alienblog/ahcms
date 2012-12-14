using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        public User()
        {
            
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 账户信息
        /// </summary>
        public virtual MemberShip MemberShip { get; set; }

        /// <summary>
        /// 用户配置
        /// </summary>
        public virtual ICollection<UserProfile> Profiles { get; set; }

        /// <summary>
        /// 用户三方授权信息
        /// </summary>
        public virtual ICollection<OAuthMemberShip> OAuthMemberShips { get; set; }
    }
}
