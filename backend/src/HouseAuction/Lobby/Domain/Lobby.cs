namespace HouseAuction.Lobby.Domain
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

        public bool TryReadyUp(string name, out string error)
        {
            error = null;
            var gamer = Gamers.SingleOrDefault(x => x.Name == name);

            if (gamer == null)
            {
                error = $"Cannot ready up - Gamer {name} is not part of lobby for game {GameId}";
                return false;
            }

            gamer.ReadyUp();

            if (IsReadyToStartGame)
            {
                HasGameStarted = true;
            }

            return true;
        }
    }
}
