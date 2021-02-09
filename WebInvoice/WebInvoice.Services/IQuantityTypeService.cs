using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.QuantityType;

namespace WebInvoice.Services
{
    public interface IQuantityTypeService
    {
        Task Create(QuantityTypeDto quantityTypeDto);
        Task Edit(QuantityTypeDto quantityTypeDto);
        Task<ICollection<QuantityTypeDto>> GetAllQuantityTypes();
        Task<ICollection<QuantityTypeShortView>> GetAllView();
        Task<QuantityTypeDto> GetById(int id);
    }
}