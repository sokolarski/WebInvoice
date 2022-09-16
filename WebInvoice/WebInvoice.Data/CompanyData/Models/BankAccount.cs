using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class BankAccount:BaseDeletableModel<int>
    {
        public BankAccount()
        {
            this.VatDocuments = new HashSet<VatDocument>();
            this.NonVatDocuments = new HashSet<NonVatDocument>();
        }
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string BankName { get; set; }

        [MaxLength(11)]
        public string BIC { get; set; }

        [MaxLength(40)]
        public string IBAN { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? PartnerId { get; set; }
        public Partner Partner { get; set; }

        public virtual ICollection<VatDocument> VatDocuments { get; set; }

        public virtual ICollection<NonVatDocument> NonVatDocuments { get; set; }

    }
}
