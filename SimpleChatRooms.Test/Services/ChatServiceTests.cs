using Microsoft.EntityFrameworkCore;
using Moq;
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
        private readonly Mock<SimpleChatRoomsDbContext> _mockContext;
        private readonly Mock<DbSet<Chat>> _mockSet;
        private readonly IChatService _service;

        public ChatServiceTests()
        {
            _mockContext = new Mock<SimpleChatRoomsDbContext>();
            _mockSet = new Mock<DbSet<Chat>>();

            _mockSet.As<IQueryable<Chat>>().Setup(m => m.Provider).Returns(new List<Chat>().AsQueryable().Provider);
            _mockSet.As<IQueryable<Chat>>().Setup(m => m.Expression).Returns(new List<Chat>().AsQueryable().Expression);
            _mockSet.As<IQueryable<Chat>>().Setup(m => m.ElementType).Returns(new List<Chat>().AsQueryable().ElementType);
            _mockSet.As<IQueryable<Chat>>().Setup(m => m.GetEnumerator()).Returns(new List<Chat>().GetEnumerator());

            _mockContext.Setup(c => c.Chats).Returns(_mockSet.Object);

            _service = new ChatService(_mockContext.Object);
        }

        [Fact]
        public async Task GetChatByNameAsync_ReturnsChat_WhenNameExists()
        {
            var testChat = TestData.GetTestChat();
            _mockSet.As<IQueryable<Chat>>().Setup(m => m.Provider).Returns(new List<Chat> { testChat }.AsQueryable().Provider);
            _mockSet.As<IQueryable<Chat>>().Setup(m => m.GetEnumerator()).Returns(new List<Chat> { testChat }.GetEnumerator());
            
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
            _mockSet.Setup(m => m.Add(It.IsAny<Chat>())).Callback<Chat>(chat => new List<Chat> { chat }.AsQueryable());

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

            _mockSet.As<IQueryable<Chat>>().Setup(m => m.Provider).Returns(new List<Chat> { testChat }.AsQueryable().Provider);
            _mockSet.As<IQueryable<Chat>>().Setup(m => m.GetEnumerator()).Returns(new List<Chat> { testChat }.GetEnumerator());

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateChatAsync(testChat.Name, 1));
        }
    }
}
