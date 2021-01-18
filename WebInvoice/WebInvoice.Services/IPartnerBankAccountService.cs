using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.BankAccount;

namespace WebInvoice.Services
{
    public interface IPartnerBankAccountService
    {
        Task Create(BankAccountDto bankAccountDto, int companyId);
        Task Edit(BankAccountDto bankAccountDto, int companyId);
        ICollection<BankAccountDto> GetAllCompanyBankAccounts(int companyId);
        BankAccountDto GetById(int id);
        void ValidateBankAccount(BankAccountDto bankAccountDto, int companyId);
    }
}