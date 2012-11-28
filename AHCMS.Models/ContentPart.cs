using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    /// <summary>
    /// 内容块
    /// </summary>
    public class ContentPart : Entity
    {
        public ContentPart()
        {
            Types = new List<ContentType>();
        }

        /// <summary>
        /// 内容块名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 编辑器名称
        /// </summary>
        public virtual string InputName { get; set; }

        /// <summary>
        /// 所属类型
        /// </summary>
        public virtual ICollection<ContentType> Types { get; set; }
    }
}
