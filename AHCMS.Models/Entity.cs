using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Models
{
    public abstract class Entity
    {
        public virtual Guid Id { get; set; }
    }
}
