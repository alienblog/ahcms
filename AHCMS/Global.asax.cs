using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AHCMS.Core.NHibernate;
using AHCMS.Core.Logging;
using AHCMS.Core.Container;
using System.Reflection;

namespace AHCMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Logger.ConfigureLog4Net(HttpContext.Current.Server.MapPath("~/log"), LogLevel.Info, LogLevel.Info);

            NHibernateManager.Configuration();
            NHibernateManager.UpdateSchema();

            AHSContainer.RegisterControllers(Assembly.GetExecutingAssembly());
            AHSContainer.SetResolver();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}