using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoApi.Models;
using LudoApi.Services;
using Microsoft.AspNetCore.SignalR;

namespace LudoApi.Hubs
{
    public class GameHub : Hub
    {
        private readonly ILobbyService _lobbyService;

        public GameHub(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        public async Task RollDice()
        {
            var lobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (lobby == null) throw new HubException("You're not in a lobby");

            var game = lobby.Game;
            var player = game.GetPlayer(Context.ConnectionId);
            if (game.GetTurn(player) != Turn.Roll) throw new HubException("Not your turn");

            var diceRoll = game.RollDice(player);
            await Clients.Group(lobby.Name).SendAsync("dice-roll", player.ConnectionId, diceRoll);
            await NextTurn(game, lobby.Name);
        }

        public async Task Advance(int piece)
        {
            var lobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (lobby == null) throw new HubException("You're not in a lobby");

            var game = lobby.Game;
            var player = game.GetPlayer(Context.ConnectionId);
            if (game.GetTurn(player) != Turn.Advance) throw new HubException("Not your turn to advance your piece");

            game.Advance(player, piece);
            await Clients.Group(lobby.Name).SendAsync("advance", player.ConnectionId, piece);
            await NextTurn(game, lobby.Name);
        }

        private async Task NextTurn(IGameService game, string lobbyName)
        {
            var player = game.NextTurn();
            var turn = game.GetTurn(player).ToString();
            await Clients.Group(lobbyName).SendAsync("next-turn", player.ConnectionId, turn);
        }

        public async Task GameStart()
        {
            var lobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (lobby == null) throw new HubException("You're not in a lobby");

            var game = lobby.Game;
            game.StartGame(lobby.Players);
            await Clients.Group(lobby.Name).SendAsync("game-start");
        }

        public IEnumerable<ILobby> GetLobbies()
        {
            return _lobbyService.GetLobbies();
        }

        public ILobby? GetJoinedLobby()
        {
            return _lobbyService.GetJoinedLobby(Context.ConnectionId);
        }

        public async Task CreateLobby(string lobbyName)
        {
            if (_lobbyService.GetLobby(lobbyName) != null)
                throw new HubException("Lobby with that name already exists");

            var lobby = _lobbyService.CreateLobby(Context.ConnectionId, lobbyName);
            await Groups.AddToGroupAsync(Context.ConnectionId, lobby.Name, Context.ConnectionAborted);
        }


        public async Task JoinLobby(string lobbyName)
        {
            var foundLobby = _lobbyService.GetLobby(lobbyName);
            if (foundLobby == null) throw new HubException("Lobby does not exist");

            var playerCount = foundLobby.Players.Count();
            if (playerCount >= 4) throw new HubException("Lobby is full");

            foundLobby.AddPlayer(Context.ConnectionId, (Color) (playerCount + 1));

            await Groups.AddToGroupAsync(Context.ConnectionId, foundLobby.Name, Context.ConnectionAborted);
        }

        public async Task LeaveLobby(string lobbyName)
        {
            var foundLobby = _lobbyService.GetLobby(lobbyName);
            if (foundLobby == null) throw new HubException("Lobby does not exist");
            foundLobby.RemovePlayer(Context.ConnectionId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, foundLobby.Name);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}