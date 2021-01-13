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
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            this.bankAccountService = bankAccountService;
        }
        public IActionResult Index()
        {
            var model = bankAccountService.GetAllCompanyBankAccounts();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = bankAccountService.GetById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BankAccountDto bankAccountDto)
        {
            bankAccountService.ValidateBankAccount(bankAccountDto);

            if (ModelState.IsValid && bankAccountDto.IsValidBankAccount)
            {
               await bankAccountService.Edit(bankAccountDto);
                return RedirectToAction("Index");
            }

            return View(bankAccountDto);
        }

        public async Task<IActionResult> Create(BankAccountDto bankAccountDto)
        {
            bankAccountService.ValidateBankAccount(bankAccountDto);

            if (ModelState.IsValid && bankAccountDto.IsValidBankAccount)
            {
                await bankAccountService.Create(bankAccountDto);
                return RedirectToAction("Index");
            }

            return View(bankAccountDto);
        }
    }
}
