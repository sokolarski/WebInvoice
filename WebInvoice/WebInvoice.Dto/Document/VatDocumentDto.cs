using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.dto;
using WebInvoice.Dto.BankAccount;
using WebInvoice.Dto.Partner;
using WebInvoice.Dto.PaymentType;
using WebInvoice.Dto.Product;
using WebInvoice.Dto.Reason;
using WebInvoice.Dto.VatType;

namespace WebInvoice.Dto.Document
{
    public class VatDocumentDto
    {
        public VatDocumentDto()
        {
            this.Sales = new List<ProductShortDto>();
        }
        public int Id { get; set; }

        public decimal SubTottal { get; set; }

        public decimal? Vat { get; set; }

        public decimal Tottal { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime VatReasonDate { get; set; }

        public string Description { get; set; }

        public string FreeText { get; set; }

        public int PartnerId { get; set; }

        public VatDocumentTypes Type { get; set; }

        public int CompanyObjectId { get; set; }
       
        public int? WriterEmployeeId { get; set; }

        public int? RecipientEmployeeId { get; set; }

        public int PaymentTypeId { get; set; }

        public int? BankAccountId { get; set; }

        public int ReasonId { get; set; }

        public int VatTypeId { get; set; }

        public List<ProductShortDto> Sales { get; set; }
    }
}
