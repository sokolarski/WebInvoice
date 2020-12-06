using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Models;
using WebInvoice.Models.Home;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext applicationDbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            this.applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            var listOfCompanies = applicationDbContext.CompanyApps
                .Select(c => new CompanyNavBar() { Id = c.Id, Name = c.CompanyName, IsActive = c.IsActive })
                .ToList();
            var listOfCompanyViewModel = new CompanyListModels() { Companies=listOfCompanies };
            return View(listOfCompanyViewModel);
        }
        public IActionResult Menu()
        {
            var listOfCompanies = applicationDbContext.CompanyApps
                .Select(c => new CompanyNavBar() { Id = c.Id, Name = c.CompanyName, IsActive = c.IsActive })
                .ToList();
            var listOfCompanyViewModel = new CompanyListModels() { Companies = listOfCompanies };
            //return View(listOfCompanyViewModel);
            return PartialView("_CompanyNavBarPartial", listOfCompanyViewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
