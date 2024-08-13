﻿namespace HouseAuction.Infrastructure.Identity
{
    public class UserContext
    {
        public List<Game> Games { get; set; }

        public string ConnectionId { get; set; }

        public class Game
        {
            public string GameId { get; set; }

            public string Player { get; set; }

            public string PlayerGroupName { get; set; }
        }

        public Game this[string gameId] => Games.Find(x => x.GameId == gameId);
    }
}
