namespace HouseAuction.Lobby.Requests
{
    public static class StartGame
    {
        public class Request
        {
            public string GameId { get; set; }

            public string Name { get; set; }
        }
    }
}
