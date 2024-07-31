using HouseAuction.Lobby;

namespace HouseAuction
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void MapHouseAuctionRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapLobbyRoutes();
        }
    }
}
