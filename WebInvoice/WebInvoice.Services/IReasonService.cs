using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.Reason;

namespace WebInvoice.Services
{
    public interface IReasonService
    {
        ICollection<ReasonDto> GetAllCompanyReason();
        Task Create(ReasonDto reasonDto);
        Task Edit(ReasonDto reasonDto);
        ReasonDto GetById(int id);
    }
}