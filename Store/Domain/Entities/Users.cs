using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Users
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Введите сколько Вам лет")]
        [Display(Name = "Колько Вам лет?")]
        [Range(6, 120, ErrorMessage = "Возраст должен быть между 6-120 ")]
        public int Year { get; set; }

        [Display(Name = "Почта")]
        [Required(ErrorMessage = "Укажите почту!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Введите категорию! ")]
        public string UserName { get; set; }
    }
}