using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Northwind.Api.Models;
using Northwind.Data;
using Northwind.Model;

namespace Northwind.Api.Controllers
{
    /// <summary>
    /// </summary>
    //[Authorize]
    [System.Web.Http.RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private readonly IRepository<Order> _repository;

        /// <summary>
        /// </summary>
        /// <param name="repository"></param>
        public OrdersController(IRepository<Order> repository)
        {
            _repository = repository;
        }


        /// <summary>
        ///     Gets all orders
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public IHttpActionResult Get()
        {
            IEnumerable<Order> orders = FetchOrders();
            return Ok(orders);
        }

        /// <summary>
        ///     Gets an order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Order result = FetchOrders(o => o.OrderID == id).SingleOrDefault();
            if (result == null) return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] OrderModel order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var target = new Order() {
                CustomerID = order.CustomerID, 
                ShipCity = order.ShipCity
            };

            _repository.Add(target);
            var saved = await _repository.SaveChangesAsync();

            return Created(Request.RequestUri, target);
        }

        private IEnumerable<Order> FetchOrders(Expression<Func<Order, bool>> predicate = null)
        {
            if (predicate == null) return _repository.Queryable();

            return _repository.Queryable().Where(predicate);
        }
    }
}