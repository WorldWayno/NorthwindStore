using System.Data.Entity;
using Northwind.Data.Interceptors;

namespace Northwind.Data
{
    public class ContextConfig : DbConfiguration
    {
        public ContextConfig()
        {
            AddInterceptor(new DbCommandInterceptor());

            SetDatabaseInitializer<NorthwindContext>(null);
        }
    }
}