using System;
using System.Collections.Generic;
using System.Linq;
using LudoApi.Models;
using LudoApi.Services;
using Xunit;

namespace LudoApiTest
{
    public class GameServiceTest
    {
        [Fact]
        public void TestAdvancePlayer()
        {
            var players = new List<IPlayer>
            {
                new Player("1", Color.Red, true),
                new Player("2", Color.Blue),
                new Player("3", Color.Yellow),
                new Player("4", Color.Green)
            };
            var gameService = new GameService();
            gameService.StartGame(players);

            const int piece = 0;
            const int playerIndex = 0;

            var rolled = gameService.RollDie(players.ElementAt(playerIndex));
            var position = players.ElementAt(playerIndex).Pieces.ElementAt(piece);

            Assert.Equal(-1, position);

            gameService.Advance(players.ElementAt(playerIndex), piece);

            Assert.Equal(rolled, players.ElementAt(playerIndex).Pieces.ElementAt(piece));
        }

        [Fact]
        public void TestAdvancePlayerSecond()
        {
            var players = new List<IPlayer>
            {
                new Player("1", Color.Red, true),
                new Player("2", Color.Blue),
                new Player("3", Color.Yellow),
                new Player("4", Color.Green)
            };
            var gameService = new GameService();
            gameService.StartGame(players);

            const int piece = 1;
            const int playerIndex = 2;

            var rolled = gameService.RollDie(players.ElementAt(playerIndex));
            var position = players.ElementAt(playerIndex).Pieces.ElementAt(piece);

            Assert.Equal(-1, position);

            gameService.Advance(players.ElementAt(playerIndex), piece);

            Assert.Equal(rolled + ColorPositions.StartPosition(players.ElementAt(playerIndex).Color),
                players.ElementAt(playerIndex).Pieces.ElementAt(piece));
        }

        [Fact]
        public void TestGetPlayer()
        {
            var players = new List<IPlayer>
            {
                new Player("1", Color.Red, true),
                new Player("2", Color.Blue),
                new Player("3", Color.Yellow),
                new Player("4", Color.Green)
            };
            var gameService = new GameService();
            gameService.StartGame(players);

            var player = gameService.GetPlayer("1");

            Assert.Equal(players.ElementAt(0), player);
        }

        [Fact]
        public void TestGetPlayerNonExisting()
        {
            var players = new List<IPlayer>
            {
                new Player("1", Color.Red, true),
                new Player("2", Color.Blue),
                new Player("3", Color.Yellow),
                new Player("4", Color.Green)
            };
            var gameService = new GameService();
            gameService.StartGame(players);

            Assert.Throws<InvalidOperationException>(() => gameService.GetPlayer("non existent"));
        }

        [Fact]
        public void TestHasNotWon()
        {
            var players = new List<IPlayer>
            {
                new Player("1", Color.Red, true),
                new Player("2", Color.Blue),
                new Player("3", Color.Yellow),
                new Player("4", Color.Green)
            };
            var gameService = new GameService();
            gameService.StartGame(players);

            var won = gameService.HasWon(players.ElementAt(0));

            Assert.False(won);
        }

        [Fact]
        public void TestHasWon()
        {
            var player = new Player("1", Color.Red, true)
            {
                Pieces = new[] {40, 41, 42, 43, 44}
            };

            var won = new GameService().HasWon(player);

            Assert.True(won);
        }

        [Fact]
        public void TestHasWonBlue()
        {
            var player = new Player("1", Color.Blue, true)
            {
                Pieces = new[] {10, 11, 12, 13}
            };

            var won = new GameService().HasWon(player);

            Assert.True(won);
        }

        [Fact]
        public void TestNextTurn()
        {
            var players = new List<IPlayer>
            {
                new Player("1", Color.Red, true),
                new Player("2", Color.Blue),
                new Player("3", Color.Yellow),
                new Player("4", Color.Green)
            };
            var gameService = new GameService();
            gameService.StartGame(players);

            Assert.Equal(Turn.Roll, gameService.GetTurn(players.ElementAt(0)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(1)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(2)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(3)));

            gameService.NextTurn();

            Assert.Equal(Turn.Advance, gameService.GetTurn(players.ElementAt(0)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(1)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(2)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(3)));

            gameService.NextTurn();

            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(0)));
            Assert.Equal(Turn.Roll, gameService.GetTurn(players.ElementAt(1)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(2)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(3)));

            gameService.NextTurn();

            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(0)));
            Assert.Equal(Turn.Advance, gameService.GetTurn(players.ElementAt(1)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(2)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(3)));
            gameService.NextTurn();

            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(0)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(1)));
            Assert.Equal(Turn.Roll, gameService.GetTurn(players.ElementAt(2)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(3)));

            gameService.NextTurn();

            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(0)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(1)));
            Assert.Equal(Turn.Advance, gameService.GetTurn(players.ElementAt(2)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(3)));

            gameService.NextTurn();

            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(0)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(1)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(2)));
            Assert.Equal(Turn.Roll, gameService.GetTurn(players.ElementAt(3)));

            gameService.NextTurn();

            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(0)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(1)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(2)));
            Assert.Equal(Turn.Advance, gameService.GetTurn(players.ElementAt(3)));

            gameService.NextTurn();

            Assert.Equal(Turn.Roll, gameService.GetTurn(players.ElementAt(0)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(1)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(2)));
            Assert.Equal(Turn.None, gameService.GetTurn(players.ElementAt(3)));
        }

        [Fact]
        public void TestNotStartedGameTurn()
        {
            var players = new List<IPlayer>
            {
                new Player("1", Color.Red, true),
                new Player("2", Color.Blue),
                new Player("3", Color.Yellow),
                new Player("4", Color.Green)
            };
            var gameService = new GameService();

            var turn = gameService.GetTurn(players.ElementAt(0));

            Assert.Equal(Turn.None, turn);
        }

        [Fact]
        public void TestStartGameTurn()
        {
            var players = new List<IPlayer>
            {
                new Player("1", Color.Red, true),
                new Player("2", Color.Blue),
                new Player("3", Color.Yellow),
                new Player("4", Color.Green)
            };
            var gameService = new GameService();
            gameService.StartGame(players);

            var turn = gameService.GetTurn(players.ElementAt(0));

            Assert.Equal(Turn.Roll, turn);
        }
    }
}