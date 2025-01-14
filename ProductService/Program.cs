using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Utility;

namespace ProductService
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
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
            var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=true";
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "ProductsRedisInstance";
            });
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

            //builder.Services.AddHttpClient("ProductDetails", u => u.BaseAddress = new Uri("http://ProductDetailsService")).AddServiceDiscovery();
            // builder.Services.AddDiscoveryClient(builder.Configuration);
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
            //lifetime.ApplicationStopping.Register(() =>
            //{
            //    // Deregister the instance on shutdown
            //    if (discoveryClient is EurekaDiscoveryClient eurekaClient)
            //    {
            //        eurekaClient.ShutdownAsync().Wait();
            //    }
            //});
            //var discoveryClient = app.Services.GetRequiredService<IDiscoveryClient>();
            //AppDomain.CurrentDomain.ProcessExit += async (sender, eventArgs) =>
            //{
            //    Console.WriteLine("Shutting down application...");
            //    if (discoveryClient != null)
            //    {
            //        await discoveryClient.ShutdownAsync();
            //        Console.WriteLine("Deregistered from Eureka.");
            //    }
            //};
            app.Run();
        }
    }
}
