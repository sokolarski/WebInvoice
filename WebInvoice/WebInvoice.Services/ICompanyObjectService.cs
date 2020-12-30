using System.Threading.Tasks;
using WebInvoice.Dto.CompanyObject;

namespace WebInvoice.Services
{
    public interface ICompanyObjectService
    {
        Task<CompanyObjectDto> Create(CompanyObjectDto companyObjectDto);
        Task<CompanyObjectDto> Edit(CompanyObjectDto companyObjectDto);
        Task<CompanyObjectListDto> GetAllCompanyObjects();
        Task<CompanyObjectDto> GetById(int id);
        Task<CompanyObjectDto> ObjectDucumentRange(CompanyObjectDto companyObjectDto);
    }
}