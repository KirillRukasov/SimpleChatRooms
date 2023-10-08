using SimpleChatRooms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChatRooms.Test
{
    public class TestData
    {
        public static Chat GetTestChat(int chatId = 1)
        {
            return new Chat
            {
                ChatId = chatId,
                Name = "Test Chat",
                CreatorUserId = 1,
                Messages = new List<Message>
                {
                    new Message
                    {
                        MessageId = 1,
                        Content = "Hello from user 1",
                        UserId = 1,
                        ChatId = chatId
                    },
                    new Message
                    {
                        MessageId = 2,
                        Content = "Hello from user 2",
                        UserId = 2,
                        ChatId = chatId
                    }
                }
            };
        }

        public static Message GetTestMessage(int messageId = 1)
        {
            return new Message
            {
                MessageId = messageId,
                Content = "Test Message",
                UserId = 1,
                ChatId = 1
            };
        }

        public static List<Chat> GetTestChats()
        {
            return new List<Chat>
            {
                GetTestChat(1),
                GetTestChat(2),
                GetTestChat(3)
            };
        }
    }
}
