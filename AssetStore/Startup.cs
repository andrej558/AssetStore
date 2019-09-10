using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AssetStore.Startup))]
namespace AssetStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
