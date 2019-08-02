using System.Collections.Generic;

namespace LudoApi.Models
{
    public interface IPlayer
    {
        string ConnectionId { get; }

        Color Color { get; }

        int PreviousDieRoll { get; set; }
        
        bool IsReady { get; set; }

        IEnumerable<int> Pieces { get; set; }
    }
}