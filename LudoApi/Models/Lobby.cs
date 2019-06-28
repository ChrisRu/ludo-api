using System.Collections.Generic;
using System.Linq;
using LudoApi.Services;

namespace LudoApi.Models
{
    public class Lobby : ILobby
    {
        public string Name { get; }
        
        public IGameService Game { get; }

        public IEnumerable<IPlayer> Players { get; private set; }

        public Lobby(string name, IGameService gameService, IPlayer creator)
        {
            Name = name;
            Game = gameService;
            Players = new List<IPlayer> { creator };
        }

        public void AddPlayer(string connectionId, Color color)
        {
            Players = Players.Append(new Player(connectionId, color));
        }

        public void RemovePlayer(string connectionId)
        {
            Players = Players.Where(player => player.ConnectionId != connectionId);
        }
    }
}