using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatRooms.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public string Name { get; set; }
        public int CreatorUserId { get; set; }
        public List<Message> Messages { get; set; }
        public virtual ICollection<ChatParticipant> ChatParticipations { get; set; } = new List<ChatParticipant>();
    }
}
