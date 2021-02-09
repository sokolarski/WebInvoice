using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.QuantityType;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class QuantityTypeController : Controller
    {
        private readonly IQuantityTypeService quantityTypeService;

        public QuantityTypeController(IQuantityTypeService quantityTypeService)
        {
            this.quantityTypeService = quantityTypeService;
        }
        
        public async Task<IActionResult> Index()
        {
            var model =await quantityTypeService.GetAllQuantityTypes();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model =await quantityTypeService.GetById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (QuantityTypeDto quantityTypeDto)
        {
            if (ModelState.IsValid)
            {
                await quantityTypeService.Edit(quantityTypeDto);
                return RedirectToAction("Index");
            }
            return View(quantityTypeDto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuantityTypeDto quantityTypeDto)
        {
            if (ModelState.IsValid)
            {
                await quantityTypeService.Create(quantityTypeDto);
                return RedirectToAction("Index");
            }
            return View(quantityTypeDto);
        }
    }
}
