using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.ViewModels
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = "Логин")]
        [Remote(action: "CheckExistAccountForEdit", controller: "Validation", ErrorMessage = "Такой пользователь уже есть")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Укажите email")]
        [Display(Name = "Email")]
        [Remote(action: "CheckExistAccountForEdit", controller: "Validation", ErrorMessage = "Такой пользователь уже есть")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите дату рождения")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [Remote(action: "CheckAge", controller: "Validation", ErrorMessage = "Вы же старше 18")]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        public string Avatar { get; set; }
        public string Id { get; set; }
    }
}
