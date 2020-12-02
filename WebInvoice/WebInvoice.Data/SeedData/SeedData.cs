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
        private CompanyDbContext CompanyDbContext;
        public SeedData()
        {
            this.CompanyDbContext = new DesignTimeCompanyDbContextFactory().CreateDbContext(null);
        }

        public async Task SeedAsync()
        {
            using (CompanyDbContext)
            {
                await SeedQuantityTypesAsync(CompanyDbContext);
                await SeedPaymentTypesAsync(CompanyDbContext);
                await SeedVatTypesAsync(CompanyDbContext);
                await SeedReasonsAsync(CompanyDbContext);
                await CompanyDbContext.SaveChangesAsync();
            }
        }

        private async Task SeedReasonsAsync(CompanyDbContext companyDbContext)
        {
            var listOfVats = companyDbContext.VatTypes.ToList();
            var types = new[] {
                new Reason(){ Name="Стандартно Основане", Description="Начисляване на ддс 20%", VatType = listOfVats.Where(v => v.Name == "Б").First(), IsActive=true },
                new Reason(){ Name="Неначисляване на ддс", Description="Начисляване на ддс 0%", VatType = listOfVats.Where(v => v.Name == "А").First(), IsActive=false },
            };

           await companyDbContext.Reasons.AddRangeAsync(types);
           await companyDbContext.SaveChangesAsync();
        }

        private async Task SeedVatTypesAsync(CompanyDbContext companyDbContext)
        {
            var types = new[] {
                new VatType(){ Name="А", Description="Група А", Percantage= 0, IsActive=false},
                new VatType(){ Name="Б", Description="Група Б", Percantage= 20, IsActive=true},
                new VatType(){ Name="В", Description="Група В", Percantage= 20, IsActive=false},
                new VatType(){ Name="Г", Description="Група Г", Percantage= 9, IsActive=false},
            };
            await companyDbContext.VatTypes.AddRangeAsync(types);
            await companyDbContext.SaveChangesAsync();
        }

        private async Task SeedPaymentTypesAsync(CompanyDbContext companyDbContext)
        {
            var types = new[] {
            new PaymentType(){ Name="Брой", Description="Брой в лева", IsActiv=true },
            new PaymentType(){ Name="Банка", Description="Превод по сметка", IsActiv=false },
            new PaymentType(){ Name="Карта", Description="Дебитна или кредитна карта", IsActiv=false },
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
    
