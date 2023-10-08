using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleChatRooms.Interfaces;
using SimpleChatRooms.Models;

namespace SimpleChatRooms.Data
{
    public class SimpleChatRoomsDbContext : DbContext, ISimpleChatRoomsDbContext
    {
        public SimpleChatRoomsDbContext(DbContextOptions<SimpleChatRoomsDbContext> options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatParticipant> ChatParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .HasKey(c => c.ChatId);

            modelBuilder.Entity<Message>()
                .HasKey(m => m.MessageId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId);

            modelBuilder.Entity<ChatParticipant>()
            .HasKey(cp => new { cp.ChatId, cp.UserId });

            modelBuilder.Entity<ChatParticipant>()
            .HasOne(cp => cp.Chat)
            .WithMany(c => c.ChatParticipations)
            .HasForeignKey(cp => cp.ChatId);
        }
    }
}