using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Northwind.Api.Tests.Helpers
{
    public class TokenResponse
    {
        public string Content { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public TokenResponse(HttpStatusCode statusCode, string reasonPhrase)
        {
            this.StatusCode = statusCode;
            this.ReasonPhrase = reasonPhrase;
        }
    }
}
