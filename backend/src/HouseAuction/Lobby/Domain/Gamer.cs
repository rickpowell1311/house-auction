
namespace HouseAuction.Lobby.Domain
{
    public class Gamer : IEquatable<Gamer>
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public bool IsReady { get; private set; }

        public Gamer(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        private Gamer(string name, bool isReady) : this(name)
        {
            IsReady = isReady;
        }

        public void ReadyUp()
        {
            IsReady = true;
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
