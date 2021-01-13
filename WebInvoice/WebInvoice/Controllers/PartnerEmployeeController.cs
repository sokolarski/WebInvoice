using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.Employee;
using WebInvoice.Dto.Partner;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    public class PartnerEmployeeController : Controller
    {
        private readonly IPartnerEmployeeService partnerEmployeeService;

        public PartnerEmployeeController(IPartnerEmployeeService partnerEmployeeService)
        {
            this.partnerEmployeeService = partnerEmployeeService;
        }
        public IActionResult Index(int companyId)
        {
            var model = partnerEmployeeService.GetAllCompanyEmployees(companyId);
            this.ViewBag.companyId = companyId;
            return View(model);
        }

        public IActionResult Edit(int id, int companyId)
        {
            var model = partnerEmployeeService.GetById(id);
            this.ViewBag.companyId = companyId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeDto employeeDto, int companyId)
        {
            if (ModelState.IsValid)
            {
                await partnerEmployeeService.Edit(employeeDto, companyId);
                return RedirectToAction("Index", new { companyId = companyId });
            }
            this.ViewBag.companyId = companyId;
            return View(employeeDto);
        }


        public IActionResult Create(int companyId)
        {
            this.ViewBag.companyId = companyId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDto employeeDto, int companyId)
        {
            if (ModelState.IsValid)
            {
                await partnerEmployeeService.Create(employeeDto, companyId);
                return RedirectToAction("Index", new { companyId = companyId });
            }
            this.ViewBag.companyId = companyId;
            return View(employeeDto);
        }
    }
}
