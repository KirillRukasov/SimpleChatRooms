using SimpleChatRooms.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatRooms.Interfaces
{
    public interface ISimpleChatRoomsDbContext
    {
        DbSet<Chat> Chats { get; set; }
        DbSet<ChatParticipant> ChatParticipants { get; set; }
        DbSet<Message> Messages { get; set; }
    }
}
