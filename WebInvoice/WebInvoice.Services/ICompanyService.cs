using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Dto.Company;

namespace WebInvoice.Services
{
    public interface ICompanyService
    {
        Task<bool> CreateCompanyAsync(CompanyDto companyInputDto, string userId);

    }
}
