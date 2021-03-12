using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        public async Task<bool> CreateCompanyAsync(CompanyDto companyInputDto, string userId)
        {
            var companyGUID = Guid.NewGuid().ToString();
            var objectGUID = Guid.NewGuid().ToString();
            var connectionString = stringGenerator.GetConnectionString(companyInputDto.Name, companyGUID);


            try
            {
                using var transaction = await companyAppRepo.Context.Database.BeginTransactionAsync();

                await CreateCompanyAppAsync(connectionString, companyInputDto, companyGUID, objectGUID, userId);
                var isSuccessfullyCreated = await CreateCompanyDbAsync(connectionString, companyInputDto, companyGUID, objectGUID);
                if (!isSuccessfullyCreated)
                {
                    throw new Exception();
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {

                return false;
            }


            return true;
        }

        private async Task CreateCompanyAppAsync(string connectionString, CompanyDto companyInputDto, string companyGUID, string objectGuid, string userId)
        {
            var obj = new CompanyAppObject()
            {
                ObjectName = "Стандарт",
                ObjectSlug = "standart",
                GUID = objectGuid,
                IsActive = true
            };

            var companyAppSlug = stringGenerator.GenerateSlug(companyInputDto.Name);
            var companyApp = new CompanyApp()
            {
                CompanyName = companyInputDto.Name,
                ConnStr = connectionString,
                GUID = companyGUID,
                Description = companyInputDto.Description,
                CompanySlug = companyAppSlug,
                ApplicationUserId = userId,
                IsActive = false,
                IsVatRegistered = companyInputDto.IsVatRegistered,
            };
            companyApp.CompanyAppObjects.Add(obj);

            await companyAppRepo.AddAsync(companyApp);
            await companyAppRepo.SaveChangesAsync();
        }

        private async Task<bool> CreateCompanyDbAsync(string connectionString, CompanyDto companyInputDto, string companyGUID, string objectGUID)
        {
            var options = new DbContextOptionsBuilder<CompanyDbContext>();
            options.UseSqlServer(connectionString);
            var companyContext = new CompanyDbContext(options.Options);

            using (companyContext)
            {
                try
                {

                    await companyContext.Database.MigrateAsync();

                    using var secondTransaction = await companyContext.Database.BeginTransactionAsync();

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
                        GUID = companyGUID,
                    };

                    var obj = new CompanyObject()
                    {
                        Name = "Стандарт",
                        City = companyInputDto.City,
                        StartNum = 1,
                        EndNum = 9999999999,
                        IsActive = true,
                        GUID = objectGUID,
                    };
                    company.CompanyObjects.Add(obj);

                    var employee = new Employee()
                    {
                        FullName = companyInputDto.MOL,
                        IsActive = true,
                    };

                    company.Employees.Add(employee);

                    await companyContext.Companies.AddAsync(company);
                    await companyContext.SaveChangesAsync();

                    var seeder = new SeedData(companyContext);
                    await seeder.SeedAsync(companyInputDto.IsVatRegistered);

                    await secondTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    companyContext.Database.EnsureDeleted();
                    return false;

                }
               
            }

            return true;
        }

    }
}
