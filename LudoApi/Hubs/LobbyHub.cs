using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LudoApi.Hubs
{
    public class LobbyHub : Hub
    {
        private List<string> LobbyNames { get; } = new List<string>();

        private string CreateLobby(string groupName)
        {
            if (!LobbyNames.Contains(groupName))
            {
                LobbyNames.Add(groupName);
            }

            return groupName;
        }

        private void DestroyLobby(string groupName)
        {
            if (LobbyNames.Contains(groupName))
            {
                LobbyNames.Remove(groupName);
            }
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

        public List<string> GetGroups()
        {
            return LobbyNames;
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, CreateLobby(roomName));
            
            
            await Clients
                .Groups(roomName)
                .SendAsync("Join", Context.ConnectionId);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, CreateLobby(roomName));
        }

        public Task EndRoom(string roomName)
        {
            DestroyLobby(roomName);
        }

        public Task StartGame(string roomName)
        {
            Clients.Groups(roomName);
        }
    }
}