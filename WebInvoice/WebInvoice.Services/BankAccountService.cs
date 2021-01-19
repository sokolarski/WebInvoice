using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.BankAccount;

namespace WebInvoice.Services
{
    public class BankAccountService : IBankAccountService
    {

        private readonly ICompanyDeletableEntityRepository<BankAccount> bankAccountRepository;

        public BankAccountService(ICompanyDeletableEntityRepository<BankAccount> bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        public async Task<ICollection<BankAccountDto>> GetAllCompanyBankAccounts()
        {
            var company =await bankAccountRepository.Context.Companies.OrderBy(c => c.Id).LastOrDefaultAsync();

            var bankAccounts =await bankAccountRepository.AllAsNoTracking().Where(ba => ba.CompanyId == company.Id)
                                                    .Select(ba => new BankAccountDto()
                                                    {
                                                        Id = ba.Id,
                                                        Name = ba.Name,
                                                        BankName = ba.BankName,
                                                        BIC = ba.BIC,
                                                        IBAN = ba.IBAN,
                                                        Description = ba.Description,
                                                        IsActive = ba.IsActive
                                                    }).ToListAsync();

            return bankAccounts;
        }

        public async Task<BankAccountDto> GetById(int id)
        {
            var bankAccount =await bankAccountRepository.AllAsNoTracking().Where(ba => ba.Id == id)
                                                    .Select(ba => new BankAccountDto()
                                                    {
                                                        Id = ba.Id,
                                                        Name = ba.Name,
                                                        BankName = ba.BankName,
                                                        BIC = ba.BIC,
                                                        IBAN = ba.IBAN,
                                                        Description = ba.Description,
                                                        IsActive = ba.IsActive
                                                    }).FirstOrDefaultAsync();
            return bankAccount;
        }

        public async Task Edit(BankAccountDto bankAccountDto)
        {
            var bankAccount = bankAccountRepository.All().Where(bo => bo.Id == bankAccountDto.Id).FirstOrDefault();

            if (bankAccountDto.Id != 0 && bankAccount != null)
            {
                if (bankAccountDto.IsActive == true)
                {
                   await SetAllNonActive();
                }

                bankAccount.Name = bankAccountDto.Name;
                bankAccount.BankName = bankAccountDto.BankName;
                bankAccount.BIC = bankAccount.BIC;
                bankAccount.IBAN = bankAccountDto.IBAN;
                bankAccount.Description = bankAccountDto.Description;
                bankAccount.IsActive = bankAccountDto.IsActive;

                bankAccountRepository.Update(bankAccount);
                await bankAccountRepository.SaveChangesAsync();
            }
        }

        public async Task Create(BankAccountDto  bankAccountDto)
        {
            var company = bankAccountRepository.Context.Companies.OrderBy(c => c.Id).LastOrDefault();
            if (bankAccountDto.IsActive == true)
            {
               await SetAllNonActive();
            }
            var bankAccount = new BankAccount()
            {
                Name = bankAccountDto.Name,
                BankName = bankAccountDto.BankName,
                BIC = bankAccountDto.BIC,
                IBAN = bankAccountDto.IBAN,
                Description = bankAccountDto.Description,
                IsActive = bankAccountDto.IsActive,
                CompanyId = company.Id,
            };
            
            await bankAccountRepository.AddAsync(bankAccount);
            await bankAccountRepository.SaveChangesAsync();
        }

        private async Task SetAllNonActive()
        {
            var company = await bankAccountRepository.Context.Companies.OrderBy(c => c.Id).LastOrDefaultAsync();

            var bankAccounts = bankAccountRepository.All().Where(ba => ba.CompanyId == company.Id); ;

            foreach (var bankAccount in bankAccounts)
            {
                bankAccount.IsActive = false;
                bankAccountRepository.Update(bankAccount);
            }
        }

        public async Task ValidateBankAccount(BankAccountDto bankAccountDto)
        {
            var company =await bankAccountRepository.Context.Companies.OrderBy(c => c.Id).LastOrDefaultAsync();
            if (bankAccountDto.Id != 0)
            {
                var bankAccount =await bankAccountRepository.AllAsNoTracking().Where(ba => ba.Name == bankAccountDto.Name && ba.CompanyId == company.Id && ba.Id != bankAccountDto.Id).FirstOrDefaultAsync();
                if (bankAccount is null)
                {
                    bankAccountDto.IsValidBankAccount = true;
                }
                else
                {
                    bankAccountDto.IsValidBankAccount = false;
                    bankAccountDto.ErrorMassages.Add($"Съществува Име {bankAccount.Name}");
                }

            }
            else
            {
                var bankAccount =await bankAccountRepository.AllAsNoTracking().Where(ba => ba.Name == bankAccountDto.Name && ba.CompanyId == company.Id).FirstOrDefaultAsync();
                if (bankAccount is null)
                {
                    bankAccountDto.IsValidBankAccount = true;
                }
                else
                {
                    bankAccountDto.IsValidBankAccount = false;
                    bankAccountDto.ErrorMassages.Add($"Съществува Име {bankAccount.Name}");
                }

            }

        }
    }
}
