using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyPlace.Web.Startup))]
namespace MyPlace.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
