using SimpleChatRooms.Data;
using SimpleChatRooms.Interfaces;
using SimpleChatRooms.Models;
using Microsoft.EntityFrameworkCore;

namespace SimpleChatRooms.Services
{
    public class ChatService : IChatService
    {
        private readonly SimpleChatRoomsDbContext _context;

        public ChatService(SimpleChatRoomsDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> GetChatByNameAsync(string chatName)
        {
            if (string.IsNullOrWhiteSpace(chatName))
                throw new ArgumentException("Chat name cannot be empty.", nameof(chatName));

            return await _context.Chats.FirstOrDefaultAsync(c => c.Name == chatName);
        }

        public async Task<Chat> CreateChatAsync(string chatName, int userId)
        {
            if (string.IsNullOrWhiteSpace(chatName))
                throw new ArgumentException("Chat name cannot be empty.", nameof(chatName));

            var existingChat = await _context.Chats.FirstOrDefaultAsync(c => c.Name == chatName);
            if (existingChat != null)
                throw new InvalidOperationException("Chat with this name already exists.");

            var newChat = new Chat
            {
                Name = chatName,
                CreatorUserId = userId
            };

            await _context.Chats.AddAsync(newChat);
            await _context.SaveChangesAsync();

            return newChat;
        }
    }

}
