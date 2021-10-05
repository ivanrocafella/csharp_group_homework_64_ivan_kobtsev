using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.Models
{
    public class Account : IdentityUser
    {
        public DateTime BirthDate { get; set; }
        public string Avatar { get; set; }
        public int CountMessage { get; set; }

        public List<Message> Messages { get; set; }
        public Account()
        {
            Messages = new List<Message>();
        }

    }
}
