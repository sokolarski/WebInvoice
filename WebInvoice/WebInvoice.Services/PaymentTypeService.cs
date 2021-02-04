using Microsoft.EntityFrameworkCore;
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

        public async Task<ICollection<PaymentTypeDto>> GetAllCompanyPaymentTypes()
        {


            var paymentType = await paymentTypeRepository.AllAsNoTracking()
                                                    .Select(e => new PaymentTypeDto()
                                                    {
                                                        Id = e.Id,
                                                        Name = e.Name,
                                                        Description = e.Description,
                                                        IsActiv = e.IsActiv,
                                                        RequiredBankAccount = e.RequireBankAccount,
                                                    }).ToListAsync();

            return paymentType;
        }

        public async Task<PaymentTypeDto> GetById(int id)
        {
            var paymentType = await paymentTypeRepository.AllAsNoTracking().Where(e => e.Id == id)
                                                    .Select(e => new PaymentTypeDto()
                                                    {
                                                        Id = e.Id,
                                                        Name = e.Name,
                                                        Description = e.Description,
                                                        IsActiv = e.IsActiv,
                                                        RequiredBankAccount = e.RequireBankAccount,
                                                    }).FirstOrDefaultAsync();
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
                paymentType.RequireBankAccount = paymentTypeDto.RequiredBankAccount;

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
                RequireBankAccount = paymentTypeDto.RequiredBankAccount,
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
