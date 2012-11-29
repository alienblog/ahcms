using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    public class Category : Entity
    {
        public Category()
        {
            Children = new List<Category>();
            Contents = new List<Content>();
        }

        /// <summary>
        /// 分类名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public virtual string Link { get; set; }

        /// <summary>
        /// 父分类
        /// </summary>
        public virtual Category Parent { get; set; }

        /// <summary>
        /// 子分类
        /// </summary>
        public virtual ICollection<Category> Children { get; set; }

        /// <summary>
        /// 所含内容
        /// </summary>
        public virtual ICollection<Content> Contents { get; set; }
    }
}
