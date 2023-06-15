using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Goods
    {
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "ID")]
        public int GoodId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название!")]
        public string Name { get; set; }

        [Display(Name = "Автор")]
        [Required(ErrorMessage = "Введите автора!")]
        public string Author { get; set; }

        [DataType(DataType.MultilineText)]
        [UIHint("MultilineText")]
        [Required(ErrorMessage = "Введите описание! ")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Введите категорию! ")]
        public string Genre { get; set; }

      
        [Display(Name = "Изображение")]
        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
        
        /*
        [Display(Name = "Файл")]
        public byte[] FileData { get; set; }
        
        [HiddenInput(DisplayValue = false)]
        public string FileMimeType { get; set; }*/
    }
}