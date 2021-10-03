using csharp_group_homework_64_ivan_kobtsev.Models;
using csharp_group_homework_64_ivan_kobtsev.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private IWebHostEnvironment _appEnvironment;
        private ApplicationContext _context;

        public AccountController(UserManager<Account> userManager, SignInManager<Account> signInManager,
          IWebHostEnvironment appEnvironment, ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile uploadImage)
        {
            if (ModelState.IsValid)
            {
                string pathImage;
                if (uploadImage != null)
                {
                    pathImage = "/files/" + uploadImage.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + pathImage, FileMode.Create))
                    {
                        await uploadImage.CopyToAsync(fileStream);
                    }
                }
                else
                    pathImage = "/files/default.png";

                Account account = new Account
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Avatar = pathImage,
                    BirthDate = model.DateBirth
                };
                var result = await _userManager.CreateAsync(account, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(account, "user");
                    await _signInManager.SignInAsync(account, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) =>
                             View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account accountByName = await _userManager.FindByNameAsync(model.EmailLogin);
                Account accountByEmail = await _userManager.FindByEmailAsync(model.EmailLogin);
                Account ByEmailOrName;
                if (accountByName != null || accountByEmail != null)
                {
                    if (accountByName != null)
                        ByEmailOrName = accountByName;
                    else
                        ByEmailOrName = accountByEmail;

                    var result = await _signInManager.PasswordSignInAsync(
                    ByEmailOrName,
                    model.Password,
                    model.RememberMe,
                    false);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Неправильный логин, электронная почта и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult PrivateCabinet(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Account account = _context.Accounts.FirstOrDefault(e => e.UserName == name);
                if (account != null)
                    return View(account);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
                return NotFound();
        }
    }
}
