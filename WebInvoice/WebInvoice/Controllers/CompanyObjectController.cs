using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.CompanyObject;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
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

            if (ModelState.IsValid)
            {
                await companyObjectService.ValidateObjectDucumentRange(companyObjectDto);

                if (companyObjectDto.IsValidObjectDocumentRange)
                {
                    await companyObjectService.Edit(companyObjectDto);
                    return RedirectToAction("Index");
                }
                
            }

            return View(companyObjectDto);
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
                await companyObjectService.ValidateObjectDucumentRange(companyObjectDto);

                if (companyObjectDto.IsValidObjectDocumentRange)
                {
                    await companyObjectService.Create(companyObjectDto);
                    return RedirectToAction("Index");
                }

            }

            return View(companyObjectDto);
        }

        public async Task<IActionResult> Delete(CompanyObjectDto companyObjectDto)
        {
            if (companyObjectDto!=null)
            {
                await companyObjectService.Delete(companyObjectDto);
            }
            return RedirectToAction("Index");
        }
    }
}
