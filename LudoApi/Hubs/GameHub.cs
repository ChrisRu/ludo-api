using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LudoApi.Hubs
{
    public class GameHub : Hub
    {
        public async Task RollDice()
        {
        }

        public async Task Advance(string piece)
        {
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