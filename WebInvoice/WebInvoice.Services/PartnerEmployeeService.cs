using Microsoft.EntityFrameworkCore;
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

        public async Task<ICollection<EmployeeDto>> GetAllCompanyEmployees(int companyId)
        {
            var employees = await employeeRepository.AllAsNoTracking().Where(e => e.PartnerId == companyId)
                                                    .Select(e => new EmployeeDto()
                                                    {
                                                        Id = e.Id,
                                                        FullName = e.FullName,
                                                        IsActive = e.IsActive,
                                                    }).ToListAsync();

            return employees;
        }

        public async Task<EmployeeDto> GetById(int id)
        {
            var employee = await employeeRepository.AllAsNoTracking().Where(e => e.Id == id)
                                                    .Select(e => new EmployeeDto()
                                                    {
                                                        Id = e.Id,
                                                        FullName = e.FullName,
                                                        IsActive = e.IsActive,
                                                    }).FirstOrDefaultAsync();
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
                PartnerId = companyId,
            };

            await employeeRepository.AddAsync(employee);
            await employeeRepository.SaveChangesAsync();
        }

        public async Task<int?> GetOrSetEmployeeIdByNameAsync(string name, int partnerId)
        {
            if (String.IsNullOrEmpty(name))
            {
                return null;
            }
            var employee = await employeeRepository.AllAsNoTracking().Where(e => e.PartnerId == partnerId && e.FullName == name).FirstOrDefaultAsync();
            if (employee != null)
            {
                return employee.Id;
            }

            var newEmployee = new Employee()
            {
                FullName = name,
                PartnerId = partnerId,
            };

            await employeeRepository.AddAsync(employee);
            await employeeRepository.SaveChangesAsync();
            return newEmployee.Id;
        }

        private void SetAllNonActive(int companyId)
        {

            var employees = employeeRepository.All().Where(e => e.PartnerId == companyId); ;

            foreach (var employee in employees)
            {
                employee.IsActive = false;
                employeeRepository.Update(employee);
            }
        }
    }
}

