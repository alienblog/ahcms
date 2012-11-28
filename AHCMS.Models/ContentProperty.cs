using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    public class ContentProperty : Entity
    {
        /// <summary>
        /// 所属内容块
        /// </summary>
        public virtual ContentPart Part { get; set; }

        /// <summary>
        /// 所属内容
        /// </summary>
        public virtual Content Content { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public virtual string PropertyValue { get; set; }
    }
}
