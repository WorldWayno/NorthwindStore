using System;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using Northwind.Api;
using Swashbuckle.Application;
using WebActivatorEx;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Northwind.Api
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Swashbuckle.Bootstrapper.Init(config);

            //     config.Routes.MapHttpRoute(
            //    "swagger_versioned_api_docs",
            //    "swagger/{apiVersion}/api-docs/{resourceName}",
            //    new { resourceName = RouteParameter.Optional },
            //    null,
            //    new SwaggerSpecHandler());

            //config.Routes.MapHttpRoute(
            //    "swagger_api_docs",
            //    "swagger/api-docs/{resourceName}",
            //    new { resourceName = RouteParameter.Optional },
            //    null,
            //    new SwaggerSpecHandler());

            SwaggerSpecConfig.Customize(c =>
            {
                c.IgnoreObsoleteActions();
                c.GroupDeclarationsBy(ResolveResourceName);
                c.ResolveBasePathUsing(ResolveBasePath);
                c.IncludeXmlComments(HostingEnvironment.MapPath("~/App_Data/Northwind.Api.xml"));
            });

            SwaggerUiConfig.Customize(c =>
            {
                c.SupportHeaderParams = true;
                c.DocExpansion = DocExpansion.List;
                //var ass  = Assembly.GetExecutingAssembly();
                //string[] names = ass.GetManifestResourceNames();
                c.CustomRoute("index.html", Assembly.GetExecutingAssembly(), "Northwind.Api.Swaggerui.index.html");
            });
        }

        private static string ResolveBasePath(HttpRequestMessage httpRequestMessage)
        {
            var req = httpRequestMessage;
            var path = HostingEnvironment.ApplicationVirtualPath.TrimStart('/');
            if (string.IsNullOrWhiteSpace(path))
            {
                path = req.RequestUri.GetLeftPart(UriPartial.Authority) +
                       req.GetConfiguration().VirtualPathRoot.TrimEnd('/');
            }

           // path = "http://northwindapi";
            return path;
        }

        private static string ResolveResourceName(ApiDescription apiDescription)
        {
            return apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName;
        }

        private static string GetBasePath()
        { 
            var virtPath = HostingEnvironment.ApplicationVirtualPath.TrimEnd('/');
            return virtPath;
        }
    }
}