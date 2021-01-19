using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.QuantityType;

namespace WebInvoice.Services
{
    public interface IQuantityTypeService
    {
        Task Create(QuantityTypeDto quantityTypeDto);
        Task Edit(QuantityTypeDto quantityTypeDto);
        ICollection<QuantityTypeDto> GetAllQuantityTypes();
        QuantityTypeDto GetById(int id);
        Task<ICollection<QuantityTypeShortView>> GetAllView();
    }
}