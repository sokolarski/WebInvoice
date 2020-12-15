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
        Task<bool> CreateCompanyAsync(CompanyInputDto companyInputDto, string userId);

        bool EditCompany(CompanyInputDto companyInputDto);
    }
}
