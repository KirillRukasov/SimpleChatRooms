using Microsoft.AspNetCore.SignalR;

namespace SimpleChatRooms.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChatGroup(string chatName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
        }

        public async Task LeaveChatGroup(string chatName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);
        }

        public async Task SendGroupMessage(string chatName, string message)
        {
            await Clients.Group(chatName).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }
    }
}
