using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatRooms.Models
{
    public class ChatCreationRequest
    {
        public string ChatName { get; set; }
        public int UserId { get; set; }
    }
}
