using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleChatRooms.Controllers;
using SimpleChatRooms.Interfaces;
using SimpleChatRooms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatRooms.Test.Controllers
{
    public class ChatsControllerTests
    {
        private readonly Mock<IChatService> _mockChatService;
        private readonly ChatsController _controller;

        public ChatsControllerTests()
        {
            _mockChatService = new Mock<IChatService>();
            _controller = new ChatsController(_mockChatService.Object);
        }

        [Fact]
        public async Task Post_CreatesChatSuccessfully_ReturnsCreatedChat()
        {
            var testChat = TestData.GetTestChat();
            var chatCreationRequest = new ChatCreationRequest
            {
                ChatName = testChat.Name,
                UserId = testChat.CreatorUserId
            };

            _mockChatService.Setup(service => service.CreateChatAsync(testChat.Name, It.IsAny<int>()))
                            .ReturnsAsync(testChat);

            var result = await _controller.CreateChat(chatCreationRequest);

            var actionResult = Assert.IsType<ActionResult<Chat>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Chat>(createdAtActionResult.Value);
            Assert.Equal(testChat.Name, returnValue.Name);
        }

        [Fact]
        public async Task Post_WithExistingChatName_ReturnsBadRequest()
        {
            var testChat = TestData.GetTestChat();
            var chatCreationRequest = new ChatCreationRequest
            {
                ChatName = testChat.Name,
                UserId = testChat.CreatorUserId
            };

            _mockChatService.Setup(service => service.CreateChatAsync(testChat.Name, It.IsAny<int>()))
                            .ThrowsAsync(new InvalidOperationException("Chat with this name already exists."));

            var result = await _controller.CreateChat(chatCreationRequest);

            var actionResult = Assert.IsType<ActionResult<Chat>>(result);

            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetChatByName_WithNonExistingName_ReturnsNotFound()
        {
            _mockChatService.Setup(service => service.GetChatByNameAsync(It.IsAny<string>()))
                            .ReturnsAsync((Chat)null);

            var result = await _controller.GetChatByName("NonExistingChat");

            var actionResult = Assert.IsType<ActionResult<Chat>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task JoinChat_JoinsSuccessfully_ReturnsOk()
        {
            var chatName = "TestChat";
            var userId = 1;

            _mockChatService.Setup(service => service.JoinChatAsync(chatName, userId))
                            .Returns(Task.CompletedTask);  

            var result = await _controller.JoinChat(chatName, userId);

            var actionResult = Assert.IsType<ActionResult<Chat>>(result);
            var okResult = Assert.IsType<OkResult>(actionResult.Result);
        }

        [Fact]
        public async Task JoinChat_NonExistingChat_ReturnsNotFound()
        {
            _mockChatService.Setup(service => service.JoinChatAsync(It.IsAny<string>(), It.IsAny<int>()))
                            .ThrowsAsync(new InvalidOperationException("Chat does not exist."));

            var result = await _controller.JoinChat("NonExistingChat", 1);

            var actionResult = Assert.IsType<ActionResult<Chat>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task SendMessage_SendsSuccessfully_ReturnsOk()
        {
            var chatName = "TestChat";
            var userId = 1;
            var messageContent = "Hello, Chat!";

            _mockChatService.Setup(service => service.SendMessageAsync(chatName, userId, messageContent))
                            .Returns(Task.CompletedTask);

            var result = await _controller.SendMessage(chatName, userId, messageContent);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SendMessage_EmptyMessage_ReturnsBadRequest()
        {
            _mockChatService.Setup(service => service.SendMessageAsync(It.IsAny<string>(), It.IsAny<int>(), string.Empty))
                            .ThrowsAsync(new ArgumentException("Message cannot be empty."));

            var result = await _controller.SendMessage("TestChat", 1, string.Empty);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SendMessage_NonExistingChat_ReturnsNotFound()
        {
            _mockChatService.Setup(service => service.SendMessageAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                            .ThrowsAsync(new InvalidOperationException("Chat does not exist."));

            var result = await _controller.SendMessage("NonExistingChat", 1, "Hello!");

            Assert.IsType<NotFoundObjectResult>(result);
        }

    }
}
