using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class FreeProduct:BaseDeletableModel<int>
    {

        public string Name { get; set; }

        public string Barcode { get; set; }

        public string Description { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TottalPrice { get; set; }
    }
}
