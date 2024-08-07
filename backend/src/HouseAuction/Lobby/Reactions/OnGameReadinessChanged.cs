using Tapper;

namespace HouseAuction.Lobby.Reactions
{
    public static class OnGameReadinessChanged
    {
        [TranspilationSource]
        public class OnGameReadinessChangedReaction
        {
            public string GameId { get; set; }

            public bool IsReadyToStart { get; set; }
        }
    }
}
