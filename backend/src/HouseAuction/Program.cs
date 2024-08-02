using System.Reflection.PortableExecutable;

namespace HouseAuction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Order important - environment variables should override appsettings.json
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{builder.Environment}.json", optional: true);
            builder.Configuration.AddEnvironmentVariables();

            var allowedOrigins = builder.Configuration
                .GetSection("Cors")
                .Get<CorsOptions>()
                .AllowedOrigins;

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(x =>
                {
                    x.WithOrigins([.. allowedOrigins]);
                    x.AllowAnyMethod();
                    x.AllowAnyHeader();
                    x.AllowCredentials();
                });
            });
            builder.Services.AddHouseAuction();

            var app = builder.Build();
            app.MapHouseAuctionRoutes();
            app.UseRouting();
            app.UseCors();

            app.Run();
        }
    }
}