using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.BankAccount;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class PartnerBankAccountController : Controller
    {
        private readonly IPartnerBankAccountService bankAccountService;

        public PartnerBankAccountController(IPartnerBankAccountService bankAccountService)
        {
            this.bankAccountService = bankAccountService;
        }

        public IActionResult Index(int companyId, string companyName)
        {
            var model = bankAccountService.GetAllCompanyBankAccounts(companyId);
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View(model);
        }

        public IActionResult Edit(int id, int companyId, string companyName)
        {
            var model = bankAccountService.GetById(id);
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BankAccountDto bankAccountDto, int companyId, string companyName)
        {
            bankAccountService.ValidateBankAccount(bankAccountDto, companyId);

            if (ModelState.IsValid && bankAccountDto.IsValidBankAccount)
            {
                await bankAccountService.Edit(bankAccountDto, companyId);
                this.ViewBag.companyId = companyId;
                this.ViewBag.companyName = companyName;
                return RedirectToAction("Index", new { companyId = companyId, companyName = companyName });
            }
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;

            return View(bankAccountDto);
        }

        public IActionResult Create(int companyId, string companyName)
        {
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BankAccountDto bankAccountDto, int companyId, string companyName)
        {
            bankAccountService.ValidateBankAccount(bankAccountDto, companyId);

            if (ModelState.IsValid && bankAccountDto.IsValidBankAccount)
            {
                await bankAccountService.Create(bankAccountDto, companyId);
                this.ViewBag.companyId = companyId;
                this.ViewBag.companyName = companyName;
                return RedirectToAction("Index", new { companyId = companyId, companyName = companyName });
            }
            this.ViewBag.companyId = companyId;
            this.ViewBag.companyName = companyName;
            return View(bankAccountDto);
        }
    }
}
