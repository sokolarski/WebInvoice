using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.CompanyObject;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    public class CompanyObjectController : Controller
    {
        private readonly ICompanyObjectService companyObjectService;

        public CompanyObjectController(ICompanyObjectService companyObjectService)
        {
            this.companyObjectService = companyObjectService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await companyObjectService.GetAllCompanyObjects();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await companyObjectService.GetById(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyObjectDto companyObjectDto)
        {
            var model = companyObjectDto;

            if (ModelState.IsValid)
            {
                var result = await companyObjectService.Edit(model);
                return RedirectToAction("Index", new {companyObject=result.CompanyObjectSlug });
            }

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyObjectDto companyObjectDto)
        {
            if (ModelState.IsValid)
            {
               var result = await companyObjectService.ObjectDucumentRange(companyObjectDto);
                if (companyObjectDto.IsValideObject)
                {
                    await companyObjectService.Create(companyObjectDto);
                    return RedirectToAction("Index");
                }
               
            }

            return View(companyObjectDto);
        }
    }
}
