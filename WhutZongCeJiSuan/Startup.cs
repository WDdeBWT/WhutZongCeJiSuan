using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WhutZongCeJiSuan.Startup))]
namespace WhutZongCeJiSuan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
