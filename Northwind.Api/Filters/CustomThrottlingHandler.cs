using System.Linq;
using System.Net.Http;
using WebApiThrottle;

namespace Northwind.Api.Filters
{
    public class CustomThrottlingHandler : ThrottlingHandler
    {
        protected override RequestIdentity SetIndentity(HttpRequestMessage request)
        {
            return new RequestIdentity()
            {
                //ClientKey = request.Headers.GetValues("Authorization-Key").First(),
                ClientIp = base.GetClientIp(request).ToString(),
                Endpoint = request.RequestUri.AbsolutePath
            };
        }
    }
}