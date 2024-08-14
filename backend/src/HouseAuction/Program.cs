using Google.Cloud.Diagnostics.AspNetCore3;
using HouseAuction.Bidding;
using HouseAuction.Bidding.Domain;
using HouseAuction.Lobby;
using HouseAuction.Lobby.Domain;

namespace HouseAuction
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
            builder.Configuration.AddEnvironmentVariables();

            var allowedOrigins = builder.Configuration
                .GetSection("Cors")
                .Get<CorsOptions>()
                .AllowedOrigins;

            var testMode = builder.Configuration
                .GetSection("TestMode")
                .Get<TestMode>();

            builder.Services.Configure<TestMode>(
                builder.Configuration.GetSection("TestMode"));

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

            if (testMode.Enabled)
            {
                // Seed some data for a game
                using var scope = app.Services.CreateScope();
                using var biddingContext = scope.ServiceProvider.GetService<BiddingContext>();
                var players = new List<string> { "Alice", "Bob", "Charlie", "David" };
                var biddingPhase = new BiddingPhase("123", players);

                biddingContext.Add(biddingPhase);

                await biddingContext.SaveChangesAsync();
            }

            app.Run();
        }
    }
}