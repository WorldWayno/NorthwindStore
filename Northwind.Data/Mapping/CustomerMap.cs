using System.Data.Entity.ModelConfiguration;
using Northwind.Model;

namespace Northwind.Data.Mapping
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.HasKey(p => p.CustomerID);

            this.Property(p => p.CustomerID)
                .HasMaxLength(5);


            this.ToTable("Customers");
        }
    }
}