using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInvoice.Dto.CompanyObject
{
    public class CompanyObjectListDto
    {
        public CompanyObjectListDto()
        {
            this.CompanyObjects = new List<CompanyObjectDto>();
        }
        public ICollection<CompanyObjectDto> CompanyObjects { get; set; }
    }
}
