using HouseAuction.Lobby;
using Microsoft.AspNetCore.SignalR;

namespace HouseAuction
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHouseAuction(this IServiceCollection services)
        {
            services.AddSignalR(cfg =>
            {
                cfg.AddFilter<HubExceptionNotifierFilter>();
            });
            services.AddLobby();
        }
    }
}
