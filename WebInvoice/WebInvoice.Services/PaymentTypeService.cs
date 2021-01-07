using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.PaymentType;

namespace WebInvoice.Services
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly ICompanyDeletableEntityRepository<PaymentType> paymentTypeRepository;

        public PaymentTypeService(ICompanyDeletableEntityRepository<PaymentType> paymentTypeRepository)
        {
            this.paymentTypeRepository = paymentTypeRepository;
        }

        public ICollection<PaymentTypeDto> GetAllCompanyPaymentTypes()
        {


            var paymentType = paymentTypeRepository.AllAsNoTracking()
                                                    .Select(e => new PaymentTypeDto()
                                                    {
                                                        Id = e.Id,
                                                        Name = e.Name,
                                                        Description = e.Description,
                                                        IsActiv = e.IsActiv,
                                                    }).ToList();

            return paymentType;
        }

        public PaymentTypeDto GetById(int id)
        {
            var paymentType = paymentTypeRepository.AllAsNoTracking().Where(e => e.Id == id)
                                                    .Select(e => new PaymentTypeDto()
                                                    {
                                                        Id = e.Id,
                                                        Name = e.Name,
                                                        Description = e.Description,
                                                        IsActiv = e.IsActiv,
                                                    }).FirstOrDefault();
            return paymentType;
        }

        public async Task Edit(PaymentTypeDto paymentTypeDto)
        {
            var paymentType = paymentTypeRepository.All().Where(e => e.Id == paymentTypeDto.Id).FirstOrDefault();

            if (paymentTypeDto.Id != 0 && paymentType != null)
            {
                if (paymentTypeDto.IsActiv == true)
                {
                    SetAllNonActive();
                }

                paymentType.Name = paymentTypeDto.Name;
                paymentType.Description = paymentTypeDto.Description;
                paymentType.IsActiv = paymentTypeDto.IsActiv;

                paymentTypeRepository.Update(paymentType);
                await paymentTypeRepository.SaveChangesAsync();
            }
        }

        public async Task Create(PaymentTypeDto paymentTypeDto)
        {
            if (paymentTypeDto.IsActiv == true)
            {
                SetAllNonActive();
            }
            var paymentType = new PaymentType()
            {
                Name = paymentTypeDto.Name,
                Description = paymentTypeDto.Description,
                IsActiv = paymentTypeDto.IsActiv,
            };

            await paymentTypeRepository.AddAsync(paymentType);
            await paymentTypeRepository.SaveChangesAsync();
        }

        private void SetAllNonActive()
        {

            var paymentTypes = paymentTypeRepository.All();

            foreach (var paymentType in paymentTypes)
            {
                paymentType.IsActiv = false;
                paymentTypeRepository.Update(paymentType);
            }
        }
    }
}
