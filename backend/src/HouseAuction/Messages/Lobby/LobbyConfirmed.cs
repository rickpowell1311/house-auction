using HouseAuction.Infrastructure.Messaging;

namespace HouseAuction.Messages.Lobby
{
    public class LobbyConfirmed : IMessage
    {
        public string GameId { get; set; }

        public List<Gamer> Gamers { get; set; }

        public class Gamer
        {
            public bool IsHost { get; set; }

            public string Name { get; set; }

            public string GroupName { get; set; }

            public string ConnectionId { get; set; }
        }
    }
}
