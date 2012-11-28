using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Role : Entity
    {
        public Role()
        {
            MemberShips = new List<MemberShip>();
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public virtual string RoleName { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual ICollection<MemberShip> MemberShips { get; set; }
    }
}
