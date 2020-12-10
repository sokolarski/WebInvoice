using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.Company;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CompanyInputDto companyInputDto)
        {
            return RedirectToAction("Created");
        }

        public IActionResult Created()
        {
            return View();
        }
    }
}
