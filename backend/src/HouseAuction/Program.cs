namespace HouseAuction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(x =>
                {
                    x.WithOrigins("http://localhost:5173");
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