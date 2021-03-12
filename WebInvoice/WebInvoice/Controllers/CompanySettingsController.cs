using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.Company;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class CompanySettingsController : Controller
    {
        private readonly ICompanySettingsService companySettingsService;
        private readonly IUserCompanyTemp userCompanyTemp;

        public CompanySettingsController(ICompanySettingsService companySettingsService, IUserCompanyTemp userCompanyTemp)
        {
            this.companySettingsService = companySettingsService;
            this.userCompanyTemp = userCompanyTemp;
        }
        public async Task<IActionResult> Index()
        {
            var model = await this.companySettingsService.GetCompanyInfo();
            return View(model);
        }

        public async Task<IActionResult> Edit()
        {
            var model = await this.companySettingsService.GetCompanyInfo();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyDto companyDto)
        {
            if (ModelState.IsValid)
            {
                var IsSuccessfully =await companySettingsService.Edit(companyDto);
                if (!IsSuccessfully)
                {
                    return View("NotFoundItem");
                }
                return RedirectToAction("Index", new { company = companyDto.CompanySlug, companyObject = userCompanyTemp.CompanyObjectSlug });
            }
            return RedirectToAction("Index");
        }

        public IActionResult Database()
        {
            return View();
        }
        public async Task<IActionResult> ApplyMigration()
        {
            await companySettingsService.ApplyMigration();
            return RedirectToAction("Index");
        }
    }
}
