using System.Linq;
using LudoApi.Models;
using Xunit;

namespace LudoApiTest
{
    public class LobbyTest
    {
        [Fact]
        public void TestCreateLobby()
        {
            var lobby = new Lobby("1", null);

            Assert.NotNull(lobby);
            Assert.Equal("lobby-1", lobby.Name.ToString());
            Assert.Empty(lobby.Players);
            Assert.Null(lobby.Game);
        }

        [Fact]
        public void TestJoinLobby()
        {
            var lobby = new Lobby("1", null);
            lobby.AddPlayer("connection-id-1", Color.Red);
            lobby.AddPlayer("connection-id-2", Color.Blue);

            Assert.Equal(2, lobby.Players.Count());
            Assert.Equal("connection-id-1", lobby.Players.ElementAt(0).ConnectionId);
            Assert.Equal("connection-id-2", lobby.Players.ElementAt(1).ConnectionId);
        }

        [Fact]
        public void TestLeaveLobby()
        {
            var lobby = new Lobby("1", null);
            lobby.AddPlayer("connection-id-1", Color.Red);
            lobby.RemovePlayer("connection-id-1");

            Assert.Empty(lobby.Players);
        }

        [Fact]
        public void TestLeaveLobbyNonExistent()
        {
            var lobby = new Lobby("1", null);
            lobby.AddPlayer("connection-id-1", Color.Red);
            lobby.RemovePlayer("connection-id-2");

            Assert.Single(lobby.Players);
        }
    }
}