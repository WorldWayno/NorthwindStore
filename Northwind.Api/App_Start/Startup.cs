using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
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
            ConfigureOAuth(app);

            var config = new HttpConfiguration();

            UnityConfig.RegisterComponents(config);
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
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseBasicAuthentication("Demo", ValidateUsers);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

        private async Task<IEnumerable<Claim>> ValidateUsers(string username, string password)
        {
            var claims = new List<Claim>();
            using (var repo = new AuthRepository())
            {
                IdentityUser user = await repo.FindUser(username, password);
                if (user != null)
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier,user.UserName));
                    claims.Add(new Claim(ClaimTypes.Role,"user"));
                    return claims;
                }
            }

            return null;
        }
    }
}