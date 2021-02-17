using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.ViewDocument
{
    public class TottalByVat
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Percentage { get; set; }

        public decimal Base { get; set; }

        public decimal Vat { get; set; }

        public decimal Tottal { get; set; }
    }
}
