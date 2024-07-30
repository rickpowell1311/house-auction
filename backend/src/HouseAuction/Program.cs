using HouseAuction.Lobby;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSignalR();
        builder.Services.AddLobby();

        var app = builder.Build();
        app.UseLobby();

        app.Run();
    }
}