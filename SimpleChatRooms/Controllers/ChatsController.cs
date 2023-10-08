using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using SimpleChatRooms.Interfaces;
using SimpleChatRooms.Models;

namespace SimpleChatRooms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("{chatName}")]
        public async Task<ActionResult<Chat>> GetChatByName(string chatName)
        {
            try
            {
                var chat = await _chatService.GetChatByNameAsync(chatName);

                if (chat == null)
                    return NotFound($"Chat with name {chatName} does not exist.");

                return chat;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{chatName}/join")]
        public async Task<ActionResult<Chat>> JoinChat(string chatName, int userId)
        {
            try
            {
                await _chatService.JoinChatAsync(chatName, userId);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound("Chat does not exist.");
            }
        }

        [HttpPost("{chatName}/send")]
        public async Task<ActionResult> SendMessage(string chatName, int userId, string messageContent)
        {
            try
            {
                await _chatService.SendMessageAsync(chatName, userId, messageContent);
                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest("Message cannot be empty.");
            }
            catch (InvalidOperationException)
            {
                return NotFound("Chat does not exist.");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Chat>> CreateChat(ChatCreationRequest request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            try
            {
                var chat = await _chatService.CreateChatAsync(request.ChatName, request.UserId);
                return CreatedAtAction(nameof(GetChatByName), new { chatName = chat.Name }, chat);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
