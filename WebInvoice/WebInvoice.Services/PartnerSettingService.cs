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
    public class PartnerSettingService : IPartnerSettingService
    {
        private readonly ICompanyDeletableEntityRepository<Partner> partnerRepository;

        public PartnerSettingService(ICompanyDeletableEntityRepository<Partner> partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }

        public async Task<PartnerDto> GetPartnerByIdAsync(int id)
        {
            if (id != 0)
            {
                var partner = await partnerRepository.GetByIdAsync(id);
                var result = new PartnerDto()
                {
                    Id = partner.Id,
                    Name = partner.Name,
                    Country = partner.Country,
                    City = partner.City,
                    Address = partner.Address,
                    EIK = partner.EIK,
                    IsVatRegistered = partner.IsVatRegistered,
                    IsActive = partner.IsActive,
                    VatId = partner.VatId,
                    MOL = partner.MOL,
                    Email = partner.Email,
                };
                return result;
            }
            return null;
        }

        public async Task Edit(PartnerDto partnerDto)
        {
            if (partnerDto.Id != 0)
            {
                var partner = await partnerRepository.GetByIdAsync(partnerDto.Id);

                partner.Name = partnerDto.Name;
                partner.Country = partnerDto.Country;
                partner.City = partnerDto.City;
                partner.Address = partnerDto.Address;
                partner.EIK = partnerDto.EIK;
                partner.IsVatRegistered = partnerDto.IsVatRegistered;
                partner.VatId = partnerDto.VatId;
                partner.Email = partnerDto.Email;
                partner.IsActive = partnerDto.IsActive;
                partner.MOL = partnerDto.MOL;


                partnerRepository.Update(partner);
                await partnerRepository.SaveChangesAsync();

            }

        }
    }
}
