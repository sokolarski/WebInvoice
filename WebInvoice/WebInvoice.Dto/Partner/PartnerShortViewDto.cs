using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.Partner
{
    public class PartnerShortViewDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string EIK { get; set; }

        public int CountOfDocuments { get; set; }

    }
}
