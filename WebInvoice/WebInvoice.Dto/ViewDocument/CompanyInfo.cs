using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.ViewDocument
{
   public class CompanyInfo
    {

        public string Name { get; set; }
      
        public string Address { get; set; }
        
        public string City { get; set; }

        public string Country { get; set; }

        public string EIK { get; set; }

        public string VatId { get; set; }

        public string Email { get; set; }

        public string MOL { get; set; }

        public bool IsVatRegistered { get; set; }

    }
}
