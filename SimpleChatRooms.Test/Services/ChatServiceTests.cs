using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using SimpleChatRooms.Data;
using SimpleChatRooms.Interfaces;
using SimpleChatRooms.Models;
using SimpleChatRooms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatRooms.Test.Services
{
    public class ChatServiceTests
    {
        private readonly Mock<ISimpleChatRoomsDbContext> _mockContext;
        private readonly Mock<DbSet<Chat>> _mockSet;
        private readonly IChatService _service;

        public ChatServiceTests()
        {
            _mockContext = new Mock<ISimpleChatRoomsDbContext>();

            var chatList = new List<Chat>();
            _mockContext.Setup(x => x.Chats).ReturnsDbSet(chatList);

            _service = new ChatService(_mockContext.Object);
        }

        [Fact]
        public async Task GetChatByNameAsync_ReturnsChat_WhenNameExists()
        {
            var testChat = TestData.GetTestChat();
            _mockContext.Setup(x => x.Chats).ReturnsDbSet(new List<Chat> { testChat });

            var result = await _service.GetChatByNameAsync(testChat.Name);

            Assert.Equal(testChat, result);
        }

        [Fact]
        public async Task GetChatByNameAsync_ReturnsNull_WhenNameDoesNotExist()
        {
            var nonExistentChatName = "NonExistentName";

            var result = await _service.GetChatByNameAsync(nonExistentChatName);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateChatAsync_CreatesChatSuccessfully()
        {
            var chatName = "New Test Chat";
            var chatList = _mockContext.Object.Chats.ToList();
            chatList.Add(new Chat { Name = chatName });

            var result = await _service.CreateChatAsync(chatName, 1);

            Assert.NotNull(result);
            Assert.Equal(chatName, result.Name);
        }

        [Fact]
        public async Task CreateChatAsync_ThrowsException_WhenNameIsEmpty()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateChatAsync(string.Empty, 1));
        }

        [Fact]
        public async Task CreateChatAsync_ThrowsException_WhenNameIsTaken()
        {
            var testChat = TestData.GetTestChat();
            var chatList = new List<Chat> { testChat };

            _mockContext.Setup(x => x.Chats).ReturnsDbSet(chatList);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateChatAsync(testChat.Name, 1));
        }
    }
}
