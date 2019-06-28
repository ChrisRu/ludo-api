using System.Collections.Generic;
using LudoApi.Models;

namespace LudoApi.Services
{
    public interface IGameService
    {
        void StartGame(IEnumerable<IPlayer> players);

        int RollDice(IPlayer player);
        
        IPlayer NextTurn();
        
        IPlayer GetPlayer(string connectionId);

        Turn GetTurn(IPlayer player);

        void Advance(IPlayer player, int piece);

        bool HasWon(IPlayer player);
    }
}