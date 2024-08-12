using Microsoft.EntityFrameworkCore;
using Onwrd.EntityFrameworkCore;
using System.Text.Json;

namespace HouseAuction.Bidding
{
    public class BiddingContext(DbContextOptions<BiddingContext> options) : DbContext(options)
    {
        public DbSet<Domain.BiddingPhase> BiddingPhases { get; set; }

        public DbSet<Domain.BiddingRound> BiddingRounds { get; set; }

        public DbSet<Domain.Hand> Hands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddOnwrdModel();

            var biddingPhaseConfig = modelBuilder.Entity<Domain.BiddingPhase>();
            biddingPhaseConfig.HasKey(x => x.GameId);
            biddingPhaseConfig.OwnsOne(x => x.Deck);
            biddingPhaseConfig.HasMany(x => x.BiddingRounds)
                .WithOne()
                .HasForeignKey(x => x.BiddingPhaseId);

            var biddingRoundConfig = modelBuilder.Entity<Domain.BiddingRound>();
            biddingRoundConfig.HasKey(x => new { x.BiddingPhaseId, x.RoundNumber });
            biddingRoundConfig.OwnsOne(x => x.PlayerCycle);
            biddingRoundConfig.Property(x => x.Plays)
                .HasConversion(
                    x => JsonSerializer.Serialize(x, default(JsonSerializerOptions)),
                    x => JsonSerializer.Deserialize<List<Domain.Play>>(x, default(JsonSerializerOptions)));

            var handConfig = modelBuilder.Entity<Domain.Hand>();
            handConfig.HasKey(x => new { x.BiddingPhaseId, x.Player });
            handConfig.Property(x => x.Properties)
                .HasConversion(
                    x => JsonSerializer.Serialize(x, default(JsonSerializerOptions)),
                    x => JsonSerializer.Deserialize<List<int>>(x, default(JsonSerializerOptions)));

            base.OnModelCreating(modelBuilder);
        }
    }
}
