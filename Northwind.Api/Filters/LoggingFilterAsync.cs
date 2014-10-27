using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Northwind.Data;
using Northwind.Model;

namespace Northwind.Api.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncLoggingFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var controller = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var action = actionContext.ActionDescriptor.ActionName;
            var username = HttpContext.Current.User.Identity.Name ?? "anonymous";
            var ip = HttpContext.Current.Request.UserHostAddress;

            await Write(controller, action, username, ip);
        }

        private Task Write(string controller, string action, string username, string ip)
        {
            return Task.Run(() => "");
        }
    }
}