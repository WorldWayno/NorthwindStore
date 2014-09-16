using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Northwind.Api.Models
{
    public interface IModel
    {
        int Id { get; set; }

        string Name { get; set; }
    }

    public class OrderModel : IModel

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderID { get; set; }

        [Required]
        [MaxLength(5)]
        public string CustomerID { get; set; }

        public string ShipCity { get; set; }

        public bool IsShipped { get; set; }
    }
}