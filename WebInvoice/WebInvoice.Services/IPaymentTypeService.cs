using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.PaymentType;

namespace WebInvoice.Services
{
    public interface IPaymentTypeService
    {
        Task Create(PaymentTypeDto paymentTypeDto);
        Task Edit(PaymentTypeDto paymentTypeDto);
        ICollection<PaymentTypeDto> GetAllCompanyPaymentTypes();
        PaymentTypeDto GetById(int id);
    }
}