using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.BankAccount;

namespace WebInvoice.Services
{
    public interface IBankAccountService
    {
        Task<ICollection<BankAccountDto>> GetAllCompanyBankAccounts();
        Task<BankAccountDto> GetById(int id);
        Task Edit(BankAccountDto bankAccountDto);
        Task Create(BankAccountDto bankAccountDto);
        Task ValidateBankAccount(BankAccountDto bankAccountDto);
    }
}