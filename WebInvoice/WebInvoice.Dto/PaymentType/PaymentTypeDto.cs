using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.PaymentType
{
    public class PaymentTypeDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(50, ErrorMessage = "Името трябва да бъде до 50 символа!")]
        [Display(Name = "Име ")]
        public string Name { get; set; }


        [MaxLength(150, ErrorMessage = "Името трябва да бъде до 150 символа!")]
        [Display(Name = "Описание ")]
        public string Description { get; set; }

        [Display(Name = "Да се използва по подразбиране")]
        public bool IsActiv { get; set; }
    }
}
