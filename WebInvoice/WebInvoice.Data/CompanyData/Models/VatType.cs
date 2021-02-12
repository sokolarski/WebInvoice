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
            this.Products = new HashSet<Product>();
            this.FreeProducts = new HashSet<FreeProduct>();
        }
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public decimal Percantage { get; set; }

        public bool IsActive { get; set; }


        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<FreeProduct> FreeProducts { get; set; }

    }
}
