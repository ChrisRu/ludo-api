using System.Collections.Generic;
using LudoApi.Models;

namespace LudoApi.Services
{
    public interface ILobbyService
    {
        IEnumerable<ILobby> GetLobbies();

        ILobby? GetJoinedLobby(string connectionId);

        ILobby? GetLobby(string lobbyName);

        ILobby CreateLobby(string connectionId, string lobbyName);

        void DestroyLobby(string lobbyName);
    }
}