using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.VatType;

namespace WebInvoice.Services
{
    public interface IVatTypeService
    {
        Task Create(VatTypeDto vatTypeDto);
        Task Edit(VatTypeDto vatTypeDto);
        Task<ICollection<VatTypeDto>> GetAll();
        Task<ICollection<VatTypeView>> GetAllView();
        Task<VatTypeDto> GetById(int id);
        Task<int> SetCorrectVatTypeOnNonVatRegisteredCompanyAsync();
        Task ValidateVatType(VatTypeDto vatTypeDto);
    }
}