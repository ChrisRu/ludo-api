using System.Collections.Generic;
using System.Linq;
using LudoApi.Models;

namespace LudoApi.Services
{
    public class LobbyService : ILobbyService
    {
        private List<ILobby> Lobbies { get; set; } = new List<ILobby>();

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
            return Lobbies.FirstOrDefault(lobby => lobby.Name == lobbyName);
        }

        public ILobby CreateLobby(string lobbyName, string connectionId)
        {
            var lobby = new Lobby(Lobbies.Count, lobbyName, connectionId, new GameService());
            Lobbies.Add(lobby);
            return lobby;
        }

        public void DestroyLobby(int id)
        {
            Lobbies = Lobbies.Where(lobby => lobby.Id != id).ToList();
        }
    }
}