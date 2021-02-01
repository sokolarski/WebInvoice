using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData;
using WebInvoice.Data.CompanyData.Models;

namespace WebInvoice.Data.SeedData
{
    public class SeedData
    {
        private CompanyDbContext companyDbContext;
        public SeedData(CompanyDbContext companyDbContext)
        {
            this.companyDbContext = companyDbContext;
        }

        public async Task SeedAsync()
        {
            using (companyDbContext)
            {
                await SeedQuantityTypesAsync(companyDbContext);
                await SeedPaymentTypesAsync(companyDbContext);
                await SeedVatTypesAsync(companyDbContext);
                await companyDbContext.SaveChangesAsync();
            }
        }

    

        private async Task SeedVatTypesAsync(CompanyDbContext companyDbContext)
        {
            var types = new[] {
                new VatType(){ Name="А", Description="Група А", Percantage= 0, IsActive=false},
                new VatType(){ Name="Б", Description="Група Б", Percantage= 20, IsActive=true},
                new VatType(){ Name="В", Description="Група В течни горива", Percantage= 20, IsActive=false},
                new VatType(){ Name="Г", Description="Група Г", Percantage= 9, IsActive=false},
            };
            await companyDbContext.VatTypes.AddRangeAsync(types);
            await companyDbContext.SaveChangesAsync();
        }

        private async Task SeedPaymentTypesAsync(CompanyDbContext companyDbContext)
        {
            var types = new[] {
            new PaymentType(){ Name="Брой", Description="Брой в лева", IsActiv=true , RequireBankAccount=false },
            new PaymentType(){ Name="Банка", Description="Превод по сметка", IsActiv=false , RequireBankAccount=true },
            new PaymentType(){ Name="Карта", Description="Дебитна или кредитна карта", IsActiv=false , RequireBankAccount=false},
            new PaymentType(){ Name="Прихващане", Description="Прихващане", IsActiv=false, RequireBankAccount=false },
            };
            await companyDbContext.PaymentTypes.AddRangeAsync(types);
            await companyDbContext.SaveChangesAsync();
        }

        private async Task SeedQuantityTypesAsync(CompanyDbContext companyDbContext)
        {
            var types = new[] {
            new QuantityType(){ Type="бр.", Description="брой", IsActive=true },
            new QuantityType(){ Type="кг.", Description="килограм", IsActive=false },
            new QuantityType(){ Type="л.", Description="литър", IsActive=false },
            };
            await companyDbContext.QuantityTypes.AddRangeAsync(types);
            await companyDbContext.SaveChangesAsync();
        }
    }
}

