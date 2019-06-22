using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace LudoApi.Hubs
{
    public class LobbyManager
    {
        private IList<(string, List<string>)> Lobbies { get; } = new List<(string, List<string>)>();

        public List<string> GetLobbyMembers(string lobbyName)
        {
            return Lobbies
                .Where(lobby => lobby.Item1 == lobbyName)
                .DefaultIfEmpty((string.Empty, new List<string>()))
                .First()
                .Item2;
        }

        public void JoinLobby(string connectionId, string lobbyName)
        {
            Lobbies
                .Where(lobby => lobby.Item1 == lobbyName)
                .DefaultIfEmpty((string.Empty, new List<string>()))
                .First()
                .Item2
                .Add(connectionId);
        }

        public bool CreateLobby(string lobbyName)
        {
            if (Lobbies.Any(lobby => lobby.Item1 == lobbyName))
            {
                return false;
            }

            Lobbies.Add((lobbyName, new List<string>()));
            return true;
        }
    }
}