using System.Collections.Generic;

namespace LudoApi.Models
{
    public interface IPlayer
    {
        string ConnectionId { get; }

        Color Color { get; }

        int PreviousDieRoll { get; set; }

        bool IsAdmin { get; }

        IEnumerable<int> Pieces { get; set; }
    }
}