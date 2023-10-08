using Microsoft.AspNetCore.Mvc;
using SimpleChatRooms.Interfaces;
using SimpleChatRooms.Models;

namespace SimpleChatRooms.Controllers
{
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<ActionResult<Chat>> GetChatByName()
        {

        }

        public async Task<ActionResult<Chat>> CreateChat()
        {

        }

        public async Task<ActionResult> SendMessage()
        {

        }
    }
}
