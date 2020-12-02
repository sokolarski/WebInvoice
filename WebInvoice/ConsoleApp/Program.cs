using Microsoft.EntityFrameworkCore;
using System;
using WebInvoice.Data;
using WebInvoice.Data.CompanyData;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var ne = new DesignTimeCompanyDbContextFactory();
            using (var db = ne.CreateDbContext(null))
            {
                db.Database.Migrate();

            }
        }
    }
}
