using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    public class OAuthMemberShip : Entity
    {
        /// <summary>
        /// 提供者
        /// </summary>
        public virtual string Provider { get; set; }

        /// <summary>
        /// 提供者用户Id
        /// </summary>
        public virtual string ProviderUserId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual User User { get; set; }
    }
}
