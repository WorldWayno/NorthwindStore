using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Northwind.Web.Startup))]
namespace Northwind.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
