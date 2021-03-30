using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebInvoice.Data.CompanyData
{
    public class DesignTimeCompanyDbContextFactory:IDesignTimeDbContextFactory<CompanyDbContext>
    {
        public CompanyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CompanyDbContext>();

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true).Build();
            builder.UseSqlServer(configuration.GetConnectionString("CompanyDefaultConnection"));
            //builder.UseSqlServer(@"Server=DESKTOP-K71BNIK\SQLEXPRESS;Database=WebInvoice-CompanyDefaultDb;Trusted_Connection=false;MultipleActiveResultSets=false;User ID=webinvoice; Password=Sokolarski860514;");

            return new CompanyDbContext(builder.Options);
        }
    }
}
