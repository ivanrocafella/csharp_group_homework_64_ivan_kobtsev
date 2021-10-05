using csharp_group_homework_64_ivan_kobtsev.Models;
using csharp_group_homework_64_ivan_kobtsev.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private ApplicationContext _context;

        public ChatController(UserManager<Account> userManager, SignInManager<Account> signInManager, ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Message> messages = await _context.Messages.Include(e => e.Account).ToListAsync();
            if (messages.Count < 30)
                return View(messages);
            else
            {
                return View(messages.SkipWhile(e => e.Id < (messages.Count - 30)).ToList());
            }
                
                
        }

        [HttpPost]
        public async Task<JsonResult> SendMessage(string message_text, string user_name)
        {
            Account account = await _context.Accounts.FirstOrDefaultAsync(e => e.UserName == user_name);
            Message message = new Message
            {
                TextOfMessage = message_text,
                AccountId = account.Id
            };
            account.CountMessage++;
            await _context.Messages.AddAsync(message);
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();

            AccountMessageViewModel accountMessageView = new AccountMessageViewModel
            {
                UserName = account.UserName,
                Avatar = account.Avatar,
                DateCreate = message.DateCreate.ToString(),
                TextOfMessage = message.TextOfMessage
            };
            List<Message> messages = await _context.Messages.ToListAsync();
            int CountMessage = messages.Count;

            return Json(new { accountMessageView, CountMessage });
        }
    }
}
