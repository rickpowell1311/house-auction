using HouseAuction.Infrastructure.Messaging;
using HouseAuction.Messages.Lobby;
using Onwrd.EntityFrameworkCore;

namespace HouseAuction.Lobby
{
    public class ForwardLobbyConfirmedMessage(IMessageBus messageBus) 
        : IOnwardProcessor<LobbyConfirmed>
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task Process(LobbyConfirmed @event, EventMetadata eventMetadata, CancellationToken cancellationToken = default)
        {
            await _messageBus.Send(@event);
        }
    }
}
