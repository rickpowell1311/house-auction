namespace HouseAuction.Lobby.Domain
{
    public class Lobby
    {
        public const int MinGamers = 3;

        public const int MaxGamers = 6;

        public bool HasStarted { get; private set; }

        public string GameId { get; private set; }

        public Gamer Creator { get; private set; }

        public ICollection<Gamer> Gamers { get; private set; }

        private Lobby(string gameId, string creator) : this(gameId, false)
        {          
            Creator = new Gamer(creator, GameId);
            Gamers = new List<Gamer> { Creator };
        }

        private Lobby(string gameId, bool hasStarted)
        {
            GameId = gameId;
            HasStarted = hasStarted;
        }

        public static Lobby Create(string creator)
        {
            return new Lobby(
                Domain.GameId.NewGameId(),
                creator);
        }

        public LobbyJoinResult Join(string name)
        {
            if (HasStarted && Gamers.Any(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant()))
            {
                return LobbyJoinResult.Reconnection();
            }

            if (HasStarted)
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

            var gamer = new Gamer(name, GameId);
            Gamers.Add(gamer);

            return LobbyJoinResult.Success();
        }

        public void ReadyUp(string name)
        {
            var gamer = Gamers.SingleOrDefault(x => x.Name == name);

            if (gamer == null)
            {
                return;
            }

            gamer.ReadyUp();
        }

        public bool TryBeginGame(out string error)
        {
            error = null;

            if (Gamers.Count < MinGamers)
            {
                error = "Not enough players";
                return false;
            }

            if (Gamers.Any(x => !x.IsReady))
            {
                error = "Not all gamers are ready";
                return false;
            }

            HasStarted = true;
            return true;
        }
    }
}
