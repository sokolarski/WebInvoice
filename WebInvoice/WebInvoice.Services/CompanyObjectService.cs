using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.CompanyObject;

namespace WebInvoice.Services
{

    public class CompanyObjectService : ICompanyObjectService
    {
        private readonly IAppDeletableEntityRepository<CompanyApp> appDeletableEntityRepository;
        private readonly ICompanyDeletableEntityRepository<Company> companyDeletableEntityRepository;
        private readonly IUserCompanyTemp userCompanyTemp;

        public CompanyObjectService(IAppDeletableEntityRepository<CompanyApp> appDeletableEntityRepository,
                                    ICompanyDeletableEntityRepository<Company> companyDeletableEntityRepository,
                                    IUserCompanyTemp userCompanyTemp)
        {
            this.appDeletableEntityRepository = appDeletableEntityRepository;
            this.companyDeletableEntityRepository = companyDeletableEntityRepository;
            this.userCompanyTemp = userCompanyTemp;
        }

        public async Task<CompanyObjectListDto> GetAllCompanyObjects()
        {
            var resultCompanyObjects = new CompanyObjectListDto();

            var companyObects = await companyDeletableEntityRepository.AllAsNoTracking().OrderBy(c => c.Id)
                .Select(c => c.CompanyObjects.Select(co =>
               new CompanyObjectDto()
               {
                   City = co.City,
                   Description = co.Description,
                   StartNum = co.StartNum,
                   EndNum = co.EndNum,
                   Id = co.Id,
                   Name = co.Name,
                   IsActive = co.IsActive,
                   CountOfDocuments = co.VatDocuments.Count,
               })
                ).LastAsync();

            resultCompanyObjects.CompanyObjects = companyObects.ToList();
            return resultCompanyObjects;
        }

        public async Task<CompanyObjectDto> GetById(int id)
        {
            var companyObect = await companyDeletableEntityRepository.AllAsNoTracking().OrderBy(c => c.Id)
                .Select(c => c.CompanyObjects.Where(co => co.Id == id).Select(co =>
               new CompanyObjectDto()
               {
                   City = co.City,
                   Description = co.Description,
                   StartNum = co.StartNum,
                   EndNum = co.EndNum,
                   Id = co.Id,
                   Name = co.Name,
                   IsActive = co.IsActive,
                   CountOfDocuments = co.VatDocuments.Count,
               })
                ).LastAsync();

            return companyObect.FirstOrDefault();
        }

        public async Task<CompanyObjectDto> Edit(CompanyObjectDto companyObjectDto)
        {
            var company = await companyDeletableEntityRepository.All().Include(x => x.CompanyObjects).OrderBy(c => c.Id).LastAsync();
            var companyObject = company.CompanyObjects.Where(co => co.Id == companyObjectDto.Id).FirstOrDefault();
            companyObject.Name = companyObjectDto.Name;
            companyObject.City = companyObjectDto.City;
            companyObject.StartNum = companyObjectDto.StartNum;
            companyObject.EndNum = companyObjectDto.EndNum;
            companyObject.IsActive = companyObjectDto.IsActive;
            companyObject.Description = companyObjectDto.Description;

            companyDeletableEntityRepository.Update(company);
            await companyDeletableEntityRepository.SaveChangesAsync();

            return companyObjectDto;
            
        }

        public async Task<CompanyObjectDto> Create(int id)
        {
            return null;
        }
    }
}
