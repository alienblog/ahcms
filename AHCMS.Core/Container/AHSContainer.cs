using AHCMS.Core.Repository;
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using NH = NHibernate;

namespace AHCMS.Core.Container
{
    public class AHSContainer
    {
        private static Autofac.IContainer container;
        private static Autofac.ContainerBuilder builder;

        static AHSContainer()
        {
            builder = new Autofac.ContainerBuilder();
        }

        public static void RegisterControllers(params Assembly[] assemblies)
        {
            builder.RegisterControllers(assemblies);
            builder.RegisterAssemblyTypes(typeof(IRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            container = builder.Build();
        }

        public static void SetResolver()
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static Repository.IRepository ResolverRepository()
        {
            return container.Resolve<Repository.IRepository>();
        }
    }
}
