using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Dto.VatType;

namespace WebInvoice.Dto.Document
{
    public class ProductDocumentDto
    {

        public int? ProductId { get; set; }

        public int? FreeProductID { get; set; }

        public bool IsProduct { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public string ProductType { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public decimal Quantity { get; set; }

        public decimal AvailableQuantity { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public decimal TottalPrice { get; set; }

        public int VatTypeId { get; set; }

        public long SaleId { get; set; }

    }
}
