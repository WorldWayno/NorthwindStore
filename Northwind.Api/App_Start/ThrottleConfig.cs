using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Tracing;
using Northwind.Api.Filters;
using WebApiThrottle;

namespace Northwind.Api.App_Start
{
    public static class ThrottleConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new CustomThrottlingHandler 
            {
                Policy = new ThrottlePolicy(perSecond: 2, perMinute: 60)
                {
                    IpThrottling = true,
                    ClientThrottling = true,
                    EndpointThrottling = true,
                },
                Logger = new TracingThrottleLogger(new SystemDiagnosticsTraceWriter()),
                Repository = new MemoryCacheRepository()
            });
        }
    }
}