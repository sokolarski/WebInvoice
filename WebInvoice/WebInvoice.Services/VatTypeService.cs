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

        public ICollection<VatTypeDto> GetAll()
        {
            var vatTypes = vatTypeRepository.AllAsNoTracking().Select(vt => new VatTypeDto()
                                            {
                                                Id = vt.Id,
                                                Name = vt.Name,
                                                Percantage = vt.Percantage,
                                                Description = vt.Description,
                                                IsActive = vt.IsActive,
                                            }).ToList();
            return vatTypes;
        }

        public VatTypeDto GetById(int id)
        {
            var vatTypes = vatTypeRepository.AllAsNoTracking().Select(vt => new VatTypeDto()
                                              {
                                                  Id = vt.Id,
                                                  Name = vt.Name,
                                                  Percantage = vt.Percantage,
                                                  Description = vt.Description,
                                                  IsActive = vt.IsActive,
                                              }).Where(vt => vt.Id == id).FirstOrDefault();
            return vatTypes;
        }

        public async Task Edit(VatTypeDto vatTypeDto)
        {
            var vatType = vatTypeRepository.All().Where(vt => vt.Id == vatTypeDto.Id).FirstOrDefault();
           
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

        private void SetAllNonActive()
        {
           var vatTypes= vatTypeRepository.All();
            
            foreach (var vatType in vatTypes)
            {
                vatType.IsActive = false;
                vatTypeRepository.Update(vatType);
            }
        }

        public void ValidateVatType(VatTypeDto vatTypeDto)
        {
            if (vatTypeDto.Id !=0)
            {
                var vatType = vatTypeRepository.AllAsNoTracking().Where(vt => vt.Name == vatTypeDto.Name && vt.Id != vatTypeDto.Id).FirstOrDefault();
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
                var vatType = vatTypeRepository.AllAsNoTracking().Where(vt => vt.Name == vatTypeDto.Name).FirstOrDefault();
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
