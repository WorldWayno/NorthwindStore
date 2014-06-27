using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Api.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }

        [Required]
        [MaxLength(5)]
        public string CustomerID { get; set; }

        public string ShipCity { get; set; }

        public bool IsShipped { get; set; }
    }
}