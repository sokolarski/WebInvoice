using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.CompanyData;
using WebInvoice.Data.SeedData;
using WebInvoice.Services;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            //var txt = Console.ReadLine();
            //DateTime result;
            /////var time = DateTime.TryParseExact(txt, "dd.MM.yyyy", new CultureInfo("bg-BG"), DateTimeStyles.None, out result);
            //var time = DateTime.TryParse(txt, new CultureInfo("bg-BG"), DateTimeStyles.None, out result);
            //if (time)
            //{
            //    Console.WriteLine(result.ToString("dd.MM.yyyy"));
            //}
            //else
            //{
            //    Console.WriteLine("error");
            //}

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
