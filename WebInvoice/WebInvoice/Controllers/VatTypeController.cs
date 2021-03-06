﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.VatType;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class VatTypeController : Controller
    {
        private readonly IVatTypeService vatTypeService;

        public VatTypeController(IVatTypeService vatTypeService)
        {
            this.vatTypeService = vatTypeService;
        }
        public async Task<IActionResult> Index()
        {
            var model =await vatTypeService.GetAll();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VatTypeDto vatTypeDto)
        {
            await vatTypeService.ValidateVatType(vatTypeDto);

            if (ModelState.IsValid && vatTypeDto.IsValidVatType)
            {
                await vatTypeService.Create(vatTypeDto);
                return RedirectToAction("Index");
            }
            return View(vatTypeDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vatType =await vatTypeService.GetById(id);

            return View(vatType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VatTypeDto vatTypeDto)
        {
            await vatTypeService.ValidateVatType(vatTypeDto);

            if (ModelState.IsValid && vatTypeDto.IsValidVatType)
            {
                await vatTypeService.Edit(vatTypeDto);
                return RedirectToAction("Index");
            }
            return View(vatTypeDto);
        }

    }
}
