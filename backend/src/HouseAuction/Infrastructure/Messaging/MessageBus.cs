namespace HouseAuction.Infrastructure.Messaging
{
    public class MessageBus : IMessageBus
    {
        private readonly Exchange _exchange;

        public MessageBus(Exchange exchange)
        {
            _exchange = exchange;
        }

        public async Task Send<T>(T message)
        {
            await _exchange.Publish(message);
        }
    }
}
