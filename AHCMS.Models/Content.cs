using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    /// <summary>
    /// 内容
    /// </summary>
    public class Content : Entity
    {
        public Content()
        {
            Properties = new List<ContentProperty>();
        }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        public virtual DateTime EditDate { get; set; }

        /// <summary>
        /// 所属类型
        /// </summary>
        public virtual ContentType Type { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public virtual ICollection<ContentProperty> Properties { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public virtual Category Category { get; set; }
    }
}
