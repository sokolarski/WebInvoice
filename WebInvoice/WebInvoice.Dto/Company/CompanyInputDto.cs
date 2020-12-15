using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.Company
{
    public class CompanyInputDto
    {
        
        [Required(ErrorMessage = "Полето е задължително!")]
        [MaxLength(50, ErrorMessage = "Максимална дължина 50 символа")]
        [Display(Name = "Име на фирма")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Полето е задължително!")]
        [MaxLength(150, ErrorMessage = "Максимална дължина 150 символа")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Полето е задължително!")]
        [MaxLength(50, ErrorMessage = "Максимална дължина 50 символа")]
        [Display(Name = "Град")]
        public string City { get; set; }

        [Required(ErrorMessage = "Полето е задължително!")]
        [MaxLength(50, ErrorMessage ="Максимална дължина 50 символа")]
        [Display(Name = "Държава")]
        public string Country { get; set; }

        [MaxLength(20, ErrorMessage = "ЕИК трябва да бъде точно 9 цифри")]
        [RegularExpression(@"^[0-9]{2,13}$", ErrorMessage = "ЕИК номер трябва да започва с две латински букви последвани от 2 до 13 символа")]
        [Display(Name ="ЕИК")]
        public string EIK { get; set; }

        [Display(Name = "ДДС номер")]
        [RegularExpression(@"^[A-Z]{2}[0-9A-Z]{2,13}$", ErrorMessage = "ДДС номер трябва да започва с две главни латински букви последвани от 2 до 13 символа")]
        public string VatId { get; set; }

        [EmailAddress]
        [Display(Name = "Електронна поща")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Полето е задължително!")]
        [MaxLength(50, ErrorMessage = "Максимална дължина 50 символа")]
        [Display(Name = "Метериално отгорвоно лице (МОЛ)")]
        public string MOL { get; set; }

        [Display(Name = "Регистрация по ДДС")]
        public bool IsVatRegistered { get; set; }

        public string LogoPath { get; set; }

        [MaxLength(300, ErrorMessage = "Максимална дължина 300 символа")]
        [Display(Name = "Пояснения")]
        public string Description { get; set; }


    }
}
