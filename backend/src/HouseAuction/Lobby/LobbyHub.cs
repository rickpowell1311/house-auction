using Microsoft.AspNetCore.SignalR;

namespace HouseAuction.Lobby
{
    public class LobbyHub(Participants _participants) : Hub<ILobby>
    {
        public async Task JoinLobby(string name)
        {
            if (_participants.Any(x => FuzzyNameMatch.IsCloseMatch(x, name)))
            {
                throw new HubException("Similar user name already in use");
            }

            _participants.Add(name);

            await Clients.All.OnGamerJoined(name);
        }
    }
}
