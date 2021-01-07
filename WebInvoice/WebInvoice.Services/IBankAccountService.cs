using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.BankAccount;

namespace WebInvoice.Services
{
    public interface IBankAccountService
    {
        ICollection<BankAccountDto> GetAllCompanyBankAccounts();
        BankAccountDto GetById(int id);
        Task Edit(BankAccountDto bankAccountDto);
        Task Create(BankAccountDto bankAccountDto);
        void ValidateBankAccount(BankAccountDto bankAccountDto);
    }
}