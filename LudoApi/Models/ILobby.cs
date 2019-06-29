using System.Collections.Generic;
using LudoApi.Services;

namespace LudoApi.Models
{
    public interface ILobby
    {
        LobbyName Name { get; }

        IGameService Game { get; }

        IEnumerable<IPlayer> Players { get; }

        void AddPlayer(string connectionId, Color color);

        void RemovePlayer(string connectionId);
    }
}