using AHCMS.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NH = NHibernate;
using NHibernate.Cfg;
using NHibernate;
using System.Web;

namespace AHCMS.Core.NHibernate
{
    public class NHibernateManager
    {
        private const string CurrentSessionKey = "nhibernate.current_session";

        private static Logger logger;

        private static Configuration configuration;
        private static ISessionFactory sessionFactory;

        /// <summary>
        /// Session工厂
        /// </summary>
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    sessionFactory = configuration.BuildSessionFactory();
                }
                return sessionFactory;
            }
        }

        static NHibernateManager() { }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="configFileName">配置文件地址</param>
        public static void Configuration(string configFileName = "hibernate.cfg.xml")
        {
            logger = Logger.GetLogger(typeof(NHibernateManager));

            configuration = new Configuration();
            string path = HttpContext.Current.Server.MapPath("~/" + configFileName);
            configuration.Configure(path);
        }

        /// <summary>
        /// 更新数据库结构
        /// </summary>
        public static void UpdateSchema()
        {
            NH.Tool.hbm2ddl.SchemaUpdate su = new NH.Tool.hbm2ddl.SchemaUpdate(configuration);
            su.Execute(true, true);
        }

        /// <summary>
        /// 获取当前会话
        /// </summary>
        /// <returns></returns>
        public static NH.ISession GetCurrentSession()
        {
            HttpContext context = HttpContext.Current;
            NH.ISession currentSession = context.Items[CurrentSessionKey] as NH.ISession;

            if (currentSession == null)
            {
                currentSession = SessionFactory.OpenSession();
                context.Items[CurrentSessionKey] = currentSession;
            }

            return currentSession;
        }

        /// <summary>
        /// 关闭当前会话
        /// </summary>
        public static void CloseSession()
        {
            HttpContext context = HttpContext.Current;
            NH.ISession currentSession = context.Items[CurrentSessionKey] as NH.ISession;

            if (currentSession == null)
            {
                return;
            }

            currentSession.Close();
            context.Items.Remove(CurrentSessionKey);
        }

        /// <summary>
        /// 关闭Session工厂
        /// </summary>
        public static void CloseSessionFactory()
        {
            if (sessionFactory != null)
            {
                sessionFactory.Close();
            }
        }
    }
}
