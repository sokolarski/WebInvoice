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
        private readonly IStringGenerator stringGenerator;

        public CompanyObjectService(IAppDeletableEntityRepository<CompanyApp> appDeletableEntityRepository,
                                    ICompanyDeletableEntityRepository<Company> companyDeletableEntityRepository,
                                    IUserCompanyTemp userCompanyTemp,
                                    IStringGenerator stringGenerator)
        {
            this.appDeletableEntityRepository = appDeletableEntityRepository;
            this.companyDeletableEntityRepository = companyDeletableEntityRepository;
            this.userCompanyTemp = userCompanyTemp;
            this.stringGenerator = stringGenerator;
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

            if (company.GUID == userCompanyTemp.CompanyGUID && companyObject.GUID == userCompanyTemp.CompanyObjectGUID)
            {
                var companyApp = await appDeletableEntityRepository.All().Include(c => c.CompanyAppObjects).Where(c => c.GUID == company.GUID).FirstOrDefaultAsync();
                var companyAppObject = companyApp.CompanyAppObjects.Where(co => co.GUID == companyObject.GUID).FirstOrDefault();


                var companyObjectSlug = stringGenerator.GenerateSlug(companyObjectDto.Name);
                companyObjectDto.CompanyObjectSlug = companyObjectSlug;
                companyAppObject.ObjectName = companyObjectDto.Name;
                companyAppObject.ObjectSlug = companyObjectSlug;
                appDeletableEntityRepository.Update(companyApp);

                companyObject.Name = companyObjectDto.Name;
                companyObject.City = companyObjectDto.City;
                companyObject.StartNum = companyObjectDto.StartNum;
                companyObject.EndNum = companyObjectDto.EndNum;
                companyObject.IsActive = companyObjectDto.IsActive;
                companyObject.Description = companyObjectDto.Description;

                companyDeletableEntityRepository.Update(company);
                await companyDeletableEntityRepository.SaveChangesAsync();
                await appDeletableEntityRepository.SaveChangesAsync();

            }
            else
            {
                return null;
            }

            return companyObjectDto;

        }

        public async Task<CompanyObjectDto> Create(CompanyObjectDto companyObjectDto)
        {
            var companyObjectGUID = Guid.NewGuid().ToString();

            var companyAppObject = new CompanyAppObject()
            {
                ObjectName = companyObjectDto.Name,
                GUID = companyObjectGUID,
                IsActive = companyObjectDto.IsActive,
                ObjectSlug = stringGenerator.GenerateSlug(companyObjectDto.Name),
            };

            var companyApp = appDeletableEntityRepository.All().Where(c => c.GUID == userCompanyTemp.CompanyGUID).FirstOrDefault();
            companyApp.CompanyAppObjects.Add(companyAppObject);
           

            var companyObject = new CompanyObject()
            {
                Name = companyObjectDto.Name,
                City = companyObjectDto.City,
                StartNum = companyObjectDto.StartNum,
                EndNum = companyObjectDto.EndNum,
                IsActive = companyObjectDto.IsActive,
                Description = companyObjectDto.Description,
                GUID = companyObjectGUID,
            };

            var company = await companyDeletableEntityRepository.All().OrderBy(c => c.Id).LastAsync();
            company.CompanyObjects.Add(companyObject);

            await appDeletableEntityRepository.SaveChangesAsync();
            await companyDeletableEntityRepository.SaveChangesAsync();

            return companyObjectDto;
        }

        public async Task<CompanyObjectDto> ObjectDucumentRange(CompanyObjectDto companyObjectDto)
        {
            var company = await companyDeletableEntityRepository.All().Include(x => x.CompanyObjects).OrderBy(c => c.Id).LastAsync();
            var companyObjects = company.CompanyObjects.ToList();
            var start = companyObjectDto.StartNum;
            var end = companyObjectDto.EndNum;
            bool IsInUse = false;
            var sb = new StringBuilder();

            foreach (var companyObject in companyObjects)
            {
                if (start > companyObject.StartNum && start < companyObject.EndNum)
                {
                    sb.AppendLine( $"Старт номерa е в номерацията  на обект {companyObject.Name}");
                    IsInUse = true;
                }
                if (end > companyObject.StartNum && end < companyObject.EndNum)
                {
                    sb.AppendLine($"Край номерa е в номерацията  на обект {companyObject.Name}");
                    IsInUse = true;
                }
            }
            if (IsInUse)
            {
                companyObjectDto.ErrorMassage = sb.ToString().Trim();
                companyObjectDto.IsValideObject = false;
            }
            return companyObjectDto;

        }


    }
}
