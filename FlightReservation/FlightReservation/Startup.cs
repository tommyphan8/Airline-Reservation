using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlightReservation.Startup))]
namespace FlightReservation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
