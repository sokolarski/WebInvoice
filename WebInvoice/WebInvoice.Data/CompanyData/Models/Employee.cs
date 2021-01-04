using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class Employee:BaseDeletableModel<int>
    {
        public Employee()
        {
            this.VatDocumentsWriter = new HashSet<VatDocument>();
            this.VatDocumentsRecipient = new HashSet<VatDocument>();
            this.NonVatDocumentsWriter = new HashSet<NonVatDocument>();
            this.NonVatDocumentsRecipient = new HashSet<NonVatDocument>();
        }

        [MaxLength(50)]
        public string FullName { get; set; }

        public bool IsActive { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? PartnerId { get; set; }
        public Partner Partner { get; set; }

        public virtual ICollection<VatDocument> VatDocumentsWriter { get; set; }

        public virtual ICollection<VatDocument> VatDocumentsRecipient { get; set; }

        public virtual ICollection<NonVatDocument> NonVatDocumentsWriter { get; set; }

        public virtual ICollection<NonVatDocument> NonVatDocumentsRecipient { get; set; }
    }
}
