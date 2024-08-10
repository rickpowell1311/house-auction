using HouseAuction.ExceptionHandling;
using HouseAuction.HubContext;
using HouseAuction.Lobby;
using Microsoft.AspNetCore.SignalR;

namespace HouseAuction
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHouseAuction(this IServiceCollection services)
        {
            services.AddScoped<CallingHubContext>();
            services.AddSignalR(cfg =>
            {
                cfg.AddFilter<HubExceptionNotifierFilter>();
                cfg.AddFilter<CallingHubContextFilter>();
            });
            services.AddLobby();
        }
    }
}
