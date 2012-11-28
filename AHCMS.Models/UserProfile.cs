using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    /// <summary>
    /// 用户配置
    /// </summary>
    public class UserProfile : Entity
    {
        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// 配置Key
        /// </summary>
        public virtual string ProfileKey { get; set; }

        /// <summary>
        /// 配置值
        /// </summary>
        public virtual string ProfileValue { get; set; }
    }
}
