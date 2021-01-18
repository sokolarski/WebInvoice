using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Dto.Partner;

namespace WebInvoice.Services
{
    public interface IPartnerService
    {
        IEnumerable<PartnerShortViewDto> GetAllPartners();
        Task<int> Create(PartnerDto partnerDto);

        IEnumerable<PartnerShortViewDto> FindPartner(string name);
        Task<PaginatedList<PartnerShortViewDto>> GetPaginatedPartnerAsync(int page);

    }
}