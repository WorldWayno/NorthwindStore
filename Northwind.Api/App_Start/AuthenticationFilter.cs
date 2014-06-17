using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity.EntityFramework;
using Northwind.Api.Repository;
using WebApi.AuthenticationFilter;

namespace Northwind.Api
{
    public class AuthenticationFilter : AuthenticationFilterAttribute
    {
        public async override Task OnAuthenticationAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var authHeader = context.Request.Headers.Authorization;
            if (authHeader == null || !authHeader.Scheme.StartsWith("Basic"))
            {
                context.ErrorResult = Unauthorized(context.Request);
            }
            else
            {
                string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                if (credentials.Length == 2)
                {
                    using (var repo = new AuthRepository())
                    {
                        IdentityUser user = await repo.FindUser(credentials[0], credentials[1]);
                        if (user != null)
                        {
                            var identity = await repo.CreateIdentityAsync(user, "BasicAuth");
                            context.Principal = new ClaimsPrincipal(new ClaimsIdentity[] {identity});
                        }
                        else
                        {
                            context.ErrorResult = Unauthorized(context.Request);
                        }
                    }
                }
                else
                {
                    context.ErrorResult = Unauthorized(context.Request);
                }
            }
        }

        private StatusCodeResult Unauthorized(HttpRequestMessage request)
        {
            return new StatusCodeResult(HttpStatusCode.Unauthorized, request);
        }
    }
}