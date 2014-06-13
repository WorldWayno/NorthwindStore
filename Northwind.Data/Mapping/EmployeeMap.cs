using System.Data.Entity.ModelConfiguration;
using Northwind.Model;

namespace Northwind.Data.Mapping
{
    public class EmployeeMap : EntityTypeConfiguration<Employee>
    {
        public EmployeeMap()
        {
            this.HasKey(p => p.EmployeeID);

            this.Property(p => p.FirstName)
                            .IsRequired()
                            .HasMaxLength(50);

            this.Property(p => p.LastName)
                            .IsRequired()
                            .HasMaxLength(50);

            this.ToTable("Employees");
        }
    }
}