using System.Collections.Generic;
using LudoApi.Models;

namespace LudoApi.Services
{
    public interface ILobbyService
    {
        IEnumerable<ILobby> GetLobbies();

        ILobby? GetJoinedLobby(string connectionId);

        ILobby? GetLobby(string lobbyName);

        ILobby CreateLobby(string lobbyName, string connectionId);

        void DestroyLobby(int id);
    }
}