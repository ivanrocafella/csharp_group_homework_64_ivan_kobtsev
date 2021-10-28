using csharp_group_homework_64_ivan_kobtsev.Models;
using csharp_group_homework_64_ivan_kobtsev.services;
using csharp_group_homework_64_ivan_kobtsev.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace csharp_group_homework_64_ivan_kobtsev.Controllers
{
    public class AccountController : Controller
    {
        private readonly EmailService _Eservice;
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private IWebHostEnvironment _appEnvironment;
        private ApplicationContext _context;

        public AccountController(UserManager<Account> userManager, SignInManager<Account> signInManager,
          IWebHostEnvironment appEnvironment, ApplicationContext context, EmailService Eservice)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
            _context = context;
            _Eservice = Eservice;
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
                    pathImage = "Default.png";

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
                    _Eservice.SendEmail(model.Email, "Регистрация", "Вы успешно прошли регистрацию");
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id != null)
            {
                Account account = await _context.Accounts.FirstOrDefaultAsync(b => b.Id == id);
              
                EditViewModel editViewModel = new EditViewModel
                {
                    Id = account.Id,
                    UserName = account.UserName,
                    Email = account.Email,
                    DateBirth = account.BirthDate,
                    Password = account.PasswordHash,
                    PasswordConfirm = account.PasswordHash,
                    Avatar = account.Avatar
                };
                if (editViewModel != null)
                {
                    return View(editViewModel);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel editViewModel, IFormFile uploadImage)
        {
            if (ModelState.IsValid)
            {
                Account account = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == editViewModel.Id);
                if (editViewModel != null)
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
                        pathImage = editViewModel.Avatar;

                    string message = String.Empty;
                    string pathImgChange = String.Empty;
                    string userNameChange = String.Empty;
                    string emailChange = String.Empty;
                    string birthDateChange = String.Empty;

                    if (account.Avatar != pathImage)
                        pathImgChange = "Изменён аватар профиля";
                    if (account.UserName != editViewModel.UserName)
                        userNameChange = $"Новое имя пользователя: {editViewModel.UserName}";
                    if (account.Email != editViewModel.Email)
                        emailChange = $"Новая электронная почта: {editViewModel.Email}";
                    if (account.BirthDate != editViewModel.DateBirth)
                        birthDateChange = $"Новая дата рождения: {editViewModel.DateBirth}";

                    account.UserName = editViewModel.UserName;
                    account.Email = editViewModel.Email;
                    account.BirthDate = editViewModel.DateBirth;
                    account.PasswordHash = editViewModel.Password;
                    account.Avatar = pathImage;

                    message = $"Изменения в профиле:\n" +
                        $"{pathImgChange}\n{userNameChange}\n" +
                        $"{emailChange}\n{birthDateChange}";

                    _Eservice.SendEmail(editViewModel.Email, "Редактироваие профиля", message);

                    _context.Accounts.Update(account);
                    _context.SaveChanges();
                }
                return RedirectToAction("PrivateCabinet", new { name = account.UserName });
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }


        [HttpGet]
        public async Task<IActionResult> SendUser(string id)
        {
            Account CurAccount = await _userManager.GetUserAsync(HttpContext.User);
            if (id != null)
            {
                Account account = await _context.Accounts
               .FirstOrDefaultAsync(e => e.Id == id);
                string message = $"Данные о пользователе:\n" +
                       $"Логин: {account.UserName}\n Электронная почта: {account.Email}\n" +
                       $"Дата рождения: {account.BirthDate.ToString("d")}\n Кол-во сообщений: {account.CountMessage}";
                _Eservice.SendEmail(CurAccount.Email, "Запрос данных о пользователе", message);
                return RedirectToAction("Accounts", "Home");
            }
            return NotFound();
           
        }
    }
}
