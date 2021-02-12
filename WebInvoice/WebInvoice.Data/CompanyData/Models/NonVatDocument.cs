using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models.Enums;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class NonVatDocument:BaseDeletableModel<long>
    {
        public NonVatDocument()
        {
            this.Sales = new HashSet<Sales>();
        }

        public decimal SubTottal { get; set; }

        public decimal? Vat { get; set; }

        public decimal Tottal { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime VatReasonDate { get; set; }

        public string Description { get; set; }

        public string FreeText { get; set; }


        public byte[] AllDocumentData { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int PartnerId { get; set; }
        public Partner Partner { get; set; }

        public NonVatDocumentTypes Type { get; set; }

        public int CompanyObjectId { get; set; }
        public CompanyObject CompanyObject { get; set; }

        public int? WriterEmployeeId { get; set; }
       
        public Employee WriterEmployee { get; set; }

        public int? RecipientEmployeeId { get; set; }
      
        public Employee RecipientEmployee { get; set; }

        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }

        public int? BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public ICollection<Sales> Sales { get; set; }
    }
}
