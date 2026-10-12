using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CompanyVehicleBookingSystem.Startup))]
namespace CompanyVehicleBookingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
