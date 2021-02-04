using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class PartnerEmployeeController : Controller
    {
        private readonly IPartnerEmployeeService partnerEmployeeService;

        public PartnerEmployeeController(IPartnerEmployeeService partnerEmployeeService)
        {
            this.partnerEmployeeService = partnerEmployeeService;
        }
        public async Task<IActionResult> Index(int companyId, string companyName)
        {
            var model =await partnerEmployeeService.GetAllCompanyEmployees(companyId);
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View(model);
        }

        public async Task<IActionResult> Edit(int id, int companyId, string companyName)
        {
            var model =await partnerEmployeeService.GetById(id);
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeDto employeeDto, int companyId, string companyName)
        {
            if (ModelState.IsValid)
            {
                await partnerEmployeeService.Edit(employeeDto, companyId);
                return RedirectToAction("Index", new { companyId = companyId, companyName = companyName });
            }
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View(employeeDto);
        }


        public IActionResult Create(int companyId, string companyName)
        {
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDto employeeDto, int companyId, string companyName)
        {
            if (ModelState.IsValid)
            {
                await partnerEmployeeService.Create(employeeDto, companyId);
                this.ViewBag.companyId = companyId;
                this.ViewBag.companyName = companyName;
                return RedirectToAction("Index", new { companyId = companyId, companyName = companyName });
            }
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View(employeeDto);
        }
    }
}
