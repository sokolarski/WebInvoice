using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.dto
{
    public enum VatDocumentTypes
    {
        Invoice = 1,
        Credit = 2,
        Debit = 3
    }
}
