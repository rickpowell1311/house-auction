using HouseAuction.Bidding.Domain;
using Microsoft.EntityFrameworkCore;
using Onwrd.EntityFrameworkCore;
using System.Text.Json;

namespace HouseAuction.Bidding
{
    public class BiddingContext(DbContextOptions<BiddingContext> options) : DbContext(options)
    {
        public DbSet<BiddingPhase> BiddingPhases { get; set; }

        public DbSet<BiddingRound> BiddingRounds { get; set; }

        public DbSet<Hand> Hands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddOnwrdModel();

            var biddingPhaseConfig = modelBuilder.Entity<BiddingPhase>();
            biddingPhaseConfig.HasKey(x => x.GameId);
            biddingPhaseConfig.OwnsOne(x => x.Deck, d =>
            {
                d.Property(x => x.Properties)
                    .HasField("_properties")
                    .HasConversion(
                        x => JsonSerializer.Serialize(x, default(JsonSerializerOptions)),
                        x => JsonSerializer.Deserialize<List<int>>(x, default(JsonSerializerOptions)));
            });
            biddingPhaseConfig.HasMany(x => x.BiddingRounds)
                .WithOne(x => x.BiddingPhase);
            biddingPhaseConfig.Navigation(x => x.BiddingRounds).AutoInclude();
            biddingPhaseConfig.HasMany(x => x.Hands)
                .WithOne()
                .HasForeignKey(x => x.BiddingPhaseId);
            biddingPhaseConfig.Navigation(x => x.Hands).AutoInclude();
            biddingPhaseConfig.HasOne(x => x.PlayerCycle)
                .WithOne()
                .HasForeignKey<PlayerCycle>(x => x.BiddingPhaseId);
            biddingPhaseConfig.Navigation(x => x.PlayerCycle)
                .AutoInclude();

            var biddingRoundConfig = modelBuilder.Entity<BiddingRound>();
            biddingRoundConfig.HasKey(x => x.Id);
            biddingRoundConfig.OwnsMany(x => x.Plays);
            biddingRoundConfig.Navigation(x => x.BiddingPhase)
                .AutoInclude();

            var handConfig = modelBuilder.Entity<Hand>();
            handConfig.HasKey(x => new { x.BiddingPhaseId, x.Player });
            handConfig.Property(x => x.Properties)
                .HasConversion(
                    x => JsonSerializer.Serialize(x, default(JsonSerializerOptions)),
                    x => JsonSerializer.Deserialize<List<int>>(x, default(JsonSerializerOptions)));

            var playerCycleConfig = modelBuilder.Entity<PlayerCycle>();
            playerCycleConfig.HasKey(x => x.Id);
            playerCycleConfig.Property(x => x.Players)
                .HasField("_players")
                .HasConversion(
                    x => JsonSerializer.Serialize(x, default(JsonSerializerOptions)),
                    x => JsonSerializer.Deserialize<Dictionary<int, string>>(x, default(JsonSerializerOptions)));

            base.OnModelCreating(modelBuilder);
        }
    }
}
