using HouseAuction.Infrastructure.Messaging;

namespace HouseAuction.Messages.Lobby
{
    public class LobbyConfirmed : IMessage
    {
        public string GameId { get; set; }

        public List<Gamer> Gamers { get; set; }

        public class Gamer
        {
            public string Name { get; set; }

            public string GroupName { get; set; }
        }
    }
}
