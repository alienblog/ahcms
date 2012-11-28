using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCMS.Core.Repository
{
    public interface ITransaction:IDisposable
    {
        void Commit();
        void RollBack();
    }
}
