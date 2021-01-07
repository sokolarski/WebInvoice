using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.Reason;
using WebInvoice.Dto.VatType;

namespace WebInvoice.Services
{
    public class ReasonService : IReasonService
    {
        private readonly ICompanyDeletableEntityRepository<Reason> reasonRepository;

        public ReasonService(ICompanyDeletableEntityRepository<Reason> reasonRepository)
        {
            this.reasonRepository = reasonRepository;
        }

        public ICollection<ReasonDto> GetAllCompanyReason()
        {
            var reasons = reasonRepository.AllAsNoTracking().Select(r => new ReasonDto()
            {
                Name = r.Name,
                Id = r.Id,
                Description = r.Description,
                IsActive = r.IsActive,
                VatTypeId = r.VatTypeId,
                VatType = new VatTypeView() { Name = r.VatType.Name + "-" + r.VatType.Percantage.ToString("F2") + "%", Id = r.VatType.Id },
            }).ToList();
            return reasons;
        }
        public ReasonDto GetById(int id)
        {
            var reason = reasonRepository.AllAsNoTracking().Where(r => r.Id == id).Select(r => new ReasonDto()
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsActive = r.IsActive,
                VatTypeId = r.VatTypeId,
            }).FirstOrDefault();

            return reason;
        }
        public async Task Create(ReasonDto reasonDto)
        {

            if (reasonDto.IsActive == true)
            {
                SetAllNonActive();
            }
            var reason = new Reason()
            {
                Name = reasonDto.Name,
                Description = reasonDto.Description,
                IsActive = reasonDto.IsActive,
                VatTypeId = reasonDto.VatTypeId,
            };

            await reasonRepository.AddAsync(reason);
            await reasonRepository.SaveChangesAsync();
        }

        public async Task Edit(ReasonDto reasonDto)
        {
            var reason = reasonRepository.All().Where(r => r.Id == reasonDto.Id).FirstOrDefault();
            if (reasonDto.Id != 0 && reason != null)
            {
                if (reasonDto.IsActive == true)
                {
                    SetAllNonActive();
                }

                reason.Name = reasonDto.Name;
                reason.Description = reasonDto.Description;
                reason.IsActive = reasonDto.IsActive;
                reason.VatTypeId = reasonDto.VatTypeId;

                reasonRepository.Update(reason);
                await reasonRepository.SaveChangesAsync();
            }
        }

        private void SetAllNonActive()
        {
            var reasons = reasonRepository.All();

            foreach (var reason in reasons)
            {
                reason.IsActive = false;
                reasonRepository.Update(reason);
            }
        }
    }
}
