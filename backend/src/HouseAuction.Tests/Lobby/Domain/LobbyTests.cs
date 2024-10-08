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
            var connectionId = Guid.NewGuid().ToString();
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(gamer, connectionId);
            var joinResult = lobby.Join(gamer, connectionId);

            Assert.Equal(LobbyJoinResult.JoinResultType.Error, joinResult.Type);
        }

        [Fact]
        public void TryJoin_WhenGameStarted_ReturnsFalse()
        {
            var creator = Gamers.Sample[0];
            var creatorConnectionId = Guid.NewGuid().ToString();
            var otherGamers = Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers - 1);
            var joiningAfterGameStarted = Gamers.Sample.Skip(HouseAuction.Lobby.Domain.Lobby.MinGamers).Take(1).Single();

            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(creator, creatorConnectionId);
            foreach (var gamer in otherGamers)
            {
                var joinResult = lobby.Join(gamer, Guid.NewGuid().ToString());
                Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            foreach (var gamer in lobby.Gamers)
            {
                var isReady = lobby.TryReadyUp(gamer.ConnectionId, out var _);
                Assert.True(isReady);
            }

            var isGameBegun = lobby.TryBeginGame(creatorConnectionId, out var _);
            Assert.True(isGameBegun);

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
            var creator = Gamers.Sample[0];
            var gamers = Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers);

            LobbyReadyToStartGame(creator, gamers);
        }

        [Fact]
        public void BeginGame_WhenIsNotReadyToStart_ReturnsFalse()
        {
            var creator = Gamers.Sample[0];
            var creatorConnectionId = Guid.NewGuid().ToString();
            var otherGamers = Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers - 1);

            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(creator, creatorConnectionId);
            foreach (var gamer in otherGamers)
            {
                var joinResult = lobby.Join(gamer, Guid.NewGuid().ToString());
                Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            var isGameBegun = lobby.TryBeginGame(creatorConnectionId, out var _);

            Assert.False(isGameBegun);
        }

        [Fact]
        public void BeginGame_WhenGamerIsNotCreator_ReturnsFalse()
        {
            var creator = Gamers.Sample[0];
            var otherGamers = Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers - 1);

            var lobby = LobbyReadyToStartGame(creator, otherGamers);
            var otherGamer = lobby.Gamers.First(x => x.Name != creator);

            var isGameBegun = lobby.TryBeginGame(otherGamer.ConnectionId, out var _);
            Assert.False(isGameBegun);
        }

        [Fact]
        public void BeginGame_ReturnsTrue()
        {
            var creator = Gamers.Sample[0];
            var otherGamers = Gamers.Sample.Skip(1).Take(HouseAuction.Lobby.Domain.Lobby.MinGamers);

            var lobby = LobbyReadyToStartGame(creator, otherGamers);
            var creatorConnectionId = lobby.Creator.ConnectionId;

            var isGameBegun = lobby.TryBeginGame(creatorConnectionId, out var _);
            Assert.True(isGameBegun);
        }

        private static HouseAuction.Lobby.Domain.Lobby LobbyReadyToStartGame(string creator, IEnumerable<string> otherGamers)
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(creator, Guid.NewGuid().ToString());
            foreach (var gamer in otherGamers)
            {
                var joinResult = lobby.Join(gamer, Guid.NewGuid().ToString());
                Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            }

            foreach (var gamer in lobby.Gamers)
            {
                var isReady = lobby.TryReadyUp(gamer.ConnectionId, out var _);
                Assert.True(isReady);
            }

            Assert.True(lobby.IsReadyToStartGame);

            return lobby;
        }
    }
}
