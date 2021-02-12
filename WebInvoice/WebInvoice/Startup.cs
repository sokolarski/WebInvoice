using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Data.AppData.Repo;
using WebInvoice.Data.CompanyData;
using WebInvoice.Data.Repository;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Middleware;
using WebInvoice.Services;

namespace WebInvoice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews(option => option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            

            services.AddScoped<IUserCompanyTemp, UserCompanyTemp>();

            //Add CompanyDbContext by user
            services.AddDbContext<CompanyDbContext>((serviceProvider, options) =>
                            options.UseSqlServer(serviceProvider.GetService<IUserCompanyTemp>().ConnectionString));

            // Data repositories
            services.AddScoped(typeof(IAppDeletableEntityRepository<>), typeof(AppDeletableEntityRepository<>));
            services.AddScoped(typeof(IAppRepository<>), typeof(AppRepository<>));

            services.AddScoped(typeof(ICompanyDeletableEntityRepository<>), typeof(CompanyDeletableEntityRepository<>));
            services.AddScoped(typeof(ICompanyRepository<>), typeof(CompanyRepository<>));
            services.AddScoped<ICompanyDbQueryRunner, CompanyDbQueryRunner>();


            //Services
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IStringGenerator, StringGenerator>();
            services.AddTransient<ICompanyObjectService, CompanyObjectService>();
            services.AddTransient<ICompanySettingsService, CompanySettingsService>();
            services.AddTransient<IVatTypeService, VatTypeService>();
            services.AddTransient<IBankAccountService, BankAccountService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IPaymentTypeService, PaymentTypeService>();
            services.AddTransient<IPartnerService, PartnerService>();
            services.AddTransient<IPartnerSettingService, PartnerSettingService>();
            services.AddTransient<IPartnerEmployeeService, PartnerEmployeeService>();
            services.AddTransient<IPartnerBankAccountService, PartnerBankAccountService>();
            services.AddTransient<IQuantityTypeService, QuantityTypeService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IVatDocumentService, VatDocumentService>();
            

          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<GetCompanyTempDataMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                       "company",
                       "{company}/{companyObject}/{controller}/{action}/{id?}");
                endpoints.MapControllerRoute(
                    "areaRoute",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
