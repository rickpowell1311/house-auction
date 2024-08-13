namespace HouseAuction.Infrastructure.Messaging
{
    public class Exchange
    {
        private readonly IServiceProvider _serviceProvider;

        public Exchange(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Publish<T>(T message)
        {
            using var messageExchangeScope = _serviceProvider.CreateScope();

            foreach (var subscriber in messageExchangeScope.ServiceProvider
                .GetRequiredService<IEnumerable<IMessageSubscriber>>()
                .Select(x => x as IMessageSubscriber<T>)
                .Where(x => x != default))
            {
                await subscriber.Handle(message);
            }
        }
    }
}
