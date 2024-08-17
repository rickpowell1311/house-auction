



namespace HouseAuction.Lobby.Domain
{
    public class Gamer : IEquatable<Gamer>
    {
        public Guid Id { get; private set; }

        public string GameId { get; private set; }

        public string Name { get; private set; }

        public bool IsDisconnected { get; private set; }

        public string ConnectionId { get; private set; }

        public bool IsReady { get; private set; }

        public Gamer(string name, string gameId, string connectionId)
        {
            Id = Guid.NewGuid();
            ConnectionId = connectionId;
            GameId = gameId;
            Name = name;
        }

        private Gamer(Guid id, string gameId, string name, string connectionId, bool isReady) : this(name, gameId, connectionId)
        {
            Id = id;
            IsReady = isReady;
        }

        public void ReadyUp()
        {
            IsReady = true;
        }

        public void Disconnect()
        {
            IsDisconnected = true;
        }

        public void Reconnect(string connectionId)
        {
            ConnectionId = connectionId;
            IsDisconnected = false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Gamer);
        }

        public bool Equals(Gamer other)
        {
            return other is not null &&
                Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public static bool operator ==(Gamer left, Gamer right)
        {
            return EqualityComparer<Gamer>.Default.Equals(left, right);
        }

        public static bool operator !=(Gamer left, Gamer right)
        {
            return !(left == right);
        }
    }
}
