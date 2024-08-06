using Google.Cloud.Diagnostics.AspNetCore3;
using Microsoft.AspNetCore.Routing;

namespace HouseAuction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            if (builder.Environment.IsProduction())
            {
                builder.Services.AddGoogleDiagnosticsForAspNetCore();
            }

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
            app.MapHub<HouseAuctionHub>(HouseAuctionHub.Route); ;
            app.UseRouting();
            app.UseCors();

            // Log setup before starting
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation($"Environment: {builder.Environment.EnvironmentName}");
            logger.LogInformation($"Allowed CORS Origins: {allowedOrigins.Aggregate((prev, curr) => $"{prev}, {curr}")}");

            app.Run();
        }
    }
}