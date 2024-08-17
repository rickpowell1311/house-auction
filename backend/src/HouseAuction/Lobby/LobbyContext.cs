using Microsoft.EntityFrameworkCore;
using Onwrd.EntityFrameworkCore;

namespace HouseAuction.Lobby
{
    public class LobbyContext(DbContextOptions<LobbyContext> options) : DbContext(options)
    {
        public DbSet<Domain.Lobby> Lobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddOnwrdModel();

            var lobbyConfiguration = modelBuilder.Entity<Domain.Lobby>();
            lobbyConfiguration.HasKey(x => x.GameId);
            lobbyConfiguration.OwnsMany(x => x.Gamers, g =>
            {
                g.HasKey(y => new { y.Name, y.GameId });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
