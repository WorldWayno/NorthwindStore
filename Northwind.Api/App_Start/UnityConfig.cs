using System.Configuration;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Northwind.Data;
using Unity.WebApi;

namespace Northwind.Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IDbContext, NorthwindContext>(new HierarchicalLifetimeManager());
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            
            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}