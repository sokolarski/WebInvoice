using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.BankAccount;

namespace WebInvoice.Services
{
    public interface IPartnerBankAccountService
    {
        Task Create(BankAccountDto bankAccountDto, int companyId);
        Task Edit(BankAccountDto bankAccountDto, int companyId);
        Task<ICollection<BankAccountDto>> GetAllCompanyBankAccounts(int companyId);
        Task<BankAccountDto> GetById(int id);
        Task ValidateBankAccount(BankAccountDto bankAccountDto, int companyId);
    }
}