using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.Employee;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {
            var model =await employeeService.GetAllCompanyEmployees();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model =await employeeService.GetById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
               await employeeService.Edit(employeeDto);
                return RedirectToAction("Index");
            }
            return View(employeeDto);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                await employeeService.Create(employeeDto);
                return RedirectToAction("Index");
            }
            return View(employeeDto);
        }
    }
}
