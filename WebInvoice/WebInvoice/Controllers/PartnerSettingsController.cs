using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.Partner;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class PartnerSettingsController : Controller
    {
        private readonly IPartnerSettingService partnerSettingService;

        public PartnerSettingsController(IPartnerSettingService partnerSettingService)
        {
            this.partnerSettingService = partnerSettingService;
        }

        public async Task<IActionResult> Index(int companyId)
        {
            var model = await partnerSettingService.GetPartnerByIdAsync(companyId);
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = model.Name;
            return View(model);
        }

        public async Task<IActionResult> Edit(int companyId, string companyName)
        {
            var model = await partnerSettingService.GetPartnerByIdAsync(companyId);
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (PartnerDto partnerDto, int companyId, string companyName)
        {
            if (ModelState.IsValid)
            {
                await partnerSettingService.Edit(partnerDto);
                this.ViewBag.companyId = companyId;
                this.ViewBag.companyName = companyName;
                return RedirectToAction("Index", new { companyId = partnerDto.Id });
            }
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View(partnerDto);
        }
    }
}
