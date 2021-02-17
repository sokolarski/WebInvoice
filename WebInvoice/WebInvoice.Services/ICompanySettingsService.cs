using System.Threading.Tasks;
using WebInvoice.Dto.Company;

namespace WebInvoice.Services
{
    public interface ICompanySettingsService
    {
        Task<CompanyDto> GetCompanyInfo();
        Task Edit(CompanyDto companyDto);
        Task ApplyMigration();
        Task<CompanyDto> GetCompanyInfoById(int id);
    }
}