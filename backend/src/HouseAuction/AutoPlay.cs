using HouseAuction.Bidding;
using HouseAuction.Bidding.Domain;
using HouseAuction.Bidding.Domain.Events;
using HouseAuction.Infrastructure.HubContext;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Onwrd.EntityFrameworkCore;
using System.Runtime.InteropServices;

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
            string humanPlayer,
            string gameId)
        {
            var biddingPhase = await _biddingContext.BiddingPhases.FindAsync(gameId)
                ?? throw new InvalidOperationException($"Bidding phase doesn't exist for game {gameId}");

            var chanceOfPassing = 0.3d;
            var thinkingTime = 2000;

            while (biddingPhase.PlayerCycle.CurrentPlayer != humanPlayer && !biddingPhase.HasFinished)
            {
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

                await Task.Delay(thinkingTime);

                await _hubContext
                    .Clients
                    .Group(biddingPhase.GameId)
                    .AsReceiver<IBiddingReceiver>()
                    .OnPlayerTurnFinished(new Bidding.Reactions.OnPlayerTurnFinished
                    {
                        Player = currentPlayer,
                        NextPlayer = biddingPhase.PlayerCycle.CurrentPlayer,
                        Result = new Bidding.Reactions.OnPlayerTurnFinished.OnPlayerTurnFinishedResult
                        {
                            Passed = pass,
                            Bid = highestBid
                        }
                    });
            }
        }
    }
}
