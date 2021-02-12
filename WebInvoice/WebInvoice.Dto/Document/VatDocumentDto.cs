using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.dto;
using WebInvoice.Dto.BankAccount;
using WebInvoice.Dto.Partner;
using WebInvoice.Dto.PaymentType;
using WebInvoice.Dto.Product;
using WebInvoice.Dto.VatType;

namespace WebInvoice.Dto.Document
{
    public class VatDocumentDto
    {
        public VatDocumentDto()
        {
            this.Sales = new List<ProductDocumentDto>();
            this.ErrorMassages = new List<string>();
        }
        public long Id { get; set; }

        public decimal SubTottal { get; set; }

        public decimal? Vat { get; set; }

        public decimal Tottal { get; set; }

        [Required]
        public string CreatedDate { get; set; }

        [Required]
        public string VatReasonDate { get; set; }

        [MaxLength(150, ErrorMessage ="Максимална дължина 150 символа")]
        public string Description { get; set; }


        [MaxLength(150, ErrorMessage = "Максимална дължина 150 символа")]
        public string FreeText { get; set; }

        public int PartnerId { get; set; }

        public VatDocumentTypes Type { get; set; }

        public int CompanyObjectId { get; set; }
       
        public string WriterEmployee { get; set; }

        public string RecipientEmployee { get; set; }

        public int PaymentTypeId { get; set; }

        public int? BankAccountId { get; set; }

        public string SalesJson => JsonConvert.SerializeObject(this.Sales);
        public bool HasErrors => this.ErrorMassages.Count > 0;
        public ICollection<string> ErrorMassages { get; set; }
        public List<ProductDocumentDto> Sales { get; set; }
    }
}
