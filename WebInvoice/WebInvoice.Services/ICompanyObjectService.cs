using System.Threading.Tasks;
using WebInvoice.Dto.CompanyObject;

namespace WebInvoice.Services
{
    public interface ICompanyObjectService
    {
        Task Create(CompanyObjectDto companyObjectDto);
        Task Edit(CompanyObjectDto companyObjectDto);
        Task<CompanyObjectListDto> GetAllCompanyObjects();
        Task<CompanyObjectDto> GetById(int id);
        Task ValidateObjectDucumentRange(CompanyObjectDto companyObjectDto);
        Task Delete(CompanyObjectDto companyObjectDto);
    }
}