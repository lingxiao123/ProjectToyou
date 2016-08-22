using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectToYou.Startup))]
namespace ProjectToYou
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
