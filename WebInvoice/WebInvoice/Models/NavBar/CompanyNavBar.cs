using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInvoice.Models.NavBar
{
    public class CompanyNavBar
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string GUID { get; set; }

        public bool IsActive { get; set; }
    }
}
