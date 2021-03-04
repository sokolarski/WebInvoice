using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.ViewDocument
{
     public class DocumentShortView
    {
        public long Id { get; set; }

        public bool IsVatDocument { get; set; }

        public string DocumentType { get; set; }

        public string PartnerName { get; set; }

        public string CreatedDate { get; set; }

        public decimal Base { get; set; }

        public decimal Vat { get; set; }

        public decimal Tottal { get; set; }
    }
}
