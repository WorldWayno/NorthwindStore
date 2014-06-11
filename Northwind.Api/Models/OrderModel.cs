using System.Collections.Generic;

namespace Northwind.Api.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }

        public string CustomerName { get; set; }

        public string ShipperCity { get; set; }

        public bool IsShipped { get; set; }

        public static List<OrderModel> CreateOrders()
        {
            var orderList = new List<OrderModel> 
            {
                new OrderModel {OrderID = 10248, CustomerName = "Taiseer Joudeh", ShipperCity = "Amman", IsShipped = true },
                new OrderModel {OrderID = 10249, CustomerName = "Ahmad Hasan", ShipperCity = "Dubai", IsShipped = false},
                new OrderModel {OrderID = 10250,CustomerName = "Tamer Yaser", ShipperCity = "Jeddah", IsShipped = false },
                new OrderModel {OrderID = 10251,CustomerName = "Lina Majed", ShipperCity = "Abu Dhabi", IsShipped = false},
                new OrderModel {OrderID = 10252,CustomerName = "Yasmeen Rami", ShipperCity = "Kuwait", IsShipped = true}
            };

            return orderList;
        }
    }
}