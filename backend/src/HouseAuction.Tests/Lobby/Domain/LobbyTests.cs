using HouseAuction.Tests._Shared.TestData;

namespace HouseAuction.Tests.Lobby.Domain
{
    public class LobbyTests
    {
        [Fact]
        public void TryJoin_WhenLobbyFull_ReturnsFalse()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0]);
            foreach (var gamer in Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MaxGamers - 1))
            {
                var joinResult = lobby.TryJoin(gamer, out _);
                Assert.True(joinResult);
            }

            var lastGamer = Gamers.Sample.Skip(HouseAuction.Lobby.Domain.Lobby.MaxGamers).Take(1).Single();

            var result = lobby.TryJoin(lastGamer, out var _);
            Assert.False(result);
        }

        [Fact]
        public void StartGame_WhenNotEnoughPlayers_ReturnsFalse()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0]);

            foreach (var gamer in lobby.Gamers)
            {
                gamer.ReadyUp();
            }

            Assert.False(lobby.TryBeginGame(out var _));
        }

        [Fact]
        public void TryBeginGame_WhenNotAllPlayersReady_ReturnsFalse()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0]);
            foreach (var gamer in Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers - 1))
            {
                var joinResult = lobby.TryJoin(gamer, out _);
                Assert.True(joinResult);
            }

            Assert.False(lobby.TryBeginGame(out var _));
        }

        [Fact]
        public void TryBeginGame_WhenAllPlayersReady_ReturnsTrue()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0]);
            foreach (var gamer in Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers))
            {
                var joinResult = lobby.TryJoin(gamer, out _);
                Assert.True(joinResult);
            }

            foreach (var gamer in lobby.Gamers)
            {
                gamer.ReadyUp();
            }

            Assert.True(lobby.TryBeginGame(out var _));
        }
    }
}
