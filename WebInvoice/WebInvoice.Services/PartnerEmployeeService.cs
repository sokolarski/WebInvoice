using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.Employee;

namespace WebInvoice.Services
{
    public class PartnerEmployeeService : IPartnerEmployeeService
    {
        private readonly ICompanyDeletableEntityRepository<Employee> employeeRepository;

        public PartnerEmployeeService(ICompanyDeletableEntityRepository<Employee> employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public ICollection<EmployeeDto> GetAllCompanyEmployees(int companyId)
        {
            var employees = employeeRepository.AllAsNoTracking().Where(e => e.CompanyId == companyId)
                                                    .Select(e => new EmployeeDto()
                                                    {
                                                        Id = e.Id,
                                                        FullName = e.FullName,
                                                        IsActive = e.IsActive,
                                                    }).ToList();

            return employees;
        }

        public EmployeeDto GetById(int id)
        {
            var employee = employeeRepository.AllAsNoTracking().Where(e => e.Id == id)
                                                    .Select(e => new EmployeeDto()
                                                    {
                                                        Id = e.Id,
                                                        FullName = e.FullName,
                                                        IsActive = e.IsActive,
                                                    }).FirstOrDefault();
            return employee;
        }

        public async Task Edit(EmployeeDto employeeDto, int companyId)
        {
            var employee = employeeRepository.All().Where(e => e.Id == employeeDto.Id).FirstOrDefault();

            if (employeeDto.Id != 0 && employee != null)
            {
                if (employeeDto.IsActive == true)
                {
                    SetAllNonActive(companyId);
                }

                employee.FullName = employeeDto.FullName;
                employee.IsActive = employeeDto.IsActive;

                employeeRepository.Update(employee);
                await employeeRepository.SaveChangesAsync();
            }
        }

        public async Task Create(EmployeeDto employeeDto, int companyId)
        {

            if (employeeDto.IsActive == true)
            {
                SetAllNonActive(companyId);
            }
            var employee = new Employee()
            {
                FullName = employeeDto.FullName,
                IsActive = employeeDto.IsActive,
                CompanyId = companyId,
            };

            await employeeRepository.AddAsync(employee);
            await employeeRepository.SaveChangesAsync();
        }

        private void SetAllNonActive(int companyId)
        {

            var employees = employeeRepository.All().Where(e => e.CompanyId == companyId); ;

            foreach (var employee in employees)
            {
                employee.IsActive = false;
                employeeRepository.Update(employee);
            }
        }
    }
}

