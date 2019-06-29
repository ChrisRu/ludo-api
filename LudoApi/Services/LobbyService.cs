using System.Collections.Generic;
using System.Linq;
using LudoApi.Models;

namespace LudoApi.Services
{
    public class LobbyService : ILobbyService
    {
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
            return Lobbies.FirstOrDefault(lobby => lobby.Name.Equals(lobbyName));
        }

        public ILobby CreateLobby(string connectionId, string lobbyName)
        {
            var lobby = new Lobby(lobbyName, new GameService());
            Lobbies.Add(lobby);
            return lobby;
        }

        public void DestroyLobby(string lobbyName)
        {
            var lobby = GetLobby(lobbyName);
            if (lobby != null)
            {
                Lobbies.Remove(lobby);
            }
        }
    }
}