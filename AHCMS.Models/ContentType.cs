using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    /// <summary>
    /// 内容类型
    /// </summary>
    public class ContentType : Entity
    {
        public ContentType()
        {
            Contents = new List<Content>();
            Parts = new List<ContentPart>();
        }

        /// <summary>
        /// 类型名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual ICollection<Content> Contents { get; set; }

        /// <summary>
        /// 拥有内容块
        /// </summary>
        public virtual ICollection<ContentPart> Parts { get; set; }
    }
}
