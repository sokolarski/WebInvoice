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
        private readonly IConnectionStringGenerator connectionStringGenerator;

        public CompanyService(IAppDeletableEntityRepository<CompanyApp> companyAppRepo, IConnectionStringGenerator connectionStringGenerator)
        {
            this.companyAppRepo = companyAppRepo;
            this.connectionStringGenerator = connectionStringGenerator;
        }
        public async Task<bool> CreateCompanyAsync(CompanyInputDto companyInputDto, string userId)
        {
            var GUIDstr = Guid.NewGuid().ToString();
            var connectionString = connectionStringGenerator.GetConnectionString(companyInputDto.Name, GUIDstr);

            var companyContext = await CreateCompanyDbAsync(connectionString, companyInputDto, GUIDstr);
            await CreateCompanyAppAsync(connectionString, companyInputDto, GUIDstr, userId);

            return true;
        }

        private async Task CreateCompanyAppAsync(string connectionString, CompanyInputDto companyInputDto, string GUIDstr, string userId)
        {
            var companyApp = new CompanyApp()
            {
                CompanyName = companyInputDto.Name,
                ConnStr = connectionString,
                GUID = GUIDstr,
                Description = companyInputDto.Description,
                ApplicationUserId = userId,
                IsActive = false,
            };

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
