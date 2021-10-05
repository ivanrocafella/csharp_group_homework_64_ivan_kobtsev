using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string TextOfMessage { get; set; }
        public DateTime DateCreate { get; set; }

        public string AccountId { get; set; }
        public Account Account { get; set; }

        public Message()
        {
            DateCreate = DateTime.Now;
        }
    }
}
