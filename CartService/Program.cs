
using CartService.Data;
using CartService.Utility;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

namespace CartService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDiscoveryClient(builder.Configuration);
            builder.Services.AddTransient<CustomExceptionMiddleware>();
            var app = builder.Build();
           
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseAuthorization();


            app.MapControllers();
            //var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            //var discoveryClient = app.Services.GetRequiredService<IDiscoveryClient>();

            //lifetime.ApplicationStopping.Register(() =>
            //{
            //    // Deregister the instance on shutdown
            //    if (discoveryClient is EurekaDiscoveryClient eurekaClient)
            //    {
            //        eurekaClient.ShutdownAsync().Wait();
            //    }
            //});

            var discoveryClient = app.Services.GetRequiredService<IDiscoveryClient>();
            AppDomain.CurrentDomain.ProcessExit += async (sender, eventArgs) =>
            {
                Console.WriteLine("Shutting down application...");
                if (discoveryClient != null)
                {
                    await discoveryClient.ShutdownAsync();
                    Console.WriteLine("Deregistered from Eureka.");
                }
            };
            app.Run();
        }
    }
}
