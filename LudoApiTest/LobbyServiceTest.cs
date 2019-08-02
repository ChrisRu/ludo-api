using System.Linq;
using System.Numerics;
using LudoApi.Models;
using LudoApi.Services;
using Xunit;

namespace LudoApiTest
{
    public class LobbyServiceTest
    {
        [Fact]
        public void TestCreateLobby()
        {
            var lobbyService = new LobbyService();
            var lobby = lobbyService.CreateLobby("apples", null);

            Assert.Equal("apples", lobby.Name);
            Assert.Equal(0, lobby.Id);
            Assert.Empty(lobby.Players);
        }

        [Fact]
        public void TestGetJoinedLobby()
        {
            var lobbyService = new LobbyService();
            var lobby = lobbyService.CreateLobby("apples", null);
            lobby.AddPlayer("name", Color.Blue);

            var joinedLobby = lobbyService.GetJoinedLobby("name");

            Assert.NotNull(joinedLobby);
        }

        [Fact]
        public void TestGetJoinedLobbyNotJoined()
        {
            var lobbyService = new LobbyService();
            lobbyService.CreateLobby("apples", null);

            var lobby = lobbyService.GetJoinedLobby("2");

            Assert.Null(lobby);
        }

        [Fact]
        public void TestGetLobbies()
        {
            var lobbyService = new LobbyService();
            lobbyService.CreateLobby("apples", null);
            lobbyService.CreateLobby("pears", null);

            var lobbies = lobbyService.GetLobbies();

            Assert.Equal(2, lobbies.Count());
        }

        [Fact]
        public void TestGetLobbiesNon()
        {
            var lobbyService = new LobbyService();

            var lobbies = lobbyService.GetLobbies();

            Assert.Empty(lobbies);
        }

        [Fact]
        public void TestGetLobby()
        {
            var lobbyService = new LobbyService();
            var createdLobby = lobbyService.CreateLobby("apples", null);

            var lobby = lobbyService.GetLobby("apples");

            Assert.Same(createdLobby, lobby);
        }

        [Fact]
        public void TestGetLobbyNonExisting()
        {
            var lobbyService = new LobbyService();
            var createdLobby = lobbyService.CreateLobby("apples", null);

            var lobby = lobbyService.GetLobby("pears");

            Assert.NotSame(createdLobby, lobby);
            Assert.Null(lobby);
        }
    }
}