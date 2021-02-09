using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.QuantityType;

namespace WebInvoice.Services
{
    public class QuantityTypeService : IQuantityTypeService
    {
        private readonly ICompanyDeletableEntityRepository<QuantityType> quantityTypeRepository;

        public QuantityTypeService(ICompanyDeletableEntityRepository<QuantityType> quantityTypeRepository)
        {
            this.quantityTypeRepository = quantityTypeRepository;
        }

        public async Task<ICollection<QuantityTypeDto>> GetAllQuantityTypes()
        {
            var quantityTypes = await quantityTypeRepository.AllAsNoTracking()
                                                    .Select(e => new QuantityTypeDto()
                                                    {
                                                        Id = e.Id,
                                                        Type = e.Type,
                                                        Description = e.Description,
                                                        IsActive = e.IsActive,
                                                    }).ToListAsync();

            return quantityTypes;
        }

        public async Task<ICollection<QuantityTypeShortView>> GetAllView()
        {
            var quantityTypes = await quantityTypeRepository.AllAsNoTracking().Select(e => new QuantityTypeShortView()
            {
                Type = e.Type,
                Id = e.Id,
                IsActive = e.IsActive,
            }).ToListAsync();
            return quantityTypes;
        }
        public async Task<QuantityTypeDto> GetById(int id)
        {
            var quantityType = await quantityTypeRepository.AllAsNoTracking().Where(e => e.Id == id)
                                                    .Select(e => new QuantityTypeDto()
                                                    {
                                                        Id = e.Id,
                                                        Type = e.Type,
                                                        Description = e.Description,
                                                        IsActive = e.IsActive,
                                                    }).FirstOrDefaultAsync();
            return quantityType;
        }

        public async Task Edit(QuantityTypeDto quantityTypeDto)
        {
            var quantityType = quantityTypeRepository.All().Where(e => e.Id == quantityTypeDto.Id).FirstOrDefault();

            if (quantityTypeDto.Id != 0 && quantityType != null)
            {
                if (quantityTypeDto.IsActive == true)
                {
                    SetAllNonActive();
                }

                quantityType.Type = quantityTypeDto.Type;
                quantityType.Description = quantityTypeDto.Description;
                quantityType.IsActive = quantityTypeDto.IsActive;

                quantityTypeRepository.Update(quantityType);
                await quantityTypeRepository.SaveChangesAsync();
            }
        }

        public async Task Create(QuantityTypeDto quantityTypeDto)
        {
            if (quantityTypeDto.IsActive == true)
            {
                SetAllNonActive();
            }
            var quantityType = new QuantityType()
            {
                Type = quantityTypeDto.Type,
                Description = quantityTypeDto.Description,
                IsActive = quantityTypeDto.IsActive,
            };

            await quantityTypeRepository.AddAsync(quantityType);
            await quantityTypeRepository.SaveChangesAsync();
        }

        private void SetAllNonActive()
        {
            var quantityTypes = quantityTypeRepository.All();

            foreach (var quantityType in quantityTypes)
            {
                quantityType.IsActive = false;
                quantityTypeRepository.Update(quantityType);
            }
        }
    }
}
