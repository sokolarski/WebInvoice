using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.Partner;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    public class PartnerController : Controller
    {
        private readonly IPartnerService partnerService;

        public PartnerController(IPartnerService partnerService)
        {
            this.partnerService = partnerService;
        }

        public IActionResult Index()
        {
            var model = partnerService.GetAllPartners();
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
                await partnerService.Create(partnerDto);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
