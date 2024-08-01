namespace HouseAuction.Lobby.Domain
{
    public class LobbyJoinResult
    {
        public enum JoinResultType { Success, Error, Reconnection }

        public JoinResultType Type { get; }

        public string ErrorMessage { get; }

        private LobbyJoinResult(JoinResultType type, string errorMessage = null)
        {
            Type = type;
            ErrorMessage = errorMessage;
        }
        public static LobbyJoinResult Success()
        {
            return new LobbyJoinResult(JoinResultType.Success);
        }

        public static LobbyJoinResult Error(string message)
        {
            return new LobbyJoinResult(JoinResultType.Error, message);
        }

        public static LobbyJoinResult Reconnection()
        {
            return new LobbyJoinResult(JoinResultType.Reconnection);
        }
    }
}
