namespace HouseAuction.Lobby
{
    public static class WebApplicationExtensions
    {
        public static void UseLobby(this WebApplication webApplication)
        {
            webApplication.MapHub<LobbyHub>("/lobby");
        }
    }
}
