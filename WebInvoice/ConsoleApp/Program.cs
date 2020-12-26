using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.CompanyData;
using WebInvoice.Data.SeedData;
using WebInvoice.Services;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
           
            //var stringGen = new ConnectionStringGenerator();
            //var str = stringGen.GetConnectionString("милко-сокоЛарски-86 оод", "dmd584vf6v2fvf6s8fv86");
            //Console.WriteLine(str);


            //var ne = new DesignTimeCompanyDbContextFactory();
            //using (var db = ne.CreateDbContext(null))
            //{
            //    db.Database.Migrate();
            //    var seeder = new SeedData(db);
            //    await seeder.SeedAsync();

            //}

            
        }
    }
}
