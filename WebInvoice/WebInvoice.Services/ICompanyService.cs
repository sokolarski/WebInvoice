using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Dto.Company;

namespace WebInvoice.Services
{
    public interface ICompanyService
    {
        bool CreateCompany(CompanyInputDto companyInputDto);

        bool EditCompany(CompanyInputDto companyInputDto);
    }
}
