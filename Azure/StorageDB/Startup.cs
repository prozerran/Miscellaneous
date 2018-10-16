using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StorageDB.Startup))]
namespace StorageDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
