using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SwaggerWeb.Startup))]
namespace SwaggerWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
