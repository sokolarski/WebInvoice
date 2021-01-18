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
    public class PartnerController : Controller
    {
        private readonly IPartnerService partnerService;

        public PartnerController(IPartnerService partnerService)
        {
            this.partnerService = partnerService;
        }


        public async Task<IActionResult> Index(string findByName, int? pageNumber)
        {

            if (findByName != null)
            {
                var result = partnerService.FindPartner(findByName);
                this.ViewBag.findName = findByName;
                return View(result);
            }
            var model = await partnerService.GetPaginatedPartnerAsync(pageNumber ?? 1);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PartnerDto partnerDto)
        {
            if (ModelState.IsValid)
            {

                var companyId = await partnerService.Create(partnerDto);
                var companyName = partnerDto.Name;
                return RedirectToAction("ConfirmCreate", new { companyId = companyId, companyName = companyName });
            }
            return View();
        }

        public IActionResult ConfirmCreate(int companyId, string companyName)
        {
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View();
        }

        public IActionResult Search(string name)
        {

            var result = partnerService.FindPartner(name);
            return Json(result);

        }
    }
}
