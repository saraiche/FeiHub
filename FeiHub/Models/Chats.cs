using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FeiHub.Models.Chat;

namespace FeiHub.Models
{
    public class Chats
    {
        public string Id { get; set; }
        public Participant[] Participants { get; set; }
        public Message[] Messages { get; set; }
        public class Participant
        {
            public string Username { get; set; }
        }
        public class Message
        {
            public string Username { get; set; }
            public string MessageSent { get; set; }
            public DateTime DateOfMessage { get; set; }
        }
    }
}
