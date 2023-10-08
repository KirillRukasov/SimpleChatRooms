using SimpleChatRooms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatRooms.Interfaces
{
    public interface IChatService
    {
        Task<Chat> GetChatByNameAsync(string chatName);
        Task<Chat> CreateChatAsync(string chatName, int userId);
        Task JoinChatAsync(string chatName, int userId);
        Task SendMessageAsync(string chatName, int userId, string messageContent);
    }
}
