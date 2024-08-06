using Tapper;

namespace HouseAuction.Lobby.Reactions
{
    public static class OnGameBegun
    {
        [TranspilationSource]
        public class OnGameBegunReaction
        {
            public string GameId { get; set; }
        }
    }
}
