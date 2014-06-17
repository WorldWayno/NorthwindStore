using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Northwind.Api.Models;
using Northwind.Data;
using Northwind.Model;

namespace Northwind.Api.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private readonly IRepository<Order> _repository;

        public OrdersController(IRepository<Order> repository)
        {
            _repository = repository;
        }


        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            var orders = FetchOrders();
            return Ok(orders);
        }

        private IEnumerable<Order> FetchOrders()
        {
            return _repository.Queryable().ToList();
        }
    }
}