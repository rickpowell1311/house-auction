using HouseAuction.Lobby;

namespace HouseAuction
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHouseAuction(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddLobby();
        }
    }
}
