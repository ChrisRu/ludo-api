using System.Collections.Generic;

namespace LudoApi.Models
{
    public class Player : IPlayer
    {
        public Player(string connectionId, Color color, bool isAdmin = false)
        {
            ConnectionId = connectionId;
            Color = color;
            IsAdmin = isAdmin;
        }

        public string ConnectionId { get; }

        public Color Color { get; }

        public int PreviousDieRoll { get; set; } = -1;

        public IEnumerable<int> Pieces { get; set; } = new[] {-1, -1, -1, -1};

        public bool IsAdmin { get; }
    }
}