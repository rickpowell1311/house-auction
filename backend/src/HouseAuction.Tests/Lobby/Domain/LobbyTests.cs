﻿using HouseAuction.Lobby.Domain;
using HouseAuction.Tests._Shared.TestData;

namespace HouseAuction.Tests.Lobby.Domain
{
    public class LobbyTests
    {
        [Fact]
        public void TryJoin_WhenLobbyFull_ReturnsFalse()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0], Guid.NewGuid().ToString());
            foreach (var gamer in Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MaxGamers - 1))
            {
                var joinResult = lobby.Join(gamer, Guid.NewGuid().ToString());
                Assert.Equal(HouseAuction.Lobby.Domain.LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            var lastGamer = Gamers.Sample.Skip(HouseAuction.Lobby.Domain.Lobby.MaxGamers).Take(1).Single();

            var lastJoinResult = lobby.Join(lastGamer, Guid.NewGuid().ToString());
            Assert.Equal(LobbyJoinResult.JoinResultType.Error, lastJoinResult.Type);
        }

        [Fact]
        public void TryJoin_WhenAlreadyJoined_ReturnsFalse()
        {
            var gamer = Gamers.Sample[0];
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(gamer, Guid.NewGuid().ToString());
            var joinResult = lobby.Join(gamer, Guid.NewGuid().ToString());

            Assert.Equal(LobbyJoinResult.JoinResultType.Error, joinResult.Type);
        }

        [Fact]
        public void TryJoin_WhenGameStarted_ReturnsFalse()
        {
            var creator = Gamers.Sample[0];
            var otherGamers = Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers - 1);
            var joiningAfterGameStarted = Gamers.Sample.Skip(HouseAuction.Lobby.Domain.Lobby.MinGamers).Take(1).Single();

            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(creator, Guid.NewGuid().ToString());
            foreach (var gamer in otherGamers)
            {
                var joinResult = lobby.Join(gamer, Guid.NewGuid().ToString());
                Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            foreach (var gamer in lobby.Gamers)
            {
                var isReady = lobby.TryReadyUp(gamer.Name, out var _);
                Assert.True(isReady);
            }

            lobby.StartGame();

            var lateJoinResult = lobby.Join(joiningAfterGameStarted, Guid.NewGuid().ToString());
            Assert.Equal(LobbyJoinResult.JoinResultType.Error, lateJoinResult.Type);
        }

        [Fact]
        public void ReadyUp_WhenAllGamersReadyButNotEnoughPlayers_IsNotReadyToStart()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0], Guid.NewGuid().ToString());

            foreach (var gamer in lobby.Gamers)
            {
                gamer.ReadyUp();
            }

            Assert.False(lobby.IsReadyToStartGame);
        }

        [Fact]
        public void ReadyUp_WhenNotAllPlayersReady_IsNotReadyToStart()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0], Guid.NewGuid().ToString());
            foreach (var gamer in Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers - 1))
            {
                var joinResult = lobby.Join(gamer, Guid.NewGuid().ToString());
                Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            Assert.False(lobby.IsReadyToStartGame);
        }

        [Fact]
        public void ReadyUp_WhenAllPlayersReadyAndEnoughPlayers_IsReadyToStart()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0], Guid.NewGuid().ToString());
            foreach (var gamer in Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers))
            {
                var joinResult = lobby.Join(gamer, Guid.NewGuid().ToString());
                Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            foreach (var gamer in lobby.Gamers)
            {
                var isReady = lobby.TryReadyUp(gamer.Name, out var _);
                Assert.True(isReady);
            }

            Assert.True(lobby.IsReadyToStartGame);
        }
    }
}
