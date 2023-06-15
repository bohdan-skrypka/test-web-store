using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Пустое поле ввода почты")]
        [Display(Name = "Введите почту")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Вы забыли указать пароль")]
        [Display(Name = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}