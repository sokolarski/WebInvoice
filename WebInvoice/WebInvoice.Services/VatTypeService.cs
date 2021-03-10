using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.VatType;

namespace WebInvoice.Services
{
    public class VatTypeService : IVatTypeService
    {
        private readonly ICompanyDeletableEntityRepository<VatType> vatTypeRepository;

        public VatTypeService(ICompanyDeletableEntityRepository<VatType> vatTypeRepository)
        {
            this.vatTypeRepository = vatTypeRepository;
        }

        public async Task<ICollection<VatTypeDto>> GetAll()
        {
            var vatTypes = await vatTypeRepository.AllAsNoTracking().Select(vt => new VatTypeDto()
            {
                Id = vt.Id,
                Name = vt.Name,
                Percantage = vt.Percantage,
                Description = vt.Description,
                IsActive = vt.IsActive,
            }).ToListAsync();
            return vatTypes;
        }

        public async Task<ICollection<VatTypeView>> GetAllView()
        {
            var vatTypes = await vatTypeRepository.AllAsNoTracking().Select(vt => new VatTypeView()
            {
                Name = vt.Name + "-" + vt.Percantage.ToString("F2") + "%",
                Id = vt.Id,
                IsActive = vt.IsActive,

            }).ToListAsync();
            return vatTypes;
        }

        public async Task<VatTypeDto> GetById(int id)
        {
            var vatTypes = await vatTypeRepository.AllAsNoTracking().Select(vt => new VatTypeDto()
            {
                Id = vt.Id,
                Name = vt.Name,
                Percantage = vt.Percantage,
                Description = vt.Description,
                IsActive = vt.IsActive,
            }).Where(vt => vt.Id == id).FirstOrDefaultAsync();
            return vatTypes;
        }

        public async Task Edit(VatTypeDto vatTypeDto)
        {
            var vatType = await vatTypeRepository.All().Where(vt => vt.Id == vatTypeDto.Id).FirstOrDefaultAsync();

            if (vatTypeDto.Id != 0 && vatType != null)
            {
                if (vatTypeDto.IsActive == true)
                {
                    SetAllNonActive();
                }

                vatType.Name = vatTypeDto.Name;
                vatType.Description = vatTypeDto.Description;
                vatType.Percantage = vatTypeDto.Percantage;
                vatType.IsActive = vatTypeDto.IsActive;

                vatTypeRepository.Update(vatType);
                await vatTypeRepository.SaveChangesAsync();
            }
        }

        public async Task Create(VatTypeDto vatTypeDto)
        {
            if (vatTypeDto.IsActive == true)
            {
                SetAllNonActive();
            }
            var vatType = new VatType();
            vatType.Name = vatTypeDto.Name;
            vatType.Description = vatTypeDto.Description;
            vatType.Percantage = vatTypeDto.Percantage;
            vatType.IsActive = vatTypeDto.IsActive;

            await vatTypeRepository.AddAsync(vatType);
            await vatTypeRepository.SaveChangesAsync();
        }

        public async Task<int> SetCorrectVatTypeOnNonVatRegisteredCompanyAsync()
        {
            var correctVat = await vatTypeRepository.All().Where(vt => vt.Name == "БезДДСРег" && vt.Percantage == 0M).FirstOrDefaultAsync();
            SetAllNonActive();
            int result;
            if (correctVat is null)
            {
                var newVat = new VatType() { Name = "БезДДСРег", Description = "Фирма не регистрирана по ДДС", Percantage = 0, IsActive = true };
                await vatTypeRepository.AddAsync(newVat);
                await vatTypeRepository.SaveChangesAsync();
                result = newVat.Id;
            }
            else
            {
                correctVat.IsActive = true;
                vatTypeRepository.Update(correctVat);
                await vatTypeRepository.SaveChangesAsync();
                result = correctVat.Id;
            }
            return result;
        }

        private void SetAllNonActive()
        {
            var vatTypes = vatTypeRepository.All();

            foreach (var vatType in vatTypes)
            {
                vatType.IsActive = false;
                vatTypeRepository.Update(vatType);
            }
        }

        public async Task ValidateVatType(VatTypeDto vatTypeDto)
        {
            if (vatTypeDto.Id != 0)
            {
                var vatType = await vatTypeRepository.AllAsNoTracking().Where(vt => vt.Name == vatTypeDto.Name && vt.Id != vatTypeDto.Id).FirstOrDefaultAsync();
                if (vatType is null)
                {
                    vatTypeDto.IsValidVatType = true;
                }
                else
                {
                    vatTypeDto.IsValidVatType = false;
                    vatTypeDto.ErrorMassages.Add($"Съществува Име {vatType.Name}");
                }

            }
            else
            {
                var vatType = await vatTypeRepository.AllAsNoTracking().Where(vt => vt.Name == vatTypeDto.Name).FirstOrDefaultAsync();
                if (vatType is null)
                {
                    vatTypeDto.IsValidVatType = true;
                }
                else
                {
                    vatTypeDto.IsValidVatType = false;
                    vatTypeDto.ErrorMassages.Add($"Съществува Име {vatType.Name}");
                }

            }

        }
    }
}
