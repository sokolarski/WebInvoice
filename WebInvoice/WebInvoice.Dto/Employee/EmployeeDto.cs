using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Полето е задължително")]
        [MaxLength(50, ErrorMessage = "Името трябва да бъде до 50 символа!")]
        [Display(Name ="Име")]
        public string FullName { get; set; }

        [Display(Name = "Да се използва по подразбиране")]
        public bool IsActive { get; set; }
    }
}
