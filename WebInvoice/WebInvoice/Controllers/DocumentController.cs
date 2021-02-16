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
    public class DocumentController : Controller
    {
        private readonly ICompanyDeletableEntityRepository<Company> companyRepository;
        private readonly IVatTypeService vatTypeService;
        private readonly IEmployeeService employeeService;
        private readonly IPaymentTypeService paymentTypeService;
        private readonly IBankAccountService bankAccountService;
        private readonly IVatDocumentService vatDocumentService;

        public DocumentController(ICompanyDeletableEntityRepository<Company> companyRepository,
            IVatTypeService vatTypeService,
            IEmployeeService employeeService,
            IPaymentTypeService paymentTypeService,
            IBankAccountService bankAccountService,
            IVatDocumentService vatDocumentService)
        {
            this.companyRepository = companyRepository;
            this.vatTypeService = vatTypeService;
            this.employeeService = employeeService;
            this.paymentTypeService = paymentTypeService;
            this.bankAccountService = bankAccountService;
            this.vatDocumentService = vatDocumentService;
        }
        public IActionResult Index()
        {
            var name = companyRepository.All().FirstOrDefault();
            return Json(name);
        }

        public async Task<IActionResult> CreateInvoice()
        {
            var model =await vatDocumentService.PrepareVatDocumentModelAsync(dto.VatDocumentTypes.Invoice);
            var vatTypes = vatTypeService.GetAll();
            this.ViewBag.VatTypes = JsonConvert.SerializeObject(vatTypes);
            this.ViewBag.Employees = await employeeService.GetAllCompanyEmployees();
            this.ViewBag.PaymentTypes = await paymentTypeService.GetAllCompanyPaymentTypes();
            this.ViewBag.BankAccounts =await bankAccountService.GetAllCompanyBankAccounts();
            return View("Create",model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VatDocumentDto vatDocumentDto)
        {
            if (ModelState.IsValid)
            {
                await vatDocumentService.CreateVatDocumentAsync(vatDocumentDto);
                if (vatDocumentDto.HasErrors)
                {
                    this.ViewBag.VatTypes = JsonConvert.SerializeObject(vatTypeService.GetAll());
                    this.ViewBag.Employees = await employeeService.GetAllCompanyEmployees();
                    this.ViewBag.PaymentTypes = await paymentTypeService.GetAllCompanyPaymentTypes();
                    this.ViewBag.BankAccounts = await bankAccountService.GetAllCompanyBankAccounts();
                    return View(vatDocumentDto);
                }

                //return ok
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    vatDocumentDto.ErrorMassages.Add(error.ErrorMessage);
                }
            }
            var vatTypes = vatTypeService.GetAll();
            this.ViewBag.VatTypes = JsonConvert.SerializeObject(vatTypes);
            this.ViewBag.Employees = await employeeService.GetAllCompanyEmployees();
            this.ViewBag.PaymentTypes = await paymentTypeService.GetAllCompanyPaymentTypes();
            this.ViewBag.BankAccounts = await bankAccountService.GetAllCompanyBankAccounts();

            return View(vatDocumentDto);
        }
    }
}
