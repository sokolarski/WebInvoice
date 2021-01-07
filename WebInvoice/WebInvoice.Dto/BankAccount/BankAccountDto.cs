using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.BankAccount
{
    public class BankAccountDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(50, ErrorMessage = "Името трябва да бъде до 50 символа!")]
        [Display(Name = "Име на сметка")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(50, ErrorMessage = "Името трябва да бъде до 50 символа!")]
        [Display(Name = "Банка")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(11, ErrorMessage = "BIC трябва да бъде до 11 символа!")]
        [Display(Name = "BIC")]
        public string BIC { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(20, ErrorMessage = "IBAN трябва да бъде до 20 символа!")]
        [Display(Name = "IBAN")]
        public string IBAN { get; set; }

        [MaxLength(150, ErrorMessage = "Описанието трябва да бъде до 150 символа!")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Да се използва по подразбиране")]
        public bool IsActive { get; set; }

        public bool IsValidBankAccount { get; set; }
        public ICollection<string> ErrorMassages { get; set; } = new List<string>();
    }
}
