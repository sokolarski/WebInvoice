using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.Reason;
using WebInvoice.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class ReasonController : Controller
    {
        private readonly IReasonService reasonService;
        private readonly IVatTypeService vatTypeService;

        public ReasonController(IReasonService reasonService, IVatTypeService vatTypeService)
        {
            this.reasonService = reasonService;
            this.vatTypeService = vatTypeService;
        }
        public IActionResult Index()
        {
            var model = reasonService.GetAllCompanyReason();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var reason = reasonService.GetById(id);
            var vatTypes = vatTypeService.GetAllView();
            this.ViewBag.SelectVatType = new SelectList(vatTypes, "Id", "Name", vatTypes.Where(v => v.Id == reason.Id).FirstOrDefault());
            return View(reason);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReasonDto reasonDto)
        {
            if (ModelState.IsValid)
            {
                await reasonService.Edit(reasonDto);
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public IActionResult Create()
        {
            var vatTypes = vatTypeService.GetAllView();
            //.Select(vt => new SelectListItem(vt.Id.ToString(),vt.Name));
            this.ViewBag.SelectVatType =new SelectList( vatTypes,"Id","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReasonDto reasonDto)
        {
            if (ModelState.IsValid)
            {
                await reasonService.Create(reasonDto);
                return RedirectToAction("Index");
            }
            return View(reasonDto);
        }
    }
}
