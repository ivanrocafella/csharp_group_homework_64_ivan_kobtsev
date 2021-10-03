using csharp_group_homework_64_ivan_kobtsev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<Account> userManager,
            SignInManager<Account> signInManager,
            ApplicationContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Accounts()
        {
            Account CurAccount = await _userManager.GetUserAsync(HttpContext.User);
            List<Account> accounts = await _context.Accounts.ToListAsync();
            accounts.Remove(CurAccount);
            return View(accounts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
