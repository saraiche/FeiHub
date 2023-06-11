using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiHub.Models
{
    public class Chat
    {

        public class Rootobject
        {
            public _Id _id { get; set; }
            public Participant[] participants { get; set; }
            public Message[] messages { get; set; }
            public int __v { get; set; }
        }

        public class _Id
        {
            public string oid { get; set; }
        }

        public class Participant
        {
            public string username { get; set; }
            public _Id1 _id { get; set; }
        }

        public class _Id1
        {
            public string oid { get; set; }
        }

        public class Message
        {
            public string username { get; set; }
            public string message { get; set; }
            public Dateofmessage dateOfMessage { get; set; }
            public _Id2 _id { get; set; }
        }

        public class Dateofmessage
        {
            public DateTime date { get; set; }
        }

        public class _Id2
        {
            public string oid { get; set; }
        }

    }
}
