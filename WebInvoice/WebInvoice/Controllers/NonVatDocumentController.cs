using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.Document;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    public class NonVatDocumentController : Controller
    {
        private readonly ICompanyDeletableEntityRepository<Company> companyRepository;
        private readonly IVatTypeService vatTypeService;
        private readonly IEmployeeService employeeService;
        private readonly IPaymentTypeService paymentTypeService;
        private readonly IBankAccountService bankAccountService;
        private readonly INonVatDocumentService nonVatDocumentService;

        public NonVatDocumentController(ICompanyDeletableEntityRepository<Company> companyRepository,
            IVatTypeService vatTypeService,
            IEmployeeService employeeService,
            IPaymentTypeService paymentTypeService,
            IBankAccountService bankAccountService,
            INonVatDocumentService nonVatDocumentService)
        {
            this.companyRepository = companyRepository;
            this.vatTypeService = vatTypeService;
            this.employeeService = employeeService;
            this.paymentTypeService = paymentTypeService;
            this.bankAccountService = bankAccountService;
            this.nonVatDocumentService = nonVatDocumentService;
        }
        public IActionResult Index()
        {
            var name = companyRepository.All().FirstOrDefault();
            return Json(name);
        }

        public async Task<IActionResult> CreateProformaInvoice()
        {
            var model = await nonVatDocumentService.PrepareNonVatDocumentModelAsync(dto.NonVatDocumentTypes.ProformaInvoice);
            await SetViewBagDataAsync();
            return View("Create", model);
        }

        public async Task<IActionResult> CreateProtocol()
        {
            var model = await nonVatDocumentService.PrepareNonVatDocumentModelAsync(dto.NonVatDocumentTypes.Protocol);
            await SetViewBagDataAsync();
            return View("Create", model);
        }

        public async Task<IActionResult> CreateStock()
        {
            var model = await nonVatDocumentService.PrepareNonVatDocumentModelAsync(dto.NonVatDocumentTypes.Stock);
            await SetViewBagDataAsync();
            return View("Create", model);
        }

        public async Task<IActionResult> Edit(long id)
        {
            var model = await nonVatDocumentService.PrepareEditNonVatDocumentModelAsync(id);
            await SetViewBagDataAsync();
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NonVatDocumentDto nonVatDocumentDto)
        {
            if (ModelState.IsValid)
            {
                await nonVatDocumentService.EditNonVatDocumentAsync(nonVatDocumentDto);
                if (nonVatDocumentDto.HasErrors)
                {
                    await SetViewBagDataAsync();
                    return View(nonVatDocumentDto);
                }

                return RedirectToAction("ViewNonVatDocument", "ViewNonVatDocument", new { id = nonVatDocumentDto.Id });
                //return ok
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    nonVatDocumentDto.ErrorMassages.Add(error.ErrorMessage);
                }
            }
            await SetViewBagDataAsync();

            return View(nonVatDocumentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NonVatDocumentDto nonVatDocumentDto)
        {
            if (ModelState.IsValid)
            {
                await nonVatDocumentService.CreateNonVatDocumentAsync(nonVatDocumentDto);
                if (nonVatDocumentDto.HasErrors)
                {
                    await SetViewBagDataAsync();
                    return View(nonVatDocumentDto);
                }

                return RedirectToAction("ViewNonVatDocument", "ViewNonVatDocument", new { id = nonVatDocumentDto.Id });
                //return ok
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    nonVatDocumentDto.ErrorMassages.Add(error.ErrorMessage);
                }
            }
            await SetViewBagDataAsync();

            return View(nonVatDocumentDto);
        }

        private async Task SetViewBagDataAsync()
        {
            var vatTypes = vatTypeService.GetAll();
            this.ViewBag.VatTypes = JsonConvert.SerializeObject(vatTypes);
            this.ViewBag.Employees = await employeeService.GetAllCompanyEmployees();
            this.ViewBag.PaymentTypes = await paymentTypeService.GetAllCompanyPaymentTypes();
            this.ViewBag.BankAccounts = await bankAccountService.GetAllCompanyBankAccounts();
        }
    }
}
