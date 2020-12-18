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
        private readonly IAppDeletableEntityRepository<CompanyApp> companyAppRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public NavBarViewComponent(IAppDeletableEntityRepository<CompanyApp> companyAppRepository, UserManager<ApplicationUser> userManager)
        {
            this.companyAppRepository = companyAppRepository;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var userConext = HttpContext.User;
            var userId = userManager.GetUserId(userConext);
            if (!string.IsNullOrEmpty(userId))
            {

                var company= companyAppRepository.All().Where(e => e.ApplicationUserId == userId).FirstOrDefault();
                if (company != null)
                {
                   
                    var companyNavBar = new CompanyNavBar() { Id = company.Id, Name = company.CompanyName, GUID = company.GUID, IsActive = company.IsActive };
                    this.ViewBag.Companies = companyNavBar;
                    return View();
                }
            }


            return View();
        }
    }
}
