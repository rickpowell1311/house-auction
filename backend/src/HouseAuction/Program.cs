namespace HouseAuction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHouseAuction();

            var app = builder.Build();
            app.MapHouseAuctionRoutes();

            app.Run();
        }
    }
}