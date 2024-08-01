using HouseAuction.Lobby.Domain;
using HouseAuction.Tests._Shared.TestData;
using Microsoft.AspNetCore.Mvc;

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
                var joinResult = lobby.Join(gamer);
                Assert.Equal(HouseAuction.Lobby.Domain.LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            var lastGamer = Gamers.Sample.Skip(HouseAuction.Lobby.Domain.Lobby.MaxGamers).Take(1).Single();

            var lastJoinResult = lobby.Join(lastGamer);
            Assert.Equal(LobbyJoinResult.JoinResultType.Error, lastJoinResult.Type);
        }

        [Fact]
        public void TryJoin_WhenAlreadyJoined_ReturnsFalse()
        {
            var gamer = Gamers.Sample[0];
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(gamer);
            var joinResult = lobby.Join(gamer);

            Assert.Equal(LobbyJoinResult.JoinResultType.Error, joinResult.Type);
        }

        [Fact]
        public void TryJoin_WhenGameStarted_ReturnsFalse()
        {
            var creator = Gamers.Sample[0];
            var otherGamers = Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers - 1);
            var joiningAfterGameStarted = Gamers.Sample.Skip(HouseAuction.Lobby.Domain.Lobby.MinGamers).Take(1).Single();

            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(creator);
            foreach (var gamer in otherGamers)
            {
                var joinResult = lobby.Join(gamer);
                Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            foreach (var gamer in lobby.Gamers)
            {
                gamer.ReadyUp();
            }

            var gameStartResult = lobby.TryBeginGame(out var _);
            Assert.True(gameStartResult);

            var lateJoinResult = lobby.Join(joiningAfterGameStarted);
            Assert.Equal(LobbyJoinResult.JoinResultType.Error, lateJoinResult.Type);
        }

        [Fact]
        public void TryBeginGame_WhenNotEnoughPlayers_ReturnsFalse()
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
                var joinResult = lobby.Join(gamer);
                Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            Assert.False(lobby.TryBeginGame(out var _));
        }

        [Fact]
        public void TryBeginGame_WhenAllPlayersReady_ReturnsTrue()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0]);
            foreach (var gamer in Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers))
            {
                var joinResult = lobby.Join(gamer);
                Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            foreach (var gamer in lobby.Gamers)
            {
                gamer.ReadyUp();
            }

            Assert.True(lobby.TryBeginGame(out var _));
        }
    }
}
