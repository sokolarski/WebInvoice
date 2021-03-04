using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;

namespace WebInvoice.Services.Reports
{
    public class Report
    {
        public decimal Base { get; set; }

        public decimal Vat { get; set; }

        public decimal Tottal { get; set; }

        public DateTime Date => DateTime.Now;

        public PaginatedList<DocumentShortView> Documents { get; set; }
    }
}
