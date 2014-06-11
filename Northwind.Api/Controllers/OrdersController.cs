using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Northwind.Api.Models;

namespace Northwind.Api.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(OrderModel.CreateOrders());
        }
    }
}