using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace WebInvoice.Data.CompanyData
{
    public class DesignTimeCompanyDbContextFactory:IDesignTimeDbContextFactory<CompanyDbContext>
    {
        public CompanyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CompanyDbContext>();

            builder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=WebInvoice-CompanyDefaultDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new CompanyDbContext(builder.Options);
        }
    }
}
