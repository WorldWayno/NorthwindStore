using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SDammann.WebApi.Versioning;

namespace Northwind.Api.Services
{
    public class VersionControllerSelector : VersionedControllerSelector
    {
        public VersionControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            VersionPrefix = "V";
        }

        protected override ControllerIdentification GetControllerIdentificationFromRequest(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            var controller = base.GetControllerNameFromRequest(request);
            var controllers = base.GetControllerMapping();
            return new ControllerIdentification(controller,"1");
        }
    }
}