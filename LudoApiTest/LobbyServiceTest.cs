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
            var lobby = lobbyService.CreateLobby("1", "apples");

            Assert.Equal("lobby-apples", lobby.Name.ToString());
            Assert.Empty(lobby.Players);
        }

        [Fact]
        public void TestGetJoinedLobby()
        {
            var lobbyService = new LobbyService();
            var lobby = lobbyService.CreateLobby("1", "apples");
            lobby.AddPlayer("name", Color.Blue);

            var joinedLobby = lobbyService.GetJoinedLobby("name");

            Assert.NotNull(joinedLobby);
        }

        [Fact]
        public void TestGetJoinedLobbyNotJoined()
        {
            var lobbyService = new LobbyService();
            lobbyService.CreateLobby("1", "apples");

            var lobby = lobbyService.GetJoinedLobby("2");

            Assert.Null(lobby);
        }

        [Fact]
        public void TestGetLobbies()
        {
            var lobbyService = new LobbyService();
            lobbyService.CreateLobby("1", "apples");
            lobbyService.CreateLobby("2", "pears");

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
            var createdLobby = lobbyService.CreateLobby("1", "apples");

            var lobby = lobbyService.GetLobby("apples");

            Assert.Same(createdLobby, lobby);
        }

        [Fact]
        public void TestGetLobbyNonExisting()
        {
            var lobbyService = new LobbyService();
            var createdLobby = lobbyService.CreateLobby("1", "apples");

            var lobby = lobbyService.GetLobby("pears");

            Assert.NotSame(createdLobby, lobby);
            Assert.Null(lobby);
        }
    }
}