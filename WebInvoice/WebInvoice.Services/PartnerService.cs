using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<PartnerShortViewDto>> GetAllPartners()
        {
            var result = await partnerRepository.AllAsNoTracking().OrderBy(p => p.Name).Select(p => new PartnerShortViewDto()
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.City + ", " + p.Address,
                EIK = p.EIK,
                CountOfDocuments = p.VatDocuments.Count + p.NonVatDocuments.Count,
            }).ToListAsync();

            return result;
        }

        public async Task<PaginatedList<PartnerShortViewDto>> GetPaginatedPartnerAsync(int page)
        {
            int itemPerPage = 10;
            var query = partnerRepository.AllAsNoTracking().OrderBy(p => p.Name).Select(p => new PartnerShortViewDto()
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.City + ", " + p.Address,
                EIK = p.EIK,
                CountOfDocuments = p.VatDocuments.Count + p.NonVatDocuments.Count,
            });
            var result = await PaginatedList<PartnerShortViewDto>.CreateAsync(query, page, itemPerPage);
            return result;
        }

        public async Task<PartnerDto> GetPartnerById(int id)
        {
            var partner = await partnerRepository.AllAsNoTracking().Where(p => p.Id == id).Select(p => new PartnerDto
            {
                Id = p.Id,
                Name = p.Name,
                EIK = p.EIK,
                Country = p.Country,
                City = p.City,
                Address = p.Address,
                VatId = p.VatId,
                MOL = p.MOL,
                IsVatRegistered = p.IsVatRegistered,
                Email = p.Email,
                IsActive = p.IsActive,
            }).FirstOrDefaultAsync();

            return partner;
        }

        public async Task<PartnerDto> GetPartnerByName(string name)
        {
            var partner = await partnerRepository.AllAsNoTracking().Where(p => p.Name == name).Select(p => new PartnerDto
            {
                Id = p.Id,
                Name = p.Name,
                EIK = p.EIK,
                Country = p.Country,
                City = p.City,
                Address = p.Address,
                VatId = p.VatId,
                MOL = p.MOL,
                IsVatRegistered = p.IsVatRegistered,
                Email = p.Email,
                IsActive = p.IsActive,
            }).FirstOrDefaultAsync();

            return partner;
        }

        public async Task<IEnumerable<PartnerShortViewDto>> FindPartner(string name)
        {
            var result = await partnerRepository.AllAsNoTracking().OrderBy(p => p.Name).Where(p => p.Name.Contains(name) == true).Select(p => new PartnerShortViewDto()
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.City + ", " + p.Address,
                EIK = p.EIK,
                CountOfDocuments = p.VatDocuments.Count + p.NonVatDocuments.Count,
            }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<PartnerDataList>> FindPartnerDataList(string name)
        {
            var result = await partnerRepository.AllAsNoTracking().OrderBy(p => p.Name).Where(p => p.Name.Contains(name) == true).Select(p => new PartnerDataList()
            {
                Id = p.Id,
                Name = p.Name,
            }).ToListAsync();

            return result;
        }
        public async Task<int> Create(PartnerDto partnerDto)
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

            var employee = new Employee()
            {
                FullName = partnerDto.MOL,
                IsActive = true,
            };

            partner.Employees.Add(employee);

            await partnerRepository.AddAsync(partner);
            await partnerRepository.SaveChangesAsync();
            return partner.Id;
        }



    }
}
