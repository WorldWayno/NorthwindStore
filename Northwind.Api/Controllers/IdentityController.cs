using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Northwind.Api.Controllers
{
    [Authorize]
    public class IdentityController : ApiController
    {
        public IHttpActionResult Get()
        {
            var principal = User as ClaimsPrincipal;
            if (principal == null) return BadRequest("principal not found");

            return Ok(principal.Claims); 
        }
    }
}