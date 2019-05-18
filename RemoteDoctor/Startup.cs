using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RemoteDoctor.Startup))]
namespace RemoteDoctor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
