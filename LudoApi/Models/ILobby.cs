using System.Collections.Generic;
using LudoApi.Services;

namespace LudoApi.Models
{
    public interface ILobby
    {
        int Id { get; }

        string Name { get; }

        IGameService Game { get; }

        IEnumerable<IPlayer> Players { get; }

        void AddPlayer(string connectionId, Color color);

        void RemovePlayer(string connectionId);
    }
}