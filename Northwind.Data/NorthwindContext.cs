﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Data.Interceptors;
using Northwind.Data.Mapping;
using Northwind.Model;

namespace Northwind.Data
{
    public class NorthwindContext : DbContext, IDbContext
    {
        static NorthwindContext()
        {
            DbInterception.Add(new LoggingCommandInterceptor());

            Database.SetInitializer<NorthwindContext>(null);
        }
        public NorthwindContext() : base("NorthwindConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
           
           // this.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new CustomerMap());
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public System.Data.Entity.DbSet<Northwind.Model.Employee> Employees { get; set; }
    }
}
