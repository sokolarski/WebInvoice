using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.Partner;

namespace WebInvoice.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly ICompanyDeletableEntityRepository<Partner> partnerRepository;

        public PartnerService(ICompanyDeletableEntityRepository<Partner> partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }

        public IEnumerable<PartnerShortViewDto> GetAllPartners()
        {
            var result = partnerRepository.AllAsNoTracking().Select(p => new PartnerShortViewDto()
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.City + ", " + p.Address,
                EIK = p.EIK,
                CountOfDocuments = p.VatDocuments.Count + p.NonVatDocuments.Count,
            }).ToList();

            return result;
        }

        public async Task Create(PartnerDto partnerDto)
        {
            var partner = new Partner()
            {
                Name = partnerDto.Name,
                Country = partnerDto.Country,
                City = partnerDto.City,
                Address = partnerDto.Address,
                EIK = partnerDto.EIK,
                IsVatRegistered = partnerDto.IsVatRegistered,
                VatId = partnerDto.VatId,
                Email = partnerDto.Email,
                IsActive = partnerDto.IsActive,
                MOL = partnerDto.MOL,
            };

            await partnerRepository.AddAsync(partner);
            await partnerRepository.SaveChangesAsync();
        }

        

    }
}
