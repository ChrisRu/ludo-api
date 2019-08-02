using System;
using System.Collections.Generic;
using System.Linq;
using LudoApi.Models;

namespace LudoApi.Services
{
    public class GameService : IGameService
    {
        private static readonly Random Random = new Random();

        private IEnumerable<IPlayer> _players = new List<IPlayer>();

        private int _playerTurn;

        private Turn _playerTurnAction;

        public void StartGame(IEnumerable<IPlayer> players)
        {
            _players = players;
            _playerTurnAction = Turn.Roll;
        }

        public int RollDie(IPlayer player)
        {
            return player.PreviousDieRoll = Random.Next(1, 6);
        }

        public void Advance(IPlayer player, int pieceIndex)
        {
            player.Pieces = player.Pieces
                .Select((pieceLocation, index) =>
                {
                    if (index == pieceIndex)
                    {
                        if (pieceLocation == -1)
                        {
                            pieceLocation = ColorPositions.StartPosition(player.Color);
                        }

                        var nextLocation = pieceLocation + player.PreviousDieRoll;
                        if (!ColorPositions.OutsideWinningPosition(player.Color, nextLocation))
                        {
                            return nextLocation;
                        }
                    }

                    return pieceLocation;
                })
                .ToArray();
        }

        public bool HasWon(IPlayer player)
        {
            return player.Pieces.Intersect(ColorPositions.WinPositions(player.Color)).Count() == 4;
        }

        public IPlayer NextTurn()
        {
            switch (_playerTurnAction)
            {
                case Turn.None:
                    _playerTurnAction = Turn.Roll;
                    break;
                case Turn.Roll:
                    _playerTurnAction = Turn.Advance;
                    break;
                case Turn.Advance:
                    _playerTurn = (_playerTurn + 1) % _players.Count();
                    _playerTurnAction = Turn.Roll;
                    break;
                default:
                    throw new Exception("Unknown turn");
            }

            return _players.ElementAt(_playerTurn);
        }

        public IPlayer? GetPlayer(string connectionId)
        {
            return _players.FirstOrDefault(player => player.ConnectionId == connectionId);
        }

        public Turn GetTurn(IPlayer player)
        {
            var index = _players.ToList().IndexOf(player);
            return index == _playerTurn ? _playerTurnAction : Turn.None;
        }
    }
}