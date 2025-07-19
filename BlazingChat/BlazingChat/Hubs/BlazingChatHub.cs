using BlazingChat.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BlazingChat.Server.Hubs
{
    [Authorize]
    public class BlazingChatHub : Hub<IBlazingChatHubClient>, IBlazingChatHubServer
    {
        private static readonly Dictionary<int, UserDto> _onlineUsers = new();
        private static readonly Dictionary<string, int> _connectionToUserId = new();

        public override async Task OnConnectedAsync()
        {
            // When a new client connects, send them the current online users list
            await Clients.Caller.OnlineUsersList(_onlineUsers.Values);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_connectionToUserId.TryGetValue(Context.ConnectionId, out int userId))
            {
                _connectionToUserId.Remove(Context.ConnectionId);

                // Check if this was the last connection for this user
                if (!_connectionToUserId.ContainsValue(userId))
                {
                    _onlineUsers.Remove(userId);
                    await Clients.All.OnlineUsersList(_onlineUsers.Values);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SetUserOnline(UserDto user)
        {
            _connectionToUserId[Context.ConnectionId] = user.Id;

            // Only add to _onlineUsers if not already tracked
            if (!_onlineUsers.ContainsKey(user.Id))
            {
                _onlineUsers.Add(user.Id, user);

                // Notify others that this user is online
                await Clients.Others.UserIsOnline(user.Id);
            }

            // Send updated list to caller
            await Clients.Caller.OnlineUsersList(_onlineUsers.Values);
        }
    }
}
