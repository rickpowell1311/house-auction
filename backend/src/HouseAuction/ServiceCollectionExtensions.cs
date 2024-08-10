using HouseAuction.Infrastructure.ExceptionHandling;
using HouseAuction.Infrastructure.HubContext;
using HouseAuction.Infrastructure.Messaging;
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
            services.AddMessaging();
            services.AddLobby();
        }
    }
}
