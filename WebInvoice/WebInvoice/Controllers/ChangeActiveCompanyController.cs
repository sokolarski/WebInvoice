using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Data.Repository.Repositories;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class ChangeActiveCompanyController : Controller
    {
        private readonly IAppDeletableEntityRepository<CompanyApp> companyAppRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ChangeActiveCompanyController(IAppDeletableEntityRepository<CompanyApp> companyAppRepository, UserManager<ApplicationUser> userManager)
        {
            this.companyAppRepository = companyAppRepository;
            this.userManager = userManager;
        }
        public async Task<IActionResult> ChangeCompanyAsync(string guid)
        {
            this.Response.Cookies.Append("Company", guid, new CookieOptions() { MaxAge = new TimeSpan(30,0,0,0) });
            var userConext = HttpContext.User;
            var userId = userManager.GetUserId(userConext);

            if (!string.IsNullOrEmpty(userId))
            {
                var companyList = companyAppRepository.All().Where(e => e.ApplicationUserId == userId);
                var activeCompanies = companyList.Where(e => e.IsActive == true).ToList();
                if (activeCompanies != null)
                {
                    if (activeCompanies.Count == 1)
                    {
                        var oldActive = activeCompanies.FirstOrDefault(e => e.IsActive == true);
                        oldActive.IsActive = false;
                        companyAppRepository.Update(oldActive);
                    }
                    else
                    {
                        foreach (var activeCompany in activeCompanies)
                        {
                            activeCompany.IsActive = false;
                            companyAppRepository.Update(activeCompany);
                        }
                    }
                }
           
                
                var newActive = companyList.Where(e => e.GUID == guid).FirstOrDefault();
                
                if (newActive == null)
                {
                    return BadRequest();
                }

                newActive.IsActive = true;
               
                companyAppRepository.Update(newActive);
                await companyAppRepository.SaveChangesAsync();

                return Redirect("/Home");
            }
            return BadRequest();
        }
    }
}
