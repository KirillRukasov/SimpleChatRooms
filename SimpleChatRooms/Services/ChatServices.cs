using SimpleChatRooms.Data;
using SimpleChatRooms.Interfaces;
using SimpleChatRooms.Models;
using Microsoft.EntityFrameworkCore;

namespace SimpleChatRooms.Services
{
    public class ChatService : IChatService
    {
        private readonly ISimpleChatRoomsDbContext _context;

        public ChatService(ISimpleChatRoomsDbContext context)
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

        public async Task JoinChatAsync(string chatName, int userId)
        {
            var chat = await _context.Chats.Include(c => c.ChatParticipations).FirstOrDefaultAsync(c => c.Name == chatName);
            if (chat == null)
                throw new InvalidOperationException("Chat does not exist.");

            if (chat.ChatParticipations.Any(p => p.UserId == userId))
                throw new InvalidOperationException("User is already a participant.");

            chat.ChatParticipations.Add(new ChatParticipant { ChatId = chat.ChatId, UserId = userId });
            await _context.SaveChangesAsync();
        }

        public async Task SendMessageAsync(string chatName, int userId, string messageContent)
        {
            if (string.IsNullOrWhiteSpace(messageContent))
                throw new ArgumentException("Message cannot be empty.");

            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Name == chatName);
            if (chat == null)
                throw new InvalidOperationException("Chat does not exist.");

            var message = new Message
            {
                Content = messageContent,
                UserId = userId,
                ChatId = chat.ChatId,
                Timestamp = DateTime.UtcNow
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }
    }

}
