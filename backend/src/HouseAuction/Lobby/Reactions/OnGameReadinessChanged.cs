using Tapper;

namespace HouseAuction.Lobby.Reactions
{
    public static class OnGameReadinessChanged
    {
        [TranspilationSource]
        public class OnGameReadinessChangedReaction
        {
            public bool IsReadyToStart { get; set; }
        }
    }
}
