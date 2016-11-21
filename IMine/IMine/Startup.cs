using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IMine.Startup))]
namespace IMine
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
