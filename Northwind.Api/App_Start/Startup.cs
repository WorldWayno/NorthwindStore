﻿using System.Web.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Northwind.Api.Middleware.Token;
using Northwind.Api.Repository;
using Northwind.Api.Security;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Northwind.Api.AppStart.Startup))]

namespace Northwind.Api.AppStart
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            // force token authentication
            //config.SuppressDefaultHostAuthentication();
           // config.Filters.Add(new HostAuthenticationFilter("Bearer"));

            ConfigureOAuth(app);

            UnityConfig.RegisterComponents(config);

            app.UseCors(CorsOptions.AllowAll);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            WebApiConfig.Register(config);

          
            var hubConfiguration = new HubConfiguration { EnableDetailedErrors = true, EnableJavaScriptProxies = true };

            app.MapSignalR("/signalr", hubConfiguration);

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                Provider = new SimpleAuthorizationServerProvider(),
                AuthenticationMode = AuthenticationMode.Passive,
                AllowInsecureHttp = true
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            //app.SetDefaultSignInAsAuthenticationType("Basic");
            app.UseBasicAuthentication(new BasicAuthenticationOptions("demo", ValidateBasicUser));

            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions()
            {
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                TokenEndpointPath = new PathString("/bearer"),
                Provider = new OAuthAuthorizationServerProvider(),
                AllowInsecureHttp = true
            });

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