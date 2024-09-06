using HouseAuction.Bidding;
using HouseAuction.Bidding.Domain;
using HouseAuction.Infrastructure.HubContext;
using HouseAuction.Infrastructure.Identity;
using Microsoft.AspNetCore.SignalR;
using static Google.Rpc.Context.AttributeContext.Types;

namespace HouseAuction
{
    public class AutoPlay
    {
        private readonly BiddingContext _biddingContext;
        private readonly IHubContext<HouseAuctionHub> _hubContext;

        public AutoPlay(
            BiddingContext biddingContext,
            IHubContext<HouseAuctionHub> hubContext)
        {
            _biddingContext = biddingContext;
            _hubContext = hubContext;
        }

        public async Task Run(
            UserContext userContext,
            string gameId)
        {
            var humanPlayer = userContext[gameId].Player;
            var biddingPhase = await _biddingContext.BiddingPhases.FindAsync(gameId)
                ?? throw new InvalidOperationException($"Bidding phase doesn't exist for game {gameId}");

            var chanceOfPassing = 0.5d;
            var thinkingTime = 200;

            while (biddingPhase.PlayerCycle.CurrentPlayer != humanPlayer && !biddingPhase.HasFinished)
            {
                await Task.Delay(thinkingTime);

                var biddingRound = biddingPhase.CurrentBiddingRound;
                var playerBanks = biddingPhase.Hands
                    .ToDictionary(x => x.Player, x => x.Coins);
                var currentPlayer = biddingPhase.PlayerCycle.CurrentPlayer;
                var highestBid = biddingRound.Plays.HighestBid();

                var pass = new Random().NextDouble() < chanceOfPassing
                    || highestBid >= playerBanks[currentPlayer]
                    || biddingRound.Plays.PlayersWhoPassed().Contains(currentPlayer);

                if (pass)
                {
                    biddingRound.Pass(currentPlayer);
                }
                else
                {
                    highestBid++;

                    biddingRound.Bid(biddingPhase.PlayerCycle.CurrentPlayer, highestBid);
                }

                await _biddingContext.SaveChangesAsync();

                var hand = await _biddingContext.Hands.FindAsync([biddingPhase.GameId, humanPlayer]);

                await _hubContext
                    .Clients
                    .Group(biddingPhase.GameId)
                    .AsReceiver<IBiddingReceiver>()
                    .OnPlayerTurnComplete(new Bidding.Reactions.OnPlayerTurnComplete
                    {
                        Player = currentPlayer,
                        NextPlayer = biddingPhase.PlayerCycle.CurrentPlayer,
                        Result = new Bidding.Reactions.OnPlayerTurnComplete.OnPlayerTurnFinishedResult
                        {
                            RemainingCoins = hand.Coins,
                            Passed = pass,
                            Bid = highestBid
                        }
                    });


                if (biddingRound.HasFinished)
                {
                    await _hubContext
                        .Clients
                        .IndividualGroupForPlayer(biddingPhase.GameId, humanPlayer)
                        .AsReceiver<IBiddingReceiver>()
                        .OnBiddingRoundComplete(new Bidding.Reactions.OnBiddingRoundComplete
                        {
                            CoinsRemaining = hand.Coins,
                            NextRound = biddingPhase.CurrentBiddingRound != null
                                ? new Bidding.Reactions.OnBiddingRoundComplete.OnBiddingRoundCompleteNextRound
                                    {
                                        Properties = biddingPhase.Deck.ForRound(
                                            biddingPhase.CurrentBiddingRound.RoundNumber),
                                        IsLastRound = biddingPhase.BiddingRounds
                                            .Max(x => x.RoundNumber) == biddingPhase.CurrentBiddingRound.RoundNumber
                                    }
                                : default
                        });
                }
            }
        }
    }
}
