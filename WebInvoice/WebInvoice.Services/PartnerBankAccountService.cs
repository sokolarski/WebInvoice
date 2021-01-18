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
    public class PartnerBankAccountService : IPartnerBankAccountService
    {
        private readonly ICompanyDeletableEntityRepository<BankAccount> bankAccountRepository;

        public PartnerBankAccountService(ICompanyDeletableEntityRepository<BankAccount> bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        public ICollection<BankAccountDto> GetAllCompanyBankAccounts(int companyId)
        {
            var bankAccounts = bankAccountRepository.AllAsNoTracking().Where(ba => ba.PartnerId == companyId)
                                                    .Select(ba => new BankAccountDto()
                                                    {
                                                        Id = ba.Id,
                                                        Name = ba.Name,
                                                        BankName = ba.BankName,
                                                        BIC = ba.BIC,
                                                        IBAN = ba.IBAN,
                                                        Description = ba.Description,
                                                        IsActive = ba.IsActive
                                                    }).ToList();

            return bankAccounts;
        }

        public BankAccountDto GetById(int id)
        {
            var bankAccount = bankAccountRepository.AllAsNoTracking().Where(ba => ba.Id == id)
                                                    .Select(ba => new BankAccountDto()
                                                    {
                                                        Id = ba.Id,
                                                        Name = ba.Name,
                                                        BankName = ba.BankName,
                                                        BIC = ba.BIC,
                                                        IBAN = ba.IBAN,
                                                        Description = ba.Description,
                                                        IsActive = ba.IsActive
                                                    }).FirstOrDefault();
            return bankAccount;
        }

        public async Task Edit(BankAccountDto bankAccountDto, int companyId)
        {
            var bankAccount = bankAccountRepository.All().Where(bo => bo.Id == bankAccountDto.Id).FirstOrDefault();

            if (bankAccountDto.Id != 0 && bankAccount != null)
            {
                if (bankAccountDto.IsActive == true)
                {
                    SetAllNonActive(companyId);
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

        public async Task Create(BankAccountDto bankAccountDto, int companyId)
        {
            if (bankAccountDto.IsActive == true)
            {
                SetAllNonActive(companyId);
            }
            var bankAccount = new BankAccount()
            {
                Name = bankAccountDto.Name,
                BankName = bankAccountDto.BankName,
                BIC = bankAccountDto.BIC,
                IBAN = bankAccountDto.IBAN,
                Description = bankAccountDto.Description,
                IsActive = bankAccountDto.IsActive,
                PartnerId = companyId,
            };

            await bankAccountRepository.AddAsync(bankAccount);
            await bankAccountRepository.SaveChangesAsync();
        }

        private void SetAllNonActive(int companyId)
        {
            var bankAccounts = bankAccountRepository.All().Where(ba => ba.PartnerId == companyId);

            foreach (var bankAccount in bankAccounts)
            {
                bankAccount.IsActive = false;
                bankAccountRepository.Update(bankAccount);
            }
        }

        public void ValidateBankAccount(BankAccountDto bankAccountDto, int companyId)
        {
            if (bankAccountDto.Id != 0)
            {
                var bankAccount = bankAccountRepository.AllAsNoTracking().Where(ba => ba.Name == bankAccountDto.Name && ba.PartnerId == companyId && ba.Id != bankAccountDto.Id).FirstOrDefault();
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
                var bankAccount = bankAccountRepository.AllAsNoTracking().Where(ba => ba.Name == bankAccountDto.Name && ba.PartnerId == companyId).FirstOrDefault();
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
