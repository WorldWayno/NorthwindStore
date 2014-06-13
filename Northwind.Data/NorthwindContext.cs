using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Data.Mapping;
using Northwind.Model;

namespace Northwind.Data
{
    public class NorthwindContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Customer> Customers { get; set; }

 
        static NorthwindContext()
        {
            Database.SetInitializer<NorthwindContext>(null);
        }

        public NorthwindContext() : base("NorthwindConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EmployeeMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
