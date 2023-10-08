using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatRooms.Models
{
    public class ChatParticipant
    {
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public int UserId { get; set; }
    }
}
