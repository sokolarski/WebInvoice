using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.VatType
{
    public class VatTypeDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Полето е задължително")]
        [MaxLength(50,ErrorMessage = "Името трябва да бъде до 50 символа!")]
        [Display(Name ="Име на групата")]
        public string Name { get; set; }

        [MaxLength(150, ErrorMessage = "Описанието трябва да бъде до 150 символа!")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Range(0.0,100.0,ErrorMessage ="Позволена стойност между 0 и 100")]
        [Display(Name = "Проценти")]
        public decimal Percantage { get; set; }

        [Display(Name = "Да се използва по подразбиране")]
        public bool IsActive { get; set; }

        public bool IsValidVatType { get; set; }
        public ICollection<string> ErrorMassages { get; set; } = new List<string>();
    }
}
