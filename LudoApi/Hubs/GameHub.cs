using System;
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

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await LeaveLobby();
            
            await base.OnDisconnectedAsync(exception);
        }
        
        #region lobby
        
        [HubMethodName("lobby:create")]
        public async Task CreateLobby(string lobbyName)
        {
            var joinedLobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (joinedLobby != null)
            {
                throw new HubException("You can't create a lobby while you're in a lobby");
            }
            
            if (_lobbyService.GetLobby(lobbyName) != null)
            {
                throw new HubException("Lobby with that name already exists");
            }

            var lobby = _lobbyService.CreateLobby(Context.ConnectionId, lobbyName);
            await JoinLobby(lobbyName);

            var player = lobby.Game.GetPlayer(Context.ConnectionId);
            player.IsAdmin = true;
        }
        
        [HubMethodName("lobby:ready")]
        public async Task ReadyPlayer(bool ready)
        {
            var lobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (lobby == null)
            {
                throw new HubException("You are not in a lobby");
            }

            var player = lobby.Game.GetPlayer(Context.ConnectionId);
            player.IsReady = ready;

            await Clients.Group(lobby.Name.ToString()).SendAsync("lobby:player-ready", Context.ConnectionId);
        }

        [HubMethodName("lobby:join")]
        public async Task JoinLobby(string lobbyName)
        {
            var joinedLobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (joinedLobby != null)
            {
                throw new HubException("You can't join a lobby while you're in a different lobby");
            }
            
            var lobby = _lobbyService.GetLobby(lobbyName);
            if (lobby == null)
            {
                throw new HubException("Lobby does not exist");
            }

            var playerCount = lobby.Players.Count();
            if (playerCount >= 4)
            {
                throw new HubException("Lobby is full");
            }

            lobby.AddPlayer(Context.ConnectionId, (Color) (playerCount + 1));

            await Groups.AddToGroupAsync(Context.ConnectionId, lobby.Name.ToString(), Context.ConnectionAborted);
            await Clients.Group(lobby.Name.ToString()).SendAsync("lobby:player-join", Context.ConnectionId);
        }

        [HubMethodName("lobby:leave")]
        public async Task LeaveLobby()
        {
            var lobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (lobby == null)
            {
                return;
            }

            lobby.RemovePlayer(Context.ConnectionId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobby.Name.ToString());
            await Clients.Group(lobby.Name.ToString()).SendAsync("lobby:player-leave", Context.ConnectionId);

            if (!lobby.Players.Any())
            {
                _lobbyService.DestroyLobby(lobby.Name.ToString());
            }
        }
        
        [HubMethodName("lobby:get-players")]
        public async Task GetPlayers(string lobbyName)
        {
            var lobby = _lobbyService.GetLobby(lobbyName);
            if (lobby == null)
            {
                throw new HubException("Lobby does not exist");
            }

            await Clients.User(Context.ConnectionId).SendAsync("lobby:players", lobby.Players);
        }
        
        #endregion
        
        #region game
        
        [HubMethodName("game:start")]
        public async Task GameStart()
        {
            var lobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (lobby == null)
            {
                throw new HubException("You're not in a lobby");
            }

            var player = lobby.Game.GetPlayer(Context.ConnectionId);
            if (player == null || !player.IsAdmin)
            {
                throw new HubException("Only an admin can start the game");
            }

            if (lobby.Players.Any(p => !p.IsReady))
            {
                throw new HubException("Not every player is ready");
            }
            
            lobby.Game.StartGame(lobby.Players);
            
            await Clients.Group(lobby.Name.ToString()).SendAsync("game:started");
        }

        [HubMethodName("game:roll-die")]
        public async Task RollDie()
        {
            var lobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (lobby == null)
            {
                throw new HubException("You're not in a lobby");
            }

            var player = lobby.Game.GetPlayer(Context.ConnectionId);
            if (lobby.Game.GetTurn(player) != Turn.Roll)
            {
                throw new HubException("Not your turn");
            }

            var dieRoll = lobby.Game.RollDie(player);
            await Clients.Group(lobby.Name.ToString()).SendAsync("game:die-roll", player.ConnectionId, dieRoll);

            await NextTurn(lobby.Game, lobby.Name);
        }

        [HubMethodName("game:advance")]
        public async Task Advance(int piece)
        {
            var lobby = _lobbyService.GetJoinedLobby(Context.ConnectionId);
            if (lobby == null)
            {
                throw new HubException("You're not in a lobby");
            }

            var game = lobby.Game;
            var player = game.GetPlayer(Context.ConnectionId);
            if (game.GetTurn(player) != Turn.Advance)
            {
                throw new HubException("Not your turn to advance your piece");
            }

            game.Advance(player, piece);
            await Clients.Group(lobby.Name.ToString()).SendAsync("game:advanced", player.ConnectionId, piece);
            
            await NextTurn(game, lobby.Name);
        }

        private async Task NextTurn(IGameService game, LobbyName lobbyName)
        {
            var player = game.NextTurn();
            var turn = game.GetTurn(player);

            await Clients.Group(lobbyName.ToString()).SendAsync("game:next-turn", player.ConnectionId, turn.ToString());
        }
        
        #endregion
    }
}