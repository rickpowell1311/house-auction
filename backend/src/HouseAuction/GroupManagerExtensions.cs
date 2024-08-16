using Microsoft.AspNetCore.SignalR;

namespace HouseAuction
{
    public static class GroupManagerExtensions
    {
        private static string IndividualGroupNameForPlayer(string gameId, string player)
        {
            return $"{gameId}-{player}";
        }

        public static async Task AddPlayerAsIndividualGroup(
            this IGroupManager groupManager,
            string connectionId,
            string gameId,
            string player)
        {
            await groupManager.AddToGroupAsync(connectionId, IndividualGroupNameForPlayer(gameId, player));
        }

        public static IClientProxy IndividualGroupForPlayer(
            this IHubClients clients,
            string gameId,
            string player)
        {
            return clients.Group(IndividualGroupNameForPlayer(gameId, player));
        }

        public static T IndividualGroupForPlayer<T>(
            this IHubClients<T> clients,
            string gameId,
            string player)
        {
            return clients.Group(IndividualGroupNameForPlayer(gameId, player));
        }
    }
}
