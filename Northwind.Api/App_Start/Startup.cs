using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly : OwinStartup(typeof(Northwind.Api.AppStart.Startup))]
namespace Northwind.Api.AppStart
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}