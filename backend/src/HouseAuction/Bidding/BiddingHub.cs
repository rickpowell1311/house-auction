using Google.Api;
using HouseAuction.Bidding.Domain;
using HouseAuction.Bidding.Requests;
using HouseAuction.Infrastructure.HubContext;
using HouseAuction.Infrastructure.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HouseAuction.Bidding
{
    public class BiddingHub : IBiddingHub
    {
        private readonly UserContext _userContext;
        private readonly BiddingContext _context;
        private readonly CallingHubContext _callingHubContext;

        public BiddingHub(
            UserContext userContext,
            BiddingContext context,
            CallingHubContext callingHubContext)
        {
            _userContext = userContext;
            _context = context;
            _callingHubContext = callingHubContext;
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

        public async Task Bid(Bid.BidRequest request)
        {
            var biddingPhase = await _context.BiddingPhases.FindAsync(request.GameId)
                ?? throw new InvalidOperationException($"Bidding phase doesn't exist for game {request.GameId}");

            var round = biddingPhase.CurrentBiddingRound
                ?? throw new HubException($"Unable to make a bid for game {request.GameId} at this time");

            var hand = biddingPhase.Hands
                .Single(x => x.Player == _userContext[request.GameId].Player);

            if (hand.Coins < request.Amount)
            {
                throw new InvalidOperationException(
                    $"Player {_userContext[request.GameId].Player} doesn't have enough coins to bid {request.Amount}");
            }

            round.Bid(_userContext[request.GameId].Player, request.Amount);

            await _context.SaveChangesAsync();

            var playerBank = await _context.Hands.FindAsync([request.GameId, _userContext[request.GameId].Player]);

            await _callingHubContext.Hub.Clients
                .Group(biddingPhase.GameId)
                .AsReceiver<IBiddingReceiver>()
                .OnPlayerTurnComplete(new Reactions.OnPlayerTurnComplete
                {
                    Player = _userContext[biddingPhase.GameId].Player,
                    NextPlayer = biddingPhase.PlayerCycle.CurrentPlayer,
                    Result = new Reactions.OnPlayerTurnComplete.OnPlayerTurnFinishedResult
                    {
                        RemainingCoins = playerBank.Coins,
                        Passed = false,
                        Bid = request.Amount
                    }
                });

            if (round.HasFinished)
            {
                await NotifyRoundFinished(biddingPhase);
            }
        }

        public async Task Pass(Pass.PassRequest request)
        {
            var biddingPhase = await _context.BiddingPhases.FindAsync(request.GameId)
                ?? throw new InvalidOperationException($"Bidding phase doesn't exist for game {request.GameId}");

            var round = biddingPhase.CurrentBiddingRound
                ?? throw new HubException($"Unable to make a bid for game {request.GameId} at this time");

            round.Pass(_userContext[request.GameId].Player);

            await _context.SaveChangesAsync();

            var playerBank = await _context.Hands.FindAsync([request.GameId, _userContext[request.GameId].Player]);
            _context.Entry(biddingPhase).State = EntityState.Detached;
            await _context.Entry(biddingPhase).ReloadAsync();

            await _callingHubContext.Hub.Clients
                .Group(biddingPhase.GameId)
                .AsReceiver<IBiddingReceiver>()
                .OnPlayerTurnComplete(new Reactions.OnPlayerTurnComplete
                {
                    Player = _userContext[biddingPhase.GameId].Player,
                    NextPlayer = biddingPhase.PlayerCycle.CurrentPlayer,
                    Result = new Reactions.OnPlayerTurnComplete.OnPlayerTurnFinishedResult
                    {
                        RemainingCoins = playerBank.Coins,
                        Passed = true
                    }
                });

            if (round.HasFinished)
            {
                await NotifyRoundFinished(biddingPhase);
            }
        }

        private async Task NotifyRoundFinished(BiddingPhase biddingPhase)
        {
            var playerBanks = await _context.Hands
                .Where(x => x.BiddingPhaseId == biddingPhase.GameId)
                .ToDictionaryAsync(x => x.Player, x => x.Coins);

            foreach (var player in biddingPhase.PlayerCycle.Players.Values)
            {
                await _callingHubContext.Hub.Clients
                    .IndividualGroupForPlayer(biddingPhase.GameId, player)
                    .AsReceiver<IBiddingReceiver>()
                    .OnBiddingRoundComplete(new Reactions.OnBiddingRoundComplete
                    {
                        CoinsRemaining = playerBanks[player],
                        NextRound = new Reactions.OnBiddingRoundComplete.OnBiddingRoundCompleteNextRound
                        {
                            Properties = biddingPhase.Deck.ForRound(
                                biddingPhase.CurrentBiddingRound.RoundNumber),
                            IsLastRound = biddingPhase.BiddingRounds.Max(x => x.RoundNumber) == biddingPhase.CurrentBiddingRound.RoundNumber
                        }
                    });
            }
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