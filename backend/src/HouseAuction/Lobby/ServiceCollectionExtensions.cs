namespace HouseAuction.Lobby
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLobby(this IServiceCollection services)
        {
            services.AddSingleton<Participants>();
        }
    }
}
