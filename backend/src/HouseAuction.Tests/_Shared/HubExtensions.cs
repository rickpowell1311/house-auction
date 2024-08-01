using FakeItEasy;
using Microsoft.AspNetCore.SignalR;

namespace HouseAuction.Tests._Shared
{
    public static class HubExtensions
    {
        public static void WithFakeConnectionId(this HouseAuctionHub hub, string connectionId)
        {
            var fake = A.Fake<IHubCallerClients<IHouseAuctionHub>>();
            var clientProxy = A.Fake<IHouseAuctionHub>();
            var clientContext = A.Fake<HubCallerContext>();
            A.CallTo(() => fake.All).Returns(clientProxy);
            A.CallTo(() => clientContext.ConnectionId).Returns(connectionId);

            hub.Context = clientContext;
        }
    }
}
