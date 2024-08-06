using FakeItEasy;
using HouseAuction.Lobby;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static HouseAuction.Tests._Shared.HubClientFixture;

namespace HouseAuction.Tests._Shared
{
    [CollectionDefinition(CollectionName)]
    public class HubClientFixture : ICollectionFixture<Provider>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.

        public const string CollectionName = "Hub Fixture";

        public class Provider : IAsyncDisposable
        {
            private readonly IWebHost _host;

            public Provider()
            {
                _host = new WebHostBuilder()
                    .ConfigureServices(x => x.AddHouseAuction())
                    .Configure(appBuilder =>
                    {
                        appBuilder.UseRouting();
                        appBuilder.UseEndpoints(e =>
                        {
                            e.MapHub<HouseAuctionHub>(HouseAuctionHub.Route);
                        });
                    })
                    .UseKestrel()
                    .UseUrls("http://127.0.0.1:0")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .Build();

                _host.Start();
            }
            public async Task<HubConnection> StartHubConnection<THub>(string route) 
                where THub : Hub
            {
                var url = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.Single();
                var hubRoute = route.StartsWith("/") ? route.Remove(0, 1) : route;

                var hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{url}/{hubRoute}")
                    .Build();

                await hubConnection.StartAsync();

                return hubConnection;
            }

            public async ValueTask DisposeAsync()
            {
                await _host.StopAsync();
                _host.Dispose();
            }
        }
    }
}
