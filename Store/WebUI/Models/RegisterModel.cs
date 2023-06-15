using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class RegisterModel
    {

        [Display(Name = "Почта")]
        [Required(ErrorMessage = "Укажите почту!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите сколько Вам лет")]
        [Display(Name = "Сколько Вам лет?")]
        [Range(6, 120, ErrorMessage = "Возраст должен быть между 6-120 ")]
        public int Year { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name ="Подтверждение ввод пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}