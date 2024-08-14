using HouseAuction.Messages.Lobby;
using Microsoft.EntityFrameworkCore;
using Onwrd.EntityFrameworkCore;

namespace HouseAuction.Lobby
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLobby(this IServiceCollection services)
        {
            services.AddTransient<ILobbyHub, LobbyHub>();
            services.AddDbContext<LobbyContext>(
                (serviceProvider, builder) =>
                {
                    // Your context configuration here...
                    builder.UseInMemoryDatabase("Lobby");
                },
                onwrdConfig =>
                {
                    // Outboxing configuration here
                    onwrdConfig.UseOnwardProcessors(x =>
                    {
                        x.Register<LobbyConfirmed, ForwardLobbyConfirmedMessage>();
                    });
                },
                contextLifetime: ServiceLifetime.Transient);
        }
    }
}
