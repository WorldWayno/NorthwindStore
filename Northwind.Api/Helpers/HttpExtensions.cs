using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;

namespace Northwind.Api.Helpers
{
    public static class HttpExtensions
    {
        public static IPAddress GetClientIp(this HttpRequestMessage request)
        {
            IPAddress ipAddress;

            if (request.Properties.ContainsKey("MS_OwinContext"))
            {
                var ok = IPAddress.TryParse(((Microsoft.Owin.OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress, out ipAddress);

                if (ok)
                {
                    return ipAddress;
                }
            }

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                var ok = IPAddress.TryParse(((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress, out ipAddress);

                if (ok)
                {
                    return ipAddress;
                }
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                var ok = IPAddress.TryParse(((RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name]).Address, out ipAddress);

                if (ok)
                {
                    return ipAddress;
                }
            }

         


            return null;
        }
    }
}