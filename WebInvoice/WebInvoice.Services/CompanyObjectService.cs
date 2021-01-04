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
        private readonly IAppRepository<CompanyApp> appRepository;
        private readonly ICompanyRepository<Company> companyRepository;
        private readonly IUserCompanyTemp userCompanyTemp;
        private readonly IStringGenerator stringGenerator;

        public CompanyObjectService(IAppRepository<CompanyApp> appRepository,
                                    ICompanyRepository<Company> companyRepository,
                                    IUserCompanyTemp userCompanyTemp,
                                    IStringGenerator stringGenerator)
        {
            this.appRepository = appRepository;
            this.companyRepository = companyRepository;
            this.userCompanyTemp = userCompanyTemp;
            this.stringGenerator = stringGenerator;
        }

        public async Task<CompanyObjectListDto> GetAllCompanyObjects()
        {
            var resultCompanyObjects = new CompanyObjectListDto();

            var companyObects = await companyRepository.AllAsNoTracking().OrderBy(c => c.Id)
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
                   ObjectGUID = co.GUID,
                   CountOfDocuments = co.VatDocuments.Count,
               })
                ).LastAsync();

            resultCompanyObjects.CompanyObjects = companyObects.ToList();
            return resultCompanyObjects;
        }

        public async Task<CompanyObjectDto> GetById(int id)
        {
            var companyObect = await companyRepository.AllAsNoTracking().OrderBy(c => c.Id)
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
                  ObjectGUID = co.GUID,
                  CountOfDocuments = co.VatDocuments.Count,
              })
                ).LastAsync();

            return companyObect.FirstOrDefault();
        }

        public async Task Edit(CompanyObjectDto companyObjectDto)
        {
            if (companyObjectDto.IsActive == true)
            {
                await SetAllObjectNonActive();
            }
            var company = await companyRepository.All().Include(x => x.CompanyObjects).OrderBy(c => c.Id).LastAsync();
            var companyObject = company.CompanyObjects.Where(co => co.Id == companyObjectDto.Id).FirstOrDefault();

            if (company.GUID == userCompanyTemp.CompanyGUID)
            {
                var companyApp = await appRepository.All().Include(c => c.CompanyAppObjects).Where(c => c.GUID == company.GUID).FirstOrDefaultAsync();
                var companyAppObject = companyApp.CompanyAppObjects.Where(co => co.GUID == companyObject.GUID).FirstOrDefault();


                var companyObjectSlug = stringGenerator.GenerateSlug(companyObjectDto.Name);
                companyObjectDto.CompanyObjectSlug = companyObjectSlug;
                companyAppObject.ObjectName = companyObjectDto.Name;
                companyAppObject.ObjectSlug = companyObjectSlug;
                companyAppObject.IsActive = companyObjectDto.IsActive;
                appRepository.Update(companyApp);

                companyObject.Name = companyObjectDto.Name;
                companyObject.City = companyObjectDto.City;
                companyObject.StartNum = companyObjectDto.StartNum;
                companyObject.EndNum = companyObjectDto.EndNum;
                companyObject.IsActive = companyObjectDto.IsActive;
                companyObject.Description = companyObjectDto.Description;

                companyRepository.Update(company);
                await companyRepository.SaveChangesAsync();
                await appRepository.SaveChangesAsync();

            }

        }
        public async Task Delete(CompanyObjectDto companyObjectDto)
        {
            var company = await companyRepository.All().Include(x => x.CompanyObjects).OrderBy(c => c.Id).LastAsync();
            var companyObject = company.CompanyObjects.Where(co => co.Id == companyObjectDto.Id && co.GUID == companyObjectDto.ObjectGUID).FirstOrDefault();

            if (companyObject.VatDocuments.Count == 0 && companyObject.NonVatDocuments.Count == 0)
            {
                if (company.GUID == userCompanyTemp.CompanyGUID)
                {
                    var companyApp = await appRepository.All().Include(c => c.CompanyAppObjects).Where(c => c.GUID == company.GUID).FirstOrDefaultAsync();
                    var companyAppObject = companyApp.CompanyAppObjects.Where(co => co.GUID == companyObject.GUID).FirstOrDefault();


                    companyRepository.Context.CompanyObjects.Remove(companyObject);
                    appRepository.Context.CompanyAppObjects.Remove(companyAppObject);
                    //companyRepository.Update(company);
                    //appRepository.Update(companyApp);
                    await companyRepository.SaveChangesAsync();
                    await appRepository.SaveChangesAsync();
                }
            }

        }
        public async Task Create(CompanyObjectDto companyObjectDto)
        {
            if (companyObjectDto.IsActive == true)
            {
                await SetAllObjectNonActive();
            }

            var companyObjectGUID = Guid.NewGuid().ToString();

            var companyAppObject = new CompanyAppObject()
            {
                ObjectName = companyObjectDto.Name,
                GUID = companyObjectGUID,
                IsActive = companyObjectDto.IsActive,
                ObjectSlug = stringGenerator.GenerateSlug(companyObjectDto.Name),
            };

            var companyApp = appRepository.All().Where(c => c.GUID == userCompanyTemp.CompanyGUID).FirstOrDefault();
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

            var company = await companyRepository.All().OrderBy(c => c.Id).LastAsync();
            company.CompanyObjects.Add(companyObject);

            await appRepository.SaveChangesAsync();
            await companyRepository.SaveChangesAsync();

        }

        public async Task ValidateObjectDucumentRange(CompanyObjectDto companyObjectDto)
        {
            var company = await companyRepository.AllAsNoTracking().Include(x => x.CompanyObjects).OrderBy(c => c.Id).LastAsync();
            var companyObjects = company.CompanyObjects.ToList();



            var start = companyObjectDto.StartNum;
            var end = companyObjectDto.EndNum;
            bool IsInUse = false;

            if (companyObjectDto.Id != 0)
            {
                var currentObject = companyObjects.Where(o => o.Id == companyObjectDto.Id).FirstOrDefault();
                if (currentObject != null)
                {
                    var firstDocument = companyRepository.Context.VatDocuments.Where(vd => vd.CompanyObjectId == companyObjectDto.Id).Select(vd => vd.Id).FirstOrDefault();
                    var lastDocument = companyRepository.Context.VatDocuments.Where(vd => vd.CompanyObjectId == companyObjectDto.Id).OrderBy(vd => vd.Id).Select(vd => vd.Id).LastOrDefault();
                    if (firstDocument != 0 && lastDocument != 0)
                    {
                        if (start > firstDocument || end < lastDocument)
                        {
                            IsInUse = true;
                            companyObjectDto.ErrorMassages.Add($"Има издадени документи от номер{firstDocument} до {lastDocument}!");
                        }

                    }
                    companyObjects.Remove(currentObject);
                }
            }

            if (start > end)
            {
                IsInUse = true;
                companyObjectDto.ErrorMassages.Add("Старт номера трябва да бъде по-голям от край номера");
            }

            foreach (var companyObject in companyObjects)
            {
                if (start >= companyObject.StartNum && start <= companyObject.EndNum)
                {
                    companyObjectDto.ErrorMassages.Add($"Старт номерa е в номерацията  на обект {companyObject.Name} с начало: {companyObject.StartNum} и край: {companyObject.EndNum}!");
                    IsInUse = true;
                }
                if (end >= companyObject.StartNum && end <= companyObject.EndNum)
                {
                    companyObjectDto.ErrorMassages.Add($"Край номерa е в номерацията  на обект {companyObject.Name} с начало: {companyObject.StartNum} и край: {companyObject.EndNum}!");
                    IsInUse = true;
                }
                if (companyObject.Name == companyObjectDto.Name)
                {
                    companyObjectDto.ErrorMassages.Add($"Съществува обект с име {companyObject.Name}!");
                    IsInUse = true;
                }
            }
            if (!IsInUse)
            {
                companyObjectDto.IsValidObjectDocumentRange = true;
            }
        }

        public async Task SetAllObjectNonActive()
        {
            var company = await companyRepository.All().Include(x => x.CompanyObjects).OrderBy(c => c.Id).LastAsync();

            if (company.GUID == userCompanyTemp.CompanyGUID)
            {
                foreach (var obj in company.CompanyObjects)
                {
                    obj.IsActive = false;
                }
            }

            var companyApp = await appRepository.All().Include(c => c.CompanyAppObjects).Where(c => c.GUID == company.GUID).FirstOrDefaultAsync();

            if (companyApp.GUID == userCompanyTemp.CompanyGUID)
            {
                foreach (var obj in companyApp.CompanyAppObjects)
                {
                    obj.IsActive = false;
                }
            }
            companyRepository.Update(company);
            appRepository.Update(companyApp);

            await companyRepository.SaveChangesAsync();
            await appRepository.SaveChangesAsync();


        }


    }
}
