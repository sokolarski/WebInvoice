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

    public class EmployeeService : IEmployeeService
    {
        private readonly ICompanyDeletableEntityRepository<Employee> employeeRepository;

        public EmployeeService(ICompanyDeletableEntityRepository<Employee> employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public ICollection<EmployeeDto> GetAllCompanyEmployees()
        {
            var company = employeeRepository.Context.Companies.OrderBy(c => c.Id).LastOrDefault();

            var employees = employeeRepository.AllAsNoTracking().Where(e => e.CompanyId == company.Id)
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

        public async Task Edit(EmployeeDto employeeDto)
        {
            var employee = employeeRepository.All().Where(e => e.Id == employeeDto.Id).FirstOrDefault();

            if (employeeDto.Id != 0 && employee != null)
            {
                if (employeeDto.IsActive == true)
                {
                    SetAllNonActive();
                }

                employee.FullName = employeeDto.FullName;
                employee.IsActive = employeeDto.IsActive;

                employeeRepository.Update(employee);
                await employeeRepository.SaveChangesAsync();
            }
        }

        public async Task Create(EmployeeDto employeeDto)
        {
            var company = employeeRepository.Context.Companies.OrderBy(c => c.Id).LastOrDefault();
            if (employeeDto.IsActive == true)
            {
                SetAllNonActive();
            }
            var employee = new Employee()
            {
                FullName = employeeDto.FullName,
                IsActive = employeeDto.IsActive,
                CompanyId = company.Id,
            };

            await employeeRepository.AddAsync(employee);
            await employeeRepository.SaveChangesAsync();
        }

        private void SetAllNonActive()
        {
            var company = employeeRepository.Context.Companies.OrderBy(c => c.Id).LastOrDefault();

            var employees = employeeRepository.All().Where(e => e.CompanyId == company.Id); ;

            foreach (var employee in employees)
            {
                employee.IsActive = false;
                employeeRepository.Update(employee);
            }
        }
    }
}
