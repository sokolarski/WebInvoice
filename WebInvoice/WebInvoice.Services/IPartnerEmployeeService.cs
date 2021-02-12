using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.Employee;

namespace WebInvoice.Services
{
    public interface IPartnerEmployeeService
    {
        Task Create(EmployeeDto employeeDto, int companyId);
        Task Edit(EmployeeDto employeeDto, int companyId);
        Task<ICollection<EmployeeDto>> GetAllCompanyEmployees(int companyId);
        Task<EmployeeDto> GetById(int id);
        Task<int?> GetOrSetEmployeeIdByNameAsync(string name, int partnerId);
    }
}