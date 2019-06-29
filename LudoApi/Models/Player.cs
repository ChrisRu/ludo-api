using System.Collections.Generic;

namespace LudoApi.Models
{
    public class Player : IPlayer
    {
        public Player(string connectionId, Color color)
        {
            ConnectionId = connectionId;
            Color = color;
        }
        
        public bool IsReady { get; set; }

        public string ConnectionId { get; }

        public Color Color { get; }

        public int PreviousDieRoll { get; set; } = -1;

        public IEnumerable<int> Pieces { get; set; } = new[] {-1, -1, -1, -1};

        public bool IsAdmin { get; set; }
    }
}