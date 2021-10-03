using csharp_group_homework_64_ivan_kobtsev.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.Controllers
{
    public class ValidationController : Controller
    {
        private ApplicationContext _context;

        public ValidationController(ApplicationContext context)
        {
            _context = context;
        }

        [AcceptVerbs("GET", "POST")]
        public bool CheckExistAccount(string UserName, string Email) => !_context.Accounts.Any(e => e.UserName == UserName || e.Email == Email);

        [AcceptVerbs("GET", "POST")]
        public bool CheckAge(DateTime DateBirth) => !(DateBirth.AddYears(18) > DateTime.Now);
    }
}
