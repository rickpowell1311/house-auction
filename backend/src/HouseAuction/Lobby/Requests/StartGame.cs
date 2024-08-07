using Tapper;

namespace HouseAuction.Lobby.Requests
{
    public static class StartGame
    {
        [TranspilationSource]
        public class StartGameRequest
        {
            public string GameId { get; set; }

            public string Name { get; set; }
        }
    }
}
