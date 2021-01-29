using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Dto.QuantityType;
using WebInvoice.Dto.VatType;

namespace WebInvoice.Dto.Product
{
    public class ProductDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [MaxLength(50, ErrorMessage = "Името трябва да бъде до 50 символа!")]
        [Display(Name = "Име на Продукта")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "Баркод трябва да бъде до 50 символа!")]
        [Display(Name = "Баркод")]
        public string Barcode { get; set; }

        [MaxLength(150, ErrorMessage = "Описание трябва да бъде до 150 символа!")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage = "Полето е задължително!")]
        [Range(double.MinValue,double.MaxValue, ErrorMessage = "Стойността е прекалено голяма")]
        public decimal Quantity { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Полето е задължително!")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Стойността е прекалено голяма")]
        public decimal Price { get; set; }

        [Display(Name = "Доставна цена")]
        [Required(ErrorMessage ="Полето е задължително!")]
        [Range(double.MinValue, double.MaxValue ,ErrorMessage ="Стойността е прекалено голяма")]
        public decimal BasePrice { get; set; }

        public decimal PriceWithVat { get; set; }
        public int VatTypeId { get; set; }
        public VatTypeView VatType { get; set; }

        public int QuantityTypeId { get; set; }
        public QuantityTypeShortView QuantityType { get; set; }

        public IEnumerable<VatTypeView> SelectVatType { get; set; }

        public IEnumerable<QuantityTypeShortView> SelectQuantityType { get; set; }
    }
}
