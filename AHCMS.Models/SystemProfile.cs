using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public class SystemProfile : Entity
    {
        /// <summary>
        /// 设置Key
        /// </summary>
        public virtual string ProfileKey { get; set; }

        /// <summary>
        /// 设置值
        /// </summary>
        public virtual string ProfileValue { get; set; }
    }
}
