using Microsoft.AspNetCore.Authorization;
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
            var userConext = HttpContext.User;
            var userId = userManager.GetUserId(userConext);
            if (!string.IsNullOrEmpty(userId))
            {

                var companyList = companyAppRepository.All().Where(e => e.ApplicationUserId == userId);
                var oldActive = companyList.Where(e => e.IsActive == true).FirstOrDefault();
                if (oldActive != null)
                {
                    oldActive.IsActive = false;
                    companyAppRepository.Update(oldActive);
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
