using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Northwind.Api
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
               name: "SwaggerApi",
               routeTemplate: "api/docs/{controller}",
               defaults: new { swagger = true }
           );  

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );


        }
    }
}