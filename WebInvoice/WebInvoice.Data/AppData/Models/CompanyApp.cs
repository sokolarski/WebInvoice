using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.AppData.Models
{
    public class CompanyApp : BaseDeletableModel<int>
    {
        public string CompanyName { get; set; }

        public string ConnStr { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        public string GUID { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
