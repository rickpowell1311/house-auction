using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using static HouseAuction.Tests.Lobby._Shared.HouseAuctionServicesFixture;

namespace HouseAuction.Tests.Lobby._Shared
{
    [CollectionDefinition(CollectionName)]
    public class HouseAuctionServicesFixture : ICollectionFixture<Provider>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.

        public const string CollectionName = "House Auction Services Fixture";

        public class Provider : IAsyncDisposable
        {
            private readonly IWebHost _host;
            private readonly ServiceProvider _serviceProvider;

            public Provider()
            {
                var services = new ServiceCollection();
                services.AddHouseAuction();

                _serviceProvider = services.BuildServiceProvider();
            }

            public T GetService<T>() 
            {
                return _serviceProvider.GetRequiredService<T>();
            }

            public async ValueTask DisposeAsync()
            {
                await _serviceProvider.DisposeAsync();
            }
        }
    }
}
