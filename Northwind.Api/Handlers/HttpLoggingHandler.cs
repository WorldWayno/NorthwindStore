using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Microsoft.Owin.Logging;
using Northwind.Api.Handlers;
using Northwind.Api.Helpers;
using Northwind.Logging;
using WebApiThrottle;

public class HttpLoggingHandler : MessageHandler
{
    public IHttpLogger Logger { get; set; }

    protected override Task LogRequestAsync(string id, string method, string requestUri, byte[] message)
    {
        return Task.Run(() => Debug.Write(String.Format("message: {0}", Encoding.UTF8.GetString(message))));
    }

    protected override async Task LogResponseAsync(string id, HttpRequestMessage request, HttpResponseMessage response,
        long responseTime)
    {
        string message = null;
        if (!response.IsSuccessStatusCode)
            message = await response.Content.ReadAsStringAsync();

        string resource = null;
        HttpRequestContext requestContext = request.GetRequestContext();
        IHttpRouteData routeData = request.GetRouteData();

        if (routeData != null)
        {
            IDictionary<string,object> dataTokens = routeData.Route.DataTokens;

            object possibleDirectRouteActions;
            if (dataTokens.TryGetValue("actions", out possibleDirectRouteActions))
            {
                if (possibleDirectRouteActions != null)
                {
                    var directRouteActions = possibleDirectRouteActions as HttpActionDescriptor[];
                    if (directRouteActions != null && directRouteActions.Length > 0)
                    {
                        HttpActionDescriptor route = directRouteActions[0];
                        if (route != null)
                        {
                            resource = route.ControllerDescriptor.ControllerName;
                        }
                    }
                }
            }
        }

        var username = Thread.CurrentPrincipal.Identity.IsAuthenticated
            ? Thread.CurrentPrincipal.Identity.Name
            : "anonymous";

        var remoteAddress = ApiHelper.GetClientIP();

        await WriteToLoggerAsync(new HttpLog
        {
            Id = id,
            RequestDate = DateTime.UtcNow,
            RequestMethod = request.Method.ToString(),
            RequestUrl = request.RequestUri.ToString(),
            RemoteAddress = remoteAddress,
            HttpStatusCode = (int) response.StatusCode,
            UserName = username,
            ResponseTime = responseTime,
            ThreadId = Thread.CurrentThread.ManagedThreadId,
            Message = message,
            Resource = resource
        });
    }

    private async Task WriteToLoggerAsync(IHttpLog log)
    {
        using (Logger = new HttpSqlLogger())
        {
            await Logger.WriteHttpResponseAsync(log);
        }
    }
}