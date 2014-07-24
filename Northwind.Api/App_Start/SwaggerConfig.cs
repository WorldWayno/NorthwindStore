using System;
using System.Net.Http;
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

            SwaggerSpecConfig.Customize(c =>
            {
                c.IgnoreObsoleteActions();
                //c.GroupDeclarationsBy(ResolveResourceName);
                c.ResolveBasePathUsing(ResolveBasePath);
                c.IncludeXmlComments(HostingEnvironment.MapPath("~/App_Data/Northwind.Api.xml"));
            });
            // NOTE: If you want to customize the generated swagger or UI, use SwaggerSpecConfig and/or SwaggerUiConfig here ...
        }

        private static string ResolveBasePath(HttpRequestMessage httpRequestMessage)
        {
            var req = httpRequestMessage;
            var path = HostingEnvironment.ApplicationVirtualPath;
            if (path.Equals("/"))
            {
              path = req.RequestUri.GetLeftPart(UriPartial.Authority) + req.GetConfiguration().VirtualPathRoot.TrimEnd('/'); 
            }
            return path;
        }

        private static string ResolveResourceName(ApiDescription apiDescription)
        {
            return apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName;
        }
    }
}