using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.ViewDocument
{
    public class ProductRow
    {
      
        public bool IsProduct { get; set; }

        public string Name { get; set; }

        public string ProductType { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TottalPrice { get; set; }

        public int VatTypeId { get; set; }

        public string VatTypeName { get; set; }

        public decimal VatTypePercentage { get; set; }

    }
}
