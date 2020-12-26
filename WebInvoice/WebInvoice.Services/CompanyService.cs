using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Data.SeedData;
using WebInvoice.Dto.Company;

namespace WebInvoice.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IAppDeletableEntityRepository<CompanyApp> companyAppRepo;
        private readonly IStringGenerator stringGenerator;

        public CompanyService(IAppDeletableEntityRepository<CompanyApp> companyAppRepo, IStringGenerator connectionStringGenerator)
        {
            this.companyAppRepo = companyAppRepo;
            this.stringGenerator = connectionStringGenerator;
        }
        public async Task<bool> CreateCompanyAsync(CompanyInputDto companyInputDto, string userId)
        {
            var GUIDstr = Guid.NewGuid().ToString();
            var connectionString = stringGenerator.GetConnectionString(companyInputDto.Name, GUIDstr);

            var companyContext = await CreateCompanyDbAsync(connectionString, companyInputDto, GUIDstr);
            await CreateCompanyAppAsync(connectionString, companyInputDto, GUIDstr, userId);

            return true;
        }

        private async Task CreateCompanyAppAsync(string connectionString, CompanyInputDto companyInputDto, string GUIDstr, string userId)
        {
            var obj = new CompanyAppObject()
            {
                ObjectName = "Стандарт",
                ObjectSlug = "standart",
                IsActive = true
            };

            var companyAppSlug = stringGenerator.GenerateSlug(companyInputDto.Name);
            var companyApp = new CompanyApp()
            {
                CompanyName = companyInputDto.Name,
                ConnStr = connectionString,
                GUID = GUIDstr,
                Description = companyInputDto.Description,
                CompanySlug = companyAppSlug,
                ApplicationUserId = userId,
                IsActive = false,
            };
            companyApp.CompanyAppObjects.Add(obj);

            await companyAppRepo.AddAsync(companyApp);
            await companyAppRepo.SaveChangesAsync();
        }

        private async Task<CompanyDbContext> CreateCompanyDbAsync(string connectionString, CompanyInputDto companyInputDto, string GuidStr)
        {
            var options = new DbContextOptionsBuilder<CompanyDbContext>();
            options.UseSqlServer(connectionString);
            var companyContext = new CompanyDbContext(options.Options);
            await companyContext.Database.MigrateAsync();
            using (companyContext)
            {

                var company = new Company()
                {
                    Name = companyInputDto.Name,
                    Address = companyInputDto.Address,
                    City = companyInputDto.City,
                    Country = companyInputDto.Country,
                    EIK = companyInputDto.EIK,
                    VatId = companyInputDto.VatId,
                    Email = companyInputDto.Email,
                    MOL = companyInputDto.MOL,
                    IsVatRegistered = companyInputDto.IsVatRegistered,
                    LogoPath = companyInputDto.LogoPath,
                    IsActive = false,
                    GUID = GuidStr,
                };

                var obj = new CompanyObject()
                {
                    Name = "Стандарт",
                    City = companyInputDto.City,
                    StartNum = 1,
                    EndNum = 9999999999,
                    IsActive = true,
                };
                company.CompanyObjects.Add(obj);

                await companyContext.Companies.AddAsync(company);
                await companyContext.SaveChangesAsync();
                var seeder = new SeedData(companyContext);
                await seeder.SeedAsync();
            }

            return companyContext;
        }

        public bool EditCompany(CompanyInputDto companyInputDto)
        {
            throw new NotImplementedException();
        }
    }
}
