using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class QuantityType:BaseDeletableModel<int>
    {
        public QuantityType()
        {
            this.Products = new HashSet<Product>();
        }
        [MaxLength(50)]
        public string Type { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
