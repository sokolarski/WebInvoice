using System.Threading.Tasks;
using WebInvoice.Dto.Partner;

namespace WebInvoice.Services
{
    public interface IPartnerSettingService
    {
        Task<PartnerDto> GetPartnerByIdAsync(int id);

        Task Edit(PartnerDto partnerDto);
    }
}