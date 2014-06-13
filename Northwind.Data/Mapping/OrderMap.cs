using System.Data.Entity.ModelConfiguration;
using Northwind.Model;

namespace Northwind.Data.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            this.HasKey(p => p.OrderID);

            this.HasOptional(p => p.Employee)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.EmployeeID);


            this.HasOptional(t => t.Customer)
                 .WithMany(t => t.Orders)
                 .HasForeignKey(d => d.CustomerID);
                 

            this.ToTable("Orders");
        }
    }
}