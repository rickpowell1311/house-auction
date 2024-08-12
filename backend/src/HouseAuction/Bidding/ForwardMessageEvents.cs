using HouseAuction.Infrastructure.Messaging;
using Onwrd.EntityFrameworkCore;

namespace HouseAuction.Bidding
{
    public class ForwardMessageEvents : IOnwardProcessor
    {
        private readonly IMessageBus _messageBus;

        public ForwardMessageEvents(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task Process<T>(T @event, 
            EventMetadata eventMetadata, 
            CancellationToken cancellationToken = default)
        {
            if (@event is IMessage)
            {
                await _messageBus.Send(@event);
            }
        }
    }
}
