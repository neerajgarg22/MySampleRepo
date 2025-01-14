using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;
using Steeltoe.Discovery.Client;

namespace Ocelot.APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddOcelot().AddEureka();
            builder.Services.AddDiscoveryClient(builder.Configuration);
            var app = builder.Build();
            app.UseOcelot();
            app.Run();
        }
    }
}
