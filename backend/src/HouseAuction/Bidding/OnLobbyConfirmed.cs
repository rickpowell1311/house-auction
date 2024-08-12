using HouseAuction.Messages.Lobby;

namespace HouseAuction.Bidding
{
    public class OnLobbyConfirmed : IMessageSubscriber<LobbyConfirmed>
    {
        private readonly BiddingContext _context;

        public OnLobbyConfirmed(BiddingContext context)
        {
            _context = context;
        }

        public async Task Handle(LobbyConfirmed message)
        {
            var biddingPhase = await _context.BiddingPhases.FindAsync(message.GameId);

            if (biddingPhase == null)
            {
                biddingPhase = new Domain.BiddingPhase(message.GameId, message.Gamers.Select(x => x.GroupName).ToList());

                _context.BiddingPhases.Add(biddingPhase);

                await _context.SaveChangesAsync();
            }
        }
    }
}
