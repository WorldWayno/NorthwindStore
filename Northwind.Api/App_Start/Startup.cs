using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Northwind.Api.Areas.HelpPage.App_Start;
using Northwind.Api.Middleware.Token;
using Northwind.Api.Repository;
using Northwind.Api.Security;
using Owin;

[assembly : OwinStartup(typeof(Northwind.Api.AppStart.Startup))]
namespace Northwind.Api.AppStart
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            // force token authentication
            //config.SuppressDefaultHostAuthentication();

            HelpPageConfig.Register(config);

            AreaRegistration.RegisterAllAreas();

       

            ConfigureOAuth(app);


            UnityConfig.RegisterComponents(config);

            WebApiConfig.Register(config);

            var hubConfiguration = new HubConfiguration { EnableDetailedErrors = true, EnableJavaScriptProxies = true };

            app.UseCors(CorsOptions.AllowAll);

            app.MapSignalR("/signalr", hubConfiguration);

            app.UseWebApi(config);

              
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            //app.UseCookieAuthentication(new CookieAuthenticationOptions()
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ExternalBearer,
            //    LoginPath = new PathString("/Account/Login"),
            //    CookieHttpOnly = true
            //});

         

            app.SetDefaultSignInAsAuthenticationType("Basic");
            app.UseBasicAuthentication(new BasicAuthenticationOptions("demo", ValidateBasicUser));
            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions()
            {
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1)

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
                    claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
                    claims.Add(new Claim(ClaimTypes.Name,user.UserName));

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