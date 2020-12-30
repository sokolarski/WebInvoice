using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Data.AppData.Models
{
    public class CompanyAppObject
    {
        public int Id { get; set; }

        public string ObjectName { get; set; }

        public string ObjectSlug { get; set; }

        public bool IsActive { get; set; }

        public string GUID { get; set; }

        public int CompanyAppId { get; set; }
        public CompanyApp CompanyApp { get; set; }

    }
}
