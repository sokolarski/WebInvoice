using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;

namespace WebInvoice.Services.Reports
{
    public class ReportExport
    {
        public string CompanyName { get; set; }

        public string DocumentsTypes { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Objects { get; set; }

        public IEnumerable<DocumentShortView> Documents { get; set; }
    }
}
