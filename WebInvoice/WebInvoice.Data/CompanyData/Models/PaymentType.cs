using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class PaymentType : BaseDeletableModel<int>
    {
        public PaymentType()
        {
            this.VatDocuments = new HashSet<VatDocument>();
            this.NonVatDocuments = new HashSet<NonVatDocument>();
        }
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }


        public bool RequireBankAccount { get; set; }

        public bool IsActiv { get; set; }

        public virtual ICollection<VatDocument> VatDocuments { get; set; }

        public virtual ICollection<NonVatDocument> NonVatDocuments { get; set; }
    }
}
