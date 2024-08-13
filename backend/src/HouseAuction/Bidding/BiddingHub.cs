using HouseAuction.Bidding.Domain;
using HouseAuction.Bidding.Requests;
using HouseAuction.Infrastructure.HubContext;
using HouseAuction.Infrastructure.Identity;
using System.Numerics;

namespace HouseAuction.Bidding
{
    public class BiddingHub : IBiddingHub
    {
        private readonly UserContext _userContext;
        private readonly BiddingContext _context;

        public BiddingHub(
            UserContext userContext,
            BiddingContext context)
        {
            _userContext = userContext;
            _context = context;
        }

        public async Task<GetBiddingPhase.GetBiddingPhaseResponse> GetBiddingPhase(GetBiddingPhase.GetBiddingPhaseRequest request)
        {
            var biddingPhase = await _context.BiddingPhases.FindAsync(request.GameId)
                ?? throw new InvalidOperationException($"Bidding phase doesn't exist for game {request.GameId}");

            return new GetBiddingPhase.GetBiddingPhaseResponse
            {
                Deck = MapToDeck(biddingPhase),
                Players = MapToPlayers(biddingPhase)
            };
        }

        private GetBiddingPhase.GetBiddingPhaseDeckResponse MapToDeck(BiddingPhase biddingPhase)
        {
            return new GetBiddingPhase.GetBiddingPhaseDeckResponse
            {
                PropertiesOnTheTable = biddingPhase.CurrentBiddingRound != null
                        ? biddingPhase.Deck.ForRound(biddingPhase.CurrentBiddingRound.RoundNumber)
                        : [],
                TotalProperties = biddingPhase.Deck.Properties.Count()
            };
        }

        private GetBiddingPhase.GetBiddingPhasePlayersResponse MapToPlayers(BiddingPhase biddingPhase)
        {
            var me = _userContext[biddingPhase.GameId].Player;

            return new GetBiddingPhase.GetBiddingPhasePlayersResponse
            {
                LatestWinner = biddingPhase.PreviousBiddingRound?.Plays.HighestBidder(),
                Me = MapMe(biddingPhase),
                Others = biddingPhase.PlayerCycle.Players.Values
                    .Except([me])
                    .Select(x => MapOtherPerson(biddingPhase, x))
                    .ToList()
            };
        }

        private GetBiddingPhase.GetBiddingPhaseMeResponse MapMe(BiddingPhase biddingPhase)
        {
            var player = _userContext[biddingPhase.GameId].Player;
            var playerHand = biddingPhase.Hands.Single(x => x.Player == player);

            return new GetBiddingPhase.GetBiddingPhaseMeResponse
            {
                Name = player,
                Bid = new GetBiddingPhase.GetBiddingPhaseBidResponse
                {
                    Amount = biddingPhase.CurrentBiddingRound.Plays
                                .HighestBid(player),
                    HasPassed = biddingPhase.CurrentBiddingRound.Plays
                        .PlayersWhoPassed()
                                .Contains(player)
                },
                BoughtProperties = biddingPhase.Hands
                    .Single(x => x.Player == player)
                    .Properties
                    .ToList(),
                Coins = playerHand.Coins,
                IsTurn = biddingPhase.PlayerCycle.CurrentPlayer == player,
                Order = biddingPhase.PlayerCycle.Players
                    .Single(x => x.Value == player)
                    .Key
            };
        }

        private GetBiddingPhase.GetBiddingPhaseOtherPersonResponse MapOtherPerson(
            BiddingPhase biddingPhase,
            string player)
        {
            var playerHand = biddingPhase.Hands.Single(x => x.Player == player);

            return new GetBiddingPhase.GetBiddingPhaseOtherPersonResponse
            {
                Name = player,
                Bid = new GetBiddingPhase.GetBiddingPhaseBidResponse
                {
                    Amount = biddingPhase.CurrentBiddingRound.Plays
                                .HighestBid(player),
                    HasPassed = biddingPhase.CurrentBiddingRound.Plays
                        .PlayersWhoPassed()
                                .Contains(player)
                },
                NumberOfBoughtProperties = biddingPhase.Hands
                    .Single(x => x.Player == player)
                    .Properties.Count,
                IsTurn = biddingPhase.PlayerCycle.CurrentPlayer == player,
                Order = biddingPhase.PlayerCycle.Players
                    .Single(x => x.Value == player)
                    .Key
            };
        }
    }
}