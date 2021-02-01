using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class VatType : BaseDeletableModel<int>
    {
        public VatType()
        {
           // this.Reasons = new HashSet<Reason>();
            this.Products = new HashSet<Product>();
            this.VatDocuments = new HashSet<VatDocument>();
            this.NonVatDocuments = new HashSet<NonVatDocument>();
        }
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public decimal Percantage { get; set; }

        public bool IsActive { get; set; }

       // public virtual ICollection<Reason> Reasons { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<VatDocument> VatDocuments { get; set; }

        public virtual ICollection<NonVatDocument> NonVatDocuments { get; set; }
    }
}
