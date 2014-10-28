using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Northwind.Api.Handlers
{
    // code ref: http://weblogs.asp.net/fredriknormen/log-message-request-and-response-in-asp-net-webapi
    public abstract class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var id = string.Format("{0}-{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);

            var watcther = Stopwatch.StartNew();

            var response = await base.SendAsync(request, cancellationToken);

            watcther.Stop();


            LogResponseAsync(id, request, response, watcther.ElapsedMilliseconds);

            return response;
        }

        protected abstract Task LogRequestAsync(string id, string method, string requestUri, byte[] message);
        protected abstract Task LogResponseAsync(string id, HttpRequestMessage request, HttpResponseMessage response, long responseTime);
    }
}