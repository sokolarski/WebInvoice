using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.Partner;

namespace WebInvoice.Services
{
    public interface IPartnerService
    {
        Task<int> Create(PartnerDto partnerDto);
        Task<IEnumerable<PartnerShortViewDto>> FindPartner(string name);
        Task<IEnumerable<PartnerDataList>> FindPartnerDataList(string name);
        Task<IEnumerable<PartnerShortViewDto>> GetAllPartners();
        Task<PaginatedList<PartnerShortViewDto>> GetPaginatedPartnerAsync(int page);
        Task<PartnerDto> GetPartnerById(int id);
        Task<PartnerDto> GetPartnerByName(string name);
    }
}