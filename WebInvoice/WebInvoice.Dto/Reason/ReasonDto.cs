using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using WebInvoice.Dto.VatType;



namespace WebInvoice.Dto.Reason
{
    public class ReasonDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(50, ErrorMessage = "Името трябва да бъде до 50 символа!")]
        [Display(Name = "Име на групата")]
        public string Name { get; set; }

        [MaxLength(150, ErrorMessage = "Описанието трябва да бъде до 150 символа!")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int VatTypeId { get; set; }
        public VatTypeView VatType { get; set; }

        public IEnumerable<VatTypeView> SelectVatType { get; set; }

    }
}
