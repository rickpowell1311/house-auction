﻿namespace HouseAuction.Lobby
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void MapLobbyRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapHub<HouseAuctionHub>(HouseAuctionHub.Route);
        }
    }
}
