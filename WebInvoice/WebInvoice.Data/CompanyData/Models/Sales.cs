using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class Sales : BaseDeletableModel<long>
    {
        public Sales()
        {

        }
        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public decimal? Vat { get; set; }

        public decimal TottalWithVat { get; set; }

        public int? ProductId { get; set; }
        public Product Product { get; set; }

        public int? FreeProductId { get; set; }
        public FreeProduct FreeProduct { get; set; }

        public long? VatDocumentId { get; set; }
        public VatDocument VatDocument { get; set; }

        public long? NonVatDocumentId { get; set; }
        public NonVatDocument NonVatDocument { get; set; }


    }
}
