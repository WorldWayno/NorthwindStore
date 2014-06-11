using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Northwind.Data
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
    }
}
