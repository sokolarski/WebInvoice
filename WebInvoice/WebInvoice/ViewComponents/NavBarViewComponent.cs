using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Models.NavBar;

namespace WebInvoice.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly IAppDeletableEntityRepository<CompanyApp> appDb;
        private readonly UserManager<ApplicationUser> userManager;

        public NavBarViewComponent(IAppDeletableEntityRepository<CompanyApp> appDb, UserManager<ApplicationUser> userManager)
        {
            this.appDb = appDb;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userConext = HttpContext.User;
            var userId = userManager.GetUserId(userConext);
            if (!string.IsNullOrEmpty(userId))
            {

                var companyList = appDb.AllAsNoTrackingWithDeleted().Where(e => e.ApplicationUserId == userId).ToList();
                if (companyList.Count > 0)
                {
                    var companyNavBars = companyList.Select(c => new CompanyNavBar() { Id = c.Id, Name = c.CompanyName, GUID = c.GUID, IsActive = c.IsActive });
                    this.ViewBag.Companies = companyNavBars;
                    return View();
                }
            }


            return View();
        }
    }
}
