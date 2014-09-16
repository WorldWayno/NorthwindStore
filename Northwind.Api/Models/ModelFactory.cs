using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Northwind.Model;

namespace Northwind.Api.Models
{
    public class ModelFactory
    {
        public static OrderModel Create(Order order)
        {
            return new OrderModel
            {
                CustomerID = order.CustomerID,
                IsShipped = order.ShippedDate.HasValue,
                OrderID = order.OrderID,
                ShipCity = order.ShipCity
            };
        }

        public static Order Parse(OrderModel model)
        {
            return new Order
            {
                CustomerID = model.CustomerID,
                OrderID = model.OrderID,
                ShipCity = model.ShipCity
            };
        }
    }
}