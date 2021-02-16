using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.dto;
using WebInvoice.Dto.Document;

namespace WebInvoice.Services
{
    public class VatDocumentService : IVatDocumentService
    {
        private readonly IUserCompanyTemp userCompanyTemp;
        private readonly ICompanyDeletableEntityRepository<VatDocument> vatDocumentRepo;
        private readonly IEmployeeService employeeService;
        private readonly IPartnerEmployeeService partnerEmployeeService;

        public VatDocumentService(IUserCompanyTemp userCompanyTemp,
            ICompanyDeletableEntityRepository<VatDocument> vatDocumentRepo,
            IEmployeeService employeeService,
            IPartnerEmployeeService partnerEmployeeService)
        {
            this.userCompanyTemp = userCompanyTemp;
            this.vatDocumentRepo = vatDocumentRepo;
            this.employeeService = employeeService;
            this.partnerEmployeeService = partnerEmployeeService;
        }

        public async Task<VatDocumentDto> PrepareVatDocumentModelAsync(VatDocumentTypes vatDocumentType)
        {
            var model = new VatDocumentDto();
            model.Id = await GetNewVatDocumentIdAsync(model);
            model.Type = vatDocumentType;
            model.CreatedDate = DateTime.Now.ToString("dd.MM.yyyy");
            model.VatReasonDate = DateTime.Now.ToString("dd.MM.yyyy");
            model.WriterEmployee = (await employeeService.GetActiveCompanyEmployee())?.FullName;

            return model;
        }

        public async Task<VatDocumentDto> CreateVatDocumentAsync(VatDocumentDto vatDocumentDto)
        {
            var documentId =await GetNewVatDocumentIdAsync(vatDocumentDto);
            if (documentId > vatDocumentDto.Id)
            {
                
                vatDocumentDto.ErrorMassages.Add($"Има вече издаден документ с №{new string('0', 10 - vatDocumentDto.Id.ToString().Length) + vatDocumentDto.Id.ToString()}, документа ще бъде издаден с № {new string('0', 10 - documentId.ToString().Length) + documentId.ToString()}");
                vatDocumentDto.Id = documentId;
                return vatDocumentDto;
            }
            if (vatDocumentDto.PartnerId == 0)
            {
                vatDocumentDto.ErrorMassages.Add($"Моля въведете контрагент!");
                return vatDocumentDto;
            }

            var companyId = await vatDocumentRepo.Context.Companies.OrderBy(c => c.Id).Select(c=>c.Id).LastAsync();
            var document = new VatDocument();
            document.Id = vatDocumentDto.Id;
            document.CompanyObjectId= await vatDocumentRepo.Context.CompanyObjects
                .Where(o => o.GUID == userCompanyTemp.CompanyObjectGUID)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();
            document.PartnerId = vatDocumentDto.PartnerId;
            document.CompanyId = companyId;
            document.WriterEmployeeId =await employeeService.GetOrSetEmployeeIdByNameAsync(vatDocumentDto.WriterEmployee);
            document.RecipientEmployeeId = await partnerEmployeeService.GetOrSetEmployeeIdByNameAsync(vatDocumentDto.RecipientEmployee, vatDocumentDto.PartnerId);
            document.CreatedDate = DateTime.Parse(vatDocumentDto.CreatedDate, new CultureInfo("bg-BG"), DateTimeStyles.None);
            document.VatReasonDate = DateTime.Parse(vatDocumentDto.VatReasonDate, new CultureInfo("bg-BG"), DateTimeStyles.None);
            document.PaymentTypeId = vatDocumentDto.PaymentTypeId;
            document.BankAccountId =await GetBankAccountIdIfRequare(vatDocumentDto);
            document.Type = (Data.CompanyData.Models.Enums.VatDocumentTypes)vatDocumentDto.Type;
            document.FreeText = vatDocumentDto.FreeText;
            document.Description = vatDocumentDto.Description;

            await ProcessingSales(vatDocumentDto);

            document.SubTottal = vatDocumentDto.SubTottal;
            document.Vat = vatDocumentDto.Vat;
            document.Tottal = vatDocumentDto.Tottal;

            if (vatDocumentDto.HasErrors)
            {
                return vatDocumentDto;
            }
            await vatDocumentRepo.AddAsync(document);
            await vatDocumentRepo.SaveChangesAsync();
            return vatDocumentDto;

        }

        private async Task ProcessingSales(VatDocumentDto vatDocumentDto)
        {
            vatDocumentDto.Vat = 0M;
            if (vatDocumentDto.Sales.Count == 0)
            {
                vatDocumentDto.ErrorMassages.Add("Няма въведени артикули");
                return ;
            }

            var context = vatDocumentRepo.Context;
            foreach (var sale in vatDocumentDto.Sales)
            {
                if (sale.IsProduct && sale.ProductId != 0)
                {
                    var product =await context.Products.Include(p => p.VatType).Where(p=> p.Id == sale.ProductId).FirstOrDefaultAsync();
                    if (product is null)
                    {
                        vatDocumentDto.ErrorMassages.Add($"Липсва продукт{sale.Name}!");
                        continue;
                    }
                    else
                    {
                        var newSale = new Sales();
                        newSale.ProductId = sale.ProductId;
                        newSale.Quantity = sale.Quantity;
                        newSale.UnitPrice = product.Price;

                        var tottal = sale.Quantity * product.Price;
                        vatDocumentDto.SubTottal += tottal;
                        newSale.Total = tottal;

                        var vat = tottal * (product.VatType.Percantage / 100);
                        vatDocumentDto.Vat += vat;
                        newSale.Vat = vat;

                        var tottalWithVat = tottal + vat;
                        vatDocumentDto.Tottal += tottalWithVat;
                        newSale.TottalWithVat = tottalWithVat;

                        newSale.VatDocumentId = vatDocumentDto.Id;
                        product.Quantity -= sale.Quantity;

                        await context.Sales.AddAsync(newSale);
                        context.Products.Update(product);
                    }
                }
                else if(!sale.IsProduct)
                {
                    var vatType =await context.VatTypes.FindAsync(sale.VatTypeId);
                    if (String.IsNullOrEmpty(sale.Name) || String.IsNullOrEmpty(sale.ProductType))
                    {
                        vatDocumentDto.ErrorMassages.Add("Има грешка в продукти!");
                        continue;
                    }
                    if (vatType is null)
                    {
                        vatDocumentDto.ErrorMassages.Add($"Продукт: {sale.Name} има грешно ДДС!");
                    }
                    var freeProduct = new FreeProduct();
                    freeProduct.Name = sale.Name;
                    freeProduct.QuantityType = sale.ProductType;
                    freeProduct.Quantity = sale.Quantity;
                    freeProduct.Price = sale.Price;
                    freeProduct.VatTypeId = sale.VatTypeId;


                    var newSale = new Sales();
                    newSale.FreeProduct = freeProduct;
                    newSale.Quantity = sale.Quantity;
                    newSale.UnitPrice = sale.Price;

                    var tottal = sale.Quantity * sale.Price;
                    vatDocumentDto.SubTottal += tottal;
                    newSale.Total = tottal;

                    var vat = tottal * (vatType.Percantage / 100);
                    vatDocumentDto.Vat += vat;
                    newSale.Vat = vat;

                    var tottalWithVat = tottal + vat;
                    vatDocumentDto.Tottal += tottalWithVat;
                    newSale.TottalWithVat = tottalWithVat;

                    newSale.VatDocumentId = vatDocumentDto.Id;

                    await context.Sales.AddAsync(newSale);
                }
                else
                {
                    vatDocumentDto.ErrorMassages.Add($"Има грешка в продукт {sale.Name}, {sale.ProductType}, {sale.Quantity}, {sale.Price}, {sale.TottalPrice}");
                }
            }
        }
        private async Task<int?> GetBankAccountIdIfRequare(VatDocumentDto vatDocumentDto)
        {
            var paymentType =await vatDocumentRepo.Context.PaymentTypes.Where(pt => pt.Id == vatDocumentDto.PaymentTypeId).FirstOrDefaultAsync();
            if (paymentType.RequireBankAccount)
            {
                return vatDocumentDto.PaymentTypeId;
            }
            return null;
        }
        private async Task<long> GetNewVatDocumentIdAsync(VatDocumentDto vatDocumentDto)
        {
            var companyObject = await vatDocumentRepo.Context.CompanyObjects
                .Where(o => o.GUID == userCompanyTemp.CompanyObjectGUID)
                .FirstOrDefaultAsync();
            var documentId = await vatDocumentRepo.AllAsNoTracking().Where(vt => vt.CompanyObjectId == companyObject.Id).OrderBy(vt => vt.Id).Select(vt => vt.Id).LastOrDefaultAsync();
            if (documentId > companyObject.EndNum)
            {
                vatDocumentDto.ErrorMassages.Add($"Номерацията в обект {companyObject.Name} е изчерпана!");
            }
            if (documentId == 0)
            {
                return companyObject.StartNum;
            }

            return documentId + 1;
        }
    }
}
