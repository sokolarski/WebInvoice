using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.CompanyObject
{
    public class CompanyObjectDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage ="Името трябва да бъде до 50 символа!")]
        [Display(Name ="Име на обекта")]
        public string Name { get; set; }

        
        [MaxLength(150, ErrorMessage = "Описанието трябва да бъде до 150 символа!")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Range(0,9999999999, ErrorMessage = "Номерът трябва да е между 1 и 9999999999 !")]
        [Display(Name = "Начален номер")]
        public long StartNum { get; set; }

        [Required]
        [Range(0, 9999999999, ErrorMessage = "Номерът трябва да е между 1 и 9999999999 !")]
        [Display(Name = "Краен номер")]
        public long EndNum { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Град трябва да бъде до 50 символа!")]
        [Display(Name = "Град")]
        public string City { get; set; }

        [Display(Name = "да се отваря по подразбиране")]
        public bool IsActive { get; set; }

        [Display(Name = "Брой документи")]
        public int CountOfDocuments { get; set; }

        public string ObjectGUID { get; set; }

        public string CompanyObjectSlug { get; set; }

        public bool IsValidObjectDocumentRange { get; set; }
        public ICollection<string> ErrorMassages { get; set; } = new List<string>();
    }
}
