﻿namespace HouseAuction.Lobby.Domain
{
    public class Lobby
    {
        public const int MinGamers = 3;

        public const int MaxGamers = 6;

        public bool IsReadyToStartGame => Gamers.Count >= MinGamers 
            && Gamers.Count <= MaxGamers
            && Gamers.All(x => x.IsReady);

        public bool HasGameStarted { get; private set; }

        public string GameId { get; private set; }

        public Gamer Creator { get; private set; }

        public ICollection<Gamer> Gamers { get; private set; }

        private Lobby(string gameId, string creator, string creatorConnectionId) : this(gameId, false)
        {          
            Creator = new Gamer(creator, GameId, creatorConnectionId);
            Gamers = new List<Gamer> { Creator };
        }

        private Lobby(string gameId, bool hasGameStarted)
        {
            GameId = gameId;
            HasGameStarted = hasGameStarted;
        }

        public static Lobby Create(string creator, string creatorConnectionId)
        {
            return new Lobby(
                Domain.GameId.NewGameId(),
                creator,
                creatorConnectionId);
        }

        public LobbyJoinResult Join(string name, string connectionId)
        {
            if (HasGameStarted && Gamers.Any(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant()))
            {
                return LobbyJoinResult.Reconnection();
            }

            if (HasGameStarted)
            {
                return LobbyJoinResult.Error("Game in progress");
            }

            if (Gamers.Count >= MaxGamers)
            {
                return LobbyJoinResult.Error("This lobby is full");
            }

            if (Gamers.Any(x => FuzzyNameMatch.IsCloseMatch(x.Name, name)))
            {
                return LobbyJoinResult.Error(
                    "Someone with a similar user name already joined this lobby. Please choose another name");
            }

            var gamer = new Gamer(name, GameId, connectionId);
            Gamers.Add(gamer);

            return LobbyJoinResult.Success();
        }

        public bool TryReadyUp(string connectionId, out string error)
        {
            error = null;
            var gamer = Gamers
                .SingleOrDefault(x => x.ConnectionId == connectionId);

            if (gamer == null)
            {
                error = $"Cannot ready up - not part of lobby for game {GameId}";
                return false;
            }

            gamer.ReadyUp();

            return true;
        }

        public bool TryBeginGame(string connectionId, out string error)
        {
            error = null;

            if (HasGameStarted)
            {
                error = $"Game {GameId} already started";
                return false;
            }

            if (!IsReadyToStartGame)
            {
                error = $"Game {GameId} is not ready to start";
                return false;
            }

            if (connectionId != Creator.ConnectionId)
            {
                error = $"Cannot ready up - not the creator of game {GameId}";
                return false;
            }

            HasGameStarted = true;
            return true;
        }

        public void Disconnect(string connectionId)
        {
            if (!HasGameStarted)
            {
                var disconnected = new List<Gamer>(
                    Gamers.Where(x => x.ConnectionId == connectionId));

                foreach (var gamer in disconnected)
                {
                    Gamers.Remove(gamer);
                }
            }
        }
    }
}
