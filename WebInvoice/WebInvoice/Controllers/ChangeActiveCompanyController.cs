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
        private readonly IAppDeletableEntityRepository<CompanyApp> appDb;
        private readonly UserManager<ApplicationUser> userManager;

        public ChangeActiveCompanyController(IAppDeletableEntityRepository<CompanyApp> appDb, UserManager<ApplicationUser> userManager)
        {
            this.appDb = appDb;
            this.userManager = userManager;
        }
        public async Task<IActionResult> ChangeCompanyAsync(string guid)
        {
            var userConext = HttpContext.User;
            var userId = userManager.GetUserId(userConext);
            if (!string.IsNullOrEmpty(userId))
            {

                var companyList = appDb.All().Where(e => e.ApplicationUserId == userId);
                var oldActive = companyList.Where(e => e.IsActive == true).FirstOrDefault();
                if (oldActive != null)
                {
                    oldActive.IsActive = false;
                    appDb.Update(oldActive);
                }
                
                var newActive = companyList.Where(e => e.GUID == guid).FirstOrDefault();
                
                if (newActive == null)
                {
                    return BadRequest();
                }

                newActive.IsActive = true;
               
                appDb.Update(newActive);
                await appDb.SaveChangesAsync();

                return Redirect("/Home");
            }
            return BadRequest();
        }
    }
}
