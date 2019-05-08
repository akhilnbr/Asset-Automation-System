using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Asset_Automation.Startup))]
namespace Asset_Automation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
