using Tapper;

namespace HouseAuction.Lobby.Reactions
{
    public static class OnLobbyMembersChanged
    {
        [TranspilationSource]
        public class OnLobbyMembersChangedReaction
        {
            public List<OnLobbyMembersChangedReactionGamer> Gamers { get; set; }
        }

        [TranspilationSource]
        public class OnLobbyMembersChangedReactionGamer
        {
            public string Name { get; set; }

            public bool IsMe { get; set; }

            public bool IsCreator { get; set; }

            public bool IsReady { get; set; }
        }
    }
}
