using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace Northwind.Api.Security
{

    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public BasicAuthenticationMiddleware.CredentialValidationFunction CredentialValidationFunction { get;
            private set; }

        public string Realm { get; private set; }

        public BasicAuthenticationOptions(string realm,
            BasicAuthenticationMiddleware.CredentialValidationFunction validationFunction)
            : base("Basic")
        {
            Realm = realm;
            CredentialValidationFunction = validationFunction;
        }
    }
}