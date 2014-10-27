using System.IO;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using System.Web.Http.Tracing;
using System.Web.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Northwind.Api.App_Start;
using Northwind.Api.Filters;
using Northwind.Api.Middleware.Token;
using Northwind.Api.Repository;
using Northwind.Api.Security;
using Northwind.Api.Services;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using SDammann.WebApi.Versioning;

[assembly: OwinStartup(typeof(Northwind.Api.AppStart.Startup))]

namespace Northwind.Api.AppStart
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }
        public void Configuration(IAppBuilder app)
        {
           HttpConfiguration = new HttpConfiguration();
            // force token authentication
           HttpConfiguration.SuppressDefaultHostAuthentication();
           HttpConfiguration.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            ConfigureOAuth(app);

            UnityConfig.RegisterComponents(HttpConfiguration);

            app.UseCors(CorsOptions.AllowAll);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var hubConfiguration = new HubConfiguration { EnableDetailedErrors = true, EnableJavaScriptProxies = true };

            app.MapSignalR("/signalr", hubConfiguration);

            //HttpConfiguration.Services.Replace(typeof (IHttpControllerSelector), 
            //    new VersionControllerSelector(HttpConfiguration));

            HttpConfiguration.Filters.Add(new AsyncLoggingFilter());

            SwaggerConfig.Register(HttpConfiguration);

            //trace provider
            //var traceWriter = new SystemDiagnosticsTraceWriter()
            //{
            //    IsVerbose = true
            //};

            //HttpConfiguration.Services.Replace(typeof(ITraceWriter), traceWriter);
            //HttpConfiguration.EnableSystemDiagnosticsTracing();

            //ThrottleConfig.Register(HttpConfiguration);

            WebApiConfig.Register(HttpConfiguration);

            app.UseWebApi(HttpConfiguration);

            //app.UseStageMarker(PipelineStage.MapHandler);

           
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //app.UseBasicAuthentication(new BasicAuthenticationOptions("demo", ValidateBasicUser));

            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(),
                ApplicationCanDisplayErrors = true
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
 
        }

        private async Task<IEnumerable<Claim>> ValidateBasicUser(string username, string password)
        {
            var claims = new List<Claim>();
            using (var repo = new AuthRepository())
            {
                IdentityUser user = await repo.FindUser(username, password);
                if (user != null)
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                    var role = "user";
                    if (user.Roles.Any())
                    {
                        role = user.Roles.First().RoleId;
                    }

                    claims.Add(new Claim(ClaimTypes.Role, role));
                    return claims;
                }
            }

            return null;
        }
    }
}