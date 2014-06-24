using System.ComponentModel.Design;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using Northwind.Api.Repository;

namespace Northwind.Api.Middleware.Token
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string id, secret;
            if (context.TryGetBasicCredentials(out id, out secret))
            {
                using (var repo = new AuthRepository())
                {
                    IdentityUser user = await repo.FindUser(id, secret);
                    if (user == null)
                    {
                        context.SetError("invalid_user");
                    }
                }
            }
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var repo = new AuthRepository())
            {
                IdentityUser user = await repo.FindUser(context.UserName, context.Password);

                if (user != null)
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    var principal = new ClaimsPrincipal(identity);
                    Thread.CurrentPrincipal = principal;
                    context.Validated();
                }
            }

            context.SetError("invalid_grant", "The user name or password is incorrect.");
            //context.Validated(identity);

        }
    }
}