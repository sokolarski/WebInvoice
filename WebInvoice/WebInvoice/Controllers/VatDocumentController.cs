using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.Document;
using WebInvoice.Dto.Product;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class VatDocumentController : Controller
    {
        private readonly ICompanyDeletableEntityRepository<Company> companyRepository;
        private readonly IVatTypeService vatTypeService;
        private readonly IEmployeeService employeeService;
        private readonly IPaymentTypeService paymentTypeService;
        private readonly IBankAccountService bankAccountService;
        private readonly IVatDocumentService vatDocumentService;
        private readonly IUserCompanyTemp userCompanyTemp;

        public VatDocumentController(ICompanyDeletableEntityRepository<Company> companyRepository,
            IVatTypeService vatTypeService,
            IEmployeeService employeeService,
            IPaymentTypeService paymentTypeService,
            IBankAccountService bankAccountService,
            IVatDocumentService vatDocumentService,
            IUserCompanyTemp userCompanyTemp)
        {
            this.companyRepository = companyRepository;
            this.vatTypeService = vatTypeService;
            this.employeeService = employeeService;
            this.paymentTypeService = paymentTypeService;
            this.bankAccountService = bankAccountService;
            this.vatDocumentService = vatDocumentService;
            this.userCompanyTemp = userCompanyTemp;
        }
        public IActionResult Index()
        {
            var name = companyRepository.All().FirstOrDefault();
            return Json(name);
        }

        public async Task<IActionResult> CreateInvoice()
        {
            var model =await vatDocumentService.PrepareVatDocumentModelAsync(dto.VatDocumentTypes.Invoice);
            await SetViewBagDataAsync();
            if (!userCompanyTemp.IsVatRegistered)
            {
                return View("CreateWithoutVat", model);
            }
            return View("Create",model);
        }

        public async Task<IActionResult> CreateCredit()
        {
            var model = await vatDocumentService.PrepareVatDocumentModelAsync(dto.VatDocumentTypes.Credit);
            await SetViewBagDataAsync();
            if (!userCompanyTemp.IsVatRegistered)
            {
                return View("CreateWithoutVat", model);
            }
            return View("Create", model);
        }

        public async Task<IActionResult> CreateDebit()
        {
            var model = await vatDocumentService.PrepareVatDocumentModelAsync(dto.VatDocumentTypes.Debit);
            await SetViewBagDataAsync();
            if (!userCompanyTemp.IsVatRegistered)
            {
                return View("CreateWithoutVat", model);
            }
            return View("Create", model);
        }

        public async Task<IActionResult> Edit(long id)
        {
            var model = await vatDocumentService.PrepareEditVatDocumentModelAsync(id);
            await SetViewBagDataAsync();
            if (!userCompanyTemp.IsVatRegistered)
            {
                return View("EditWithoutVat", model);
            }
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VatDocumentDto vatDocumentDto)
        {
            if (ModelState.IsValid)
            {
                await vatDocumentService.EditVatDocumentAsync(vatDocumentDto);
                if (vatDocumentDto.HasErrors)
                {
                    await SetViewBagDataAsync();
                    return View(vatDocumentDto);
                }

                return RedirectToAction("ViewVatDocument", "ViewVatDocument", new { id = vatDocumentDto.Id });
                
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    vatDocumentDto.ErrorMassages.Add(error.ErrorMessage);
                }
            }
            await SetViewBagDataAsync();

            return View(vatDocumentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VatDocumentDto vatDocumentDto)
        {
            if (ModelState.IsValid)
            {
                await vatDocumentService.CreateVatDocumentAsync(vatDocumentDto);
                if (vatDocumentDto.HasErrors)
                {
                    await SetViewBagDataAsync();
                    return View(vatDocumentDto);
                }

                return RedirectToAction("ViewVatDocument", "ViewVatDocument", new { id = vatDocumentDto.Id });
                
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    vatDocumentDto.ErrorMassages.Add(error.ErrorMessage);
                }
            }
            await SetViewBagDataAsync();

            return View(vatDocumentDto);
        }

        private async Task SetViewBagDataAsync()
        {
            var vatTypes =await vatTypeService.GetAll();
            this.ViewBag.VatTypes = JsonConvert.SerializeObject(vatTypes);
            this.ViewBag.Employees = await employeeService.GetAllCompanyEmployees();
            this.ViewBag.PaymentTypes = await paymentTypeService.GetAllCompanyPaymentTypes();
            this.ViewBag.BankAccounts = await bankAccountService.GetAllCompanyBankAccounts();
        }
    }
}
