using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.VatType;

namespace WebInvoice.Services
{
    public interface IVatTypeService
    {
        Task Create(VatTypeDto vatTypeDto);
        Task Edit(VatTypeDto vatTypeDto);
        ICollection<VatTypeDto> GetAll();
        VatTypeDto GetById(int id);
        void ValidateVatType(VatTypeDto vatTypeDto);
        ICollection<VatTypeView> GetAllView();
    }
}