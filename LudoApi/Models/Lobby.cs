using System.Collections.Generic;
using System.Linq;
using LudoApi.Services;

namespace LudoApi.Models
{
    public class Lobby : ILobby
    {
        public Lobby(int id, string name, string admin, IGameService gameService)
        {
            Id = id;
            Admin = admin;
            Name = name;
            Game = gameService;
        }

        public int Id { get; }

        public string Name { get; }

        public string Admin { get; }

        public IGameService Game { get; }

        public IEnumerable<IPlayer> Players { get; private set; } = new List<IPlayer>();

        public void AddPlayer(string connectionId, Color color)
        {
            Players = Players.Append(new Player(connectionId, color)).ToList();
        }

        public void RemovePlayer(string connectionId)
        {
            Players = Players.Where(player => player.ConnectionId != connectionId).ToList();
        }
    }
}