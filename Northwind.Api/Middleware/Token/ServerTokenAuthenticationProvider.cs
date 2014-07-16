using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace Northwind.Api.Middleware.Token
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerTokenAuthenticationProvider : OAuthAuthorizationServerProvider
    {
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            return base.GrantClientCredentials(context);
        }

        public override Task ValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context)
        {
            return base.ValidateAuthorizeRequest(context);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return base.GrantResourceOwnerCredentials(context);
        }
    }
}