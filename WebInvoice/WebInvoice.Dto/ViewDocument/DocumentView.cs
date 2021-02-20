using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.dto;
using WebInvoice.Dto.BankAccount;
using WebInvoice.Dto.Company;
using WebInvoice.Dto.Partner;
using WebInvoice.Dto.PaymentType;

namespace WebInvoice.Dto.ViewDocument
{
    public class DocumentView
    {
        public DocumentView()
        {
            this.Products = new List<ProductRow>();
            this.TottalByVats = new List<TottalByVat>();
        }

        public long Id { get; set; }

        public string Type { get; set; }

        public decimal SubTottal { get; set; }

        public decimal Vat { get; set; }

        public decimal Tottal { get; set; }

        public string TottalSlovom { get; set; }

        public string CreatedDate { get; set; }

        public string VatReasonDate { get; set; }

        public string Description { get; set; }

        public string FreeText { get; set; }

        public string WriterEmployee { get; set; }

        public string RecipientEmployee { get; set; }

        public string ObjectName { get; set; }

        public string ObjectCity { get; set; }

        public CompanyDto Company { get; set; }

        public PartnerDto Partner { get; set; }

        public PaymentTypeDto PaymentType{ get; set; }

        public BankAccountDto  BankAccount{ get; set; }

        public ICollection<ProductRow> Products { get; set; }
        public ICollection<TottalByVat> TottalByVats { get; set; }
    }
}
