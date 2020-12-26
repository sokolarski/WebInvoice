using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Data.Repository.Repositories;

namespace WebInvoice.Services
{
    public class CompanyDbContextConnStrProvider : ICompanyDbContextConnStrProvider
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CompanyDbContextConnStrProvider(ApplicationDbContext applicationDbContext,
                                                UserManager<ApplicationUser> userManager,
                                                IHttpContextAccessor httpContextAccessor)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetConnectionString()
        {

            var user = httpContextAccessor.HttpContext.User;
            var userId = userManager.GetUserId(user);

            var companies = applicationDbContext.CompanyApps.Where(c => c.ApplicationUserId == userId && c.IsActive == true).ToList();

            if (companies.Count() != 1 && companies != null)
            {
                foreach (var company in companies)
                {
                    company.IsActive = false;
                }
                applicationDbContext.SaveChanges();
                return null;
            }
            var connString = companies.FirstOrDefault().ConnStr;
            return connString;
        }
    }
}
