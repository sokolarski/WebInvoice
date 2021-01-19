using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
   public class Product:BaseDeletableModel<int>
    {
        public Product()
        {

        }

        public string Name { get; set; }

        public string Barcode { get; set; }

        public string Description { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal BasePrice { get; set; }

        public int VatTypeId { get; set; }
        public VatType VatType { get; set; }

        public int QuantityTypeId { get; set; }
        public QuantityType QuantityType { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
