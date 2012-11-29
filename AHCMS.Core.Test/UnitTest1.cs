using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AHCMS.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            NHibernate.NHibernateManager.Configuration();
            NHibernate.NHibernateManager.UpdateSchema();
            
        }
    }
}
