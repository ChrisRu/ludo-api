using System.Collections.Generic;
using System.Linq;
using LudoApi.Models;

namespace LudoApi.Services
{
    public class LobbyService : ILobbyService
    {
        private const string LobbyPrefix = "lobby-";
        
        private List<ILobby> Lobbies { get; } = new List<ILobby>();

        public IEnumerable<ILobby> GetLobbies()
        {
            return Lobbies;
        }

        public ILobby? GetJoinedLobby(string connectionId)
        {
            return Lobbies.Find(lobby => lobby.Players.Any(player => player.ConnectionId == connectionId));
        }

        public ILobby? GetLobby(string lobbyName)
        {
            lobbyName = LobbyPrefix + lobbyName.Replace(LobbyPrefix, string.Empty);
            
            return Lobbies.FirstOrDefault(lobby => lobby.Name == lobbyName);
        }

        public ILobby CreateLobby(string connectionId, string lobbyName)
        {
            lobbyName = LobbyPrefix + lobbyName.Replace(LobbyPrefix, string.Empty);

            var lobby = new Lobby(lobbyName, new GameService(), new Player(connectionId, Color.Red, true));
            Lobbies.Add(lobby);
            return lobby;
        }
    }
}