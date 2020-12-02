using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class Reason:BaseDeletableModel<int>
    {
        public Reason()
        {
            this.VatDocuments = new HashSet<VatDocument>();
            this.NonVatDocuments = new HashSet<NonVatDocument>();
        }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int VatTypeId { get; set; }
        public VatType VatType { get; set; }

        public virtual ICollection<VatDocument> VatDocuments { get; set; }

        public virtual ICollection<NonVatDocument> NonVatDocuments { get; set; }
    }
}
