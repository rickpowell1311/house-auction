using HouseAuction.Messages.Lobby;

namespace HouseAuction.Bidding
{
    public class OnLobbyConfirmed : IMessageSubscriber<LobbyConfirmed>
    {
        public Task Handle(LobbyConfirmed message)
        {
            // TODO:
            return Task.CompletedTask;
        }
    }
}
