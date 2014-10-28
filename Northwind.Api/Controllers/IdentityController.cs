using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Northwind.Api.Helpers;

namespace Northwind.Api.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class IdentityController : ApiController
    {
        public IHttpActionResult Get()
        {
            var principal = User as ClaimsPrincipal;
            if (principal == null) return BadRequest("principal not found");

            return Ok(Request.GetClientIp().ToString()); 
        }
    }
}