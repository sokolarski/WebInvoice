using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.PaymentType;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    public class PaymentTypeController : Controller
    {
        private readonly IPaymentTypeService paymentTypeService;

        public PaymentTypeController(IPaymentTypeService paymentTypeService)
        {
            this.paymentTypeService = paymentTypeService;
        }
        public IActionResult Index()
        {
            var model = paymentTypeService.GetAllCompanyPaymentTypes();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = paymentTypeService.GetById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PaymentTypeDto paymentTypeDto)
        {
            if (ModelState.IsValid)
            {
                await paymentTypeService.Edit(paymentTypeDto);
                return RedirectToAction("Index");
            }
            return View(paymentTypeDto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentTypeDto paymentTypeDto)
        {
            if (ModelState.IsValid)
            {
                await paymentTypeService.Create(paymentTypeDto);
                return RedirectToAction("Index");
            }
            return View(paymentTypeDto);
        }
    }
}
