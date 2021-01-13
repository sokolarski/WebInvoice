using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.Partner;

namespace WebInvoice.Services
{
    public interface IPartnerService
    {
        IEnumerable<PartnerShortViewDto> GetAllPartners();
        Task Create(PartnerDto partnerDto);

    }
}