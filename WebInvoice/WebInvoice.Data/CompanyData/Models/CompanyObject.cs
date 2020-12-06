using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class CompanyObject:BaseDeletableModel<int>
    {
        public CompanyObject()
        {
            this.VatDocuments = new HashSet<VatDocument>();
            this.NonVatDocuments = new HashSet<NonVatDocument>();
        }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public long StartNum { get; set; }

        public long EndNum { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        public bool IsActive { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public virtual ICollection<VatDocument> VatDocuments { get; set; }

        public virtual ICollection<NonVatDocument> NonVatDocuments { get; set; }

    }
}
