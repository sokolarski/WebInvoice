using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.Company;

namespace WebInvoice.Services
{
    public class CompanySettingsService : ICompanySettingsService
    {
        private readonly Data.Repository.Repositories.ICompanyRepository<Company> companyRepository;
        private readonly IAppDeletableEntityRepository<CompanyApp> companyAppRepository;
        private readonly IStringGenerator stringGenerator;
        private readonly IVatTypeService vatTypeService;
        private readonly IProductService productService;

        public CompanySettingsService(ICompanyRepository<Company> companyRepository, 
                                        IAppDeletableEntityRepository<CompanyApp> companyAppRepository,
                                        IStringGenerator stringGenerator,
                                        IVatTypeService vatTypeService,
                                        IProductService productService)
        {
            this.companyRepository = companyRepository;
            this.companyAppRepository = companyAppRepository;
            this.stringGenerator = stringGenerator;
            this.vatTypeService = vatTypeService;
            this.productService = productService;
        }

        public async Task<CompanyDto> GetCompanyInfo()
        {
            var company = await companyRepository.AllAsNoTracking().OrderBy(c => c.Id).LastAsync();
            var companyApp = this.companyAppRepository.AllAsNoTracking().Where(c => c.GUID == company.GUID).FirstOrDefault();
            
            var companyInfo = new CompanyDto()
            {
                Name = company.Name,
                Address = company.Address,
                City = company.City,
                Country = company.Country,
                Description = company.Description,
                EIK = company.EIK,
                Email = company.Email,
                IsVatRegistered = company.IsVatRegistered,
                LogoPath = company.LogoPath,
                MOL = company.MOL,
                VatId = company.VatId,
                IsActive = companyApp.IsActive,
            };
            return companyInfo;
        }

        public async Task<CompanyDto> GetCompanyInfoById(int id)
        {
            var company = await companyRepository.GetByIdAsync(id);
            
            var companyInfo = new CompanyDto()
            {
                Name = company.Name,
                Address = company.Address,
                City = company.City,
                Country = company.Country,
                Description = company.Description,
                EIK = company.EIK,
                Email = company.Email,
                IsVatRegistered = company.IsVatRegistered,
                LogoPath = company.LogoPath,
                MOL = company.MOL,
                VatId = company.VatId
            };
            return companyInfo;
        }

        public async Task Edit(CompanyDto companyDto)
        {
            var company = await companyRepository.All().OrderBy(c => c.Id).LastAsync();
            var companyApp = this.companyAppRepository.All().Where(c => c.GUID == company.GUID).FirstOrDefault();
            if (company.IsVatRegistered && !companyDto.IsVatRegistered)
            {
                var vatTypeId = await vatTypeService.SetCorrectVatTypeOnNonVatRegisteredCompanyAsync();
                await productService.SetAllProductToVatType(vatTypeId);
            }

            company.Name = companyDto.Name;
            company.Address = companyDto.Address;
            company.City = companyDto.City;
            company.Country = companyDto.Country;
            company.Description = companyDto.Description;
            company.EIK = companyDto.EIK;
            company.Email = companyDto.Email;
            company.IsVatRegistered = companyDto.IsVatRegistered;
            company.LogoPath = companyDto.LogoPath;
            company.MOL = companyDto.MOL;
            company.VatId = companyDto.VatId;

            companyRepository.Update(company);

            if (companyDto.IsActive == true)
            {
                var allCompanyApp = this.companyAppRepository.All().Where(c => c.ApplicationUserId == companyApp.ApplicationUserId);
                foreach (var comp in allCompanyApp)
                {
                    comp.IsActive = false;
                    companyAppRepository.Update(comp);
                }
            }

            var slug = stringGenerator.GenerateSlug(companyDto.Name);
            companyDto.CompanySlug = slug;
            companyApp.CompanyName = companyDto.Name;
            companyApp.CompanySlug = slug;
            companyApp.Description = companyDto.Description;
            companyApp.IsActive = companyDto.IsActive;
            companyApp.IsVatRegistered = companyDto.IsVatRegistered;

            companyAppRepository.Update(companyApp);

            await companyRepository.SaveChangesAsync();
            await companyAppRepository.SaveChangesAsync();
        }

        public async Task ApplyMigration()
        {
            await companyRepository.Context.Database.MigrateAsync();
        }
    }
}
