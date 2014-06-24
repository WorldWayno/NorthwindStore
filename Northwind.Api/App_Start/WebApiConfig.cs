using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Northwind.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.Filters.Add(new AuthenticationFilter());

            // Cors
            config.EnableCors();

            // attribute routing
            config.MapHttpAttributeRoutes();

            // OData support
            config.EnableQuerySupport();

            // Routes
            config.Routes.MapHttpRoute(
                name: "VersionedtApi",
                routeTemplate: "api/v{version}/{controller}/{id}",
                defaults: new {version = 1, id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional}
                );

            // Formatters
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
