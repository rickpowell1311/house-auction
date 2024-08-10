namespace HouseAuction.Infrastructure.Messaging
{
    public class Exchange
    {
        private readonly IEnumerable<IMessageSubscriber> _subscribers;

        public Exchange(IEnumerable<IMessageSubscriber> subscribers)
        {
            _subscribers = subscribers;
        }

        public async Task Publish<T>(T message)
        {
            foreach (var subscriber in _subscribers.OfType<IMessageSubscriber<T>>())
            {
                await subscriber.Handle(message);
            }
        }
    }
}
