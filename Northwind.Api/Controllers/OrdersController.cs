using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Northwind.Data;
using Northwind.Model;

namespace Northwind.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private readonly IRepository<Order> _repository;

        public OrdersController(IRepository<Order> repository)
        {
            _repository = repository;
        }


        
        [Route("")]
        public IHttpActionResult Get()
        {
            var orders = FetchOrders();
            return Ok(orders);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var result = FetchOrders(o => o.OrderID == id);
            if(result == null || !result.Any())
                return NotFound();

            return Ok(result);
        }

        private IEnumerable<Order> FetchOrders(Expression<Func<Order,bool>> predicate = null)
        {
            if (predicate == null) return _repository.Queryable();

            return _repository.Queryable().Where(predicate);
        }
    }
}