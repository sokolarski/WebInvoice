using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.Employee;

namespace WebInvoice.Services
{
    public interface IEmployeeService
    {
        Task Create(EmployeeDto employeeDto);
        Task Edit(EmployeeDto employeeDto);
        Task<ICollection<EmployeeDto>> GetAllCompanyEmployees();
        Task<EmployeeDto> GetById(int id);
    }
}