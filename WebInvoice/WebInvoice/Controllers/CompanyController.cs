using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Dto.Company;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;
        private readonly UserManager<ApplicationUser> userManager;

        public CompanyController(ICompanyService companyService, UserManager<ApplicationUser> userManager)
        {
            this.companyService = companyService;
            this.userManager = userManager;
        }
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
        public async Task<IActionResult> Create(CompanyDto companyInputDto)
        {
            if (!ModelState.IsValid)
            {
                return View(companyInputDto);
            }

            var userConext = HttpContext.User;
            var userId = userManager.GetUserId(userConext);

            var isCreated = await companyService.CreateCompanyAsync(companyInputDto, userId);
            if (!isCreated)
            {
                return View("NotFoundItem");
            }
            return RedirectToAction("Created", new { companyInputDto.Name });
        }

        public IActionResult Created(string name)
        {
            this.ViewBag.CompanyName = name;
            return View();
        }

    }
}
