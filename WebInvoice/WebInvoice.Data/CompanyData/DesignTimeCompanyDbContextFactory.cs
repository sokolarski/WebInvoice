using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebInvoice.Data.CompanyData
{
    public class DesignTimeCompanyDbContextFactory:IDesignTimeDbContextFactory<CompanyDbContext>
    {
        public CompanyDbContext CreateDbContext(string[] args)
        {
            //var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", false, true).Build();

            var builder = new DbContextOptionsBuilder<CompanyDbContext>();

           // var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=CompanyDefaultDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new CompanyDbContext(builder.Options);
        }
    }
}
