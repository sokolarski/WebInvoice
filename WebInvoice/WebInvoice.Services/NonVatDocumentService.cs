using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.CompanyData.Models.Enums;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.dto;
using WebInvoice.Dto.Document;

namespace WebInvoice.Services
{
    public class NonVatDocumentService : INonVatDocumentService
    {
        private readonly IUserCompanyTemp userCompanyTemp;
        private readonly ICompanyDeletableEntityRepository<NonVatDocument> NonVatDocumentRepo;
        private readonly IEmployeeService employeeService;
        private readonly IPartnerEmployeeService partnerEmployeeService;

        public NonVatDocumentService(IUserCompanyTemp userCompanyTemp,
            ICompanyDeletableEntityRepository<NonVatDocument> NonVatDocumentRepo,
            IEmployeeService employeeService,
            IPartnerEmployeeService partnerEmployeeService)
        {
            this.userCompanyTemp = userCompanyTemp;
            this.NonVatDocumentRepo = NonVatDocumentRepo;
            this.employeeService = employeeService;
            this.partnerEmployeeService = partnerEmployeeService;
        }

        public async Task<NonVatDocumentDto> PrepareNonVatDocumentModelAsync(dto.NonVatDocumentTypes nonVatDocumentType)
        {
            var model = new NonVatDocumentDto();
            model.Id = await GetNewNonVatDocumentIdAsync(model);
            model.Type = nonVatDocumentType;
            model.CreatedDate = DateTime.Now.ToString("dd.MM.yyyy");
            model.VatReasonDate = DateTime.Now.ToString("dd.MM.yyyy");
            model.WriterEmployee = (await employeeService.GetActiveCompanyEmployee())?.FullName;

            return model;
        }

        public async Task<NonVatDocumentDto> PrepareEditNonVatDocumentModelAsync(long id)
        {
            var model = await NonVatDocumentRepo.AllAsNoTracking().Where(d => d.Id == id)
                .Select(vt => new NonVatDocumentDto()
                {
                    Id = vt.Id,
                    CreatedDate = vt.CreatedDate.ToString("dd.MM.yyyy"),
                    VatReasonDate = vt.VatReasonDate.ToString("dd.MM.yyyy"),
                    Description = vt.Description,
                    FreeText = vt.FreeText,
                    RecipientEmployee = vt.RecipientEmployee.FullName,
                    WriterEmployee = vt.WriterEmployee.FullName,
                    SubTottal = vt.SubTottal,
                    Vat = vt.Vat,
                    Tottal = vt.Tottal,
                    Type = (dto.NonVatDocumentTypes)vt.Type,
                    PartnerId = vt.PartnerId,
                    BankAccountId = vt.BankAccountId,
                    CompanyObjectId = vt.CompanyObjectId,
                    PaymentTypeId = vt.PaymentTypeId,
                }
                ).FirstOrDefaultAsync();

            if (model is null)
            {
                return null;
            }
            var sales = await NonVatDocumentRepo.Context.Sales
                 .Include(s => s.Product)
                 .Include(s => s.Product.QuantityType)
                 .Include(s => s.FreeProduct)
                 .Where(s => s.NonVatDocumentId == id)
                 .ToListAsync();

            var salesResult = new List<ProductDocumentDto>();
            foreach (var sale in sales)
            {
                if (sale.ProductId != null && sale.ProductId != 0)
                {
                    var product = new ProductDocumentDto()
                    {
                        ProductId = sale.ProductId,
                        Name = sale.Product.Name,
                        ProductType = sale.Product.QuantityType.Type,
                        Quantity = sale.Quantity,
                        Price = sale.UnitPrice,
                        TottalPrice = sale.Total,
                        VatTypeId = sale.Product.VatTypeId,
                        SaleId = sale.Id,
                        IsProduct = true,

                    };
                    salesResult.Add(product);
                }
                else if (sale.FreeProductId != null && sale.FreeProductId != 0)
                {
                    var product = new ProductDocumentDto()
                    {
                        FreeProductID = sale.FreeProductId,
                        Name = sale.FreeProduct.Name,
                        ProductType = sale.FreeProduct.QuantityType,
                        Quantity = sale.Quantity,
                        Price = sale.UnitPrice,
                        TottalPrice = sale.Total,
                        VatTypeId = sale.FreeProduct.VatTypeId,
                        SaleId = sale.Id,
                    };
                    salesResult.Add(product);
                }
            }
            model.Sales = salesResult;
            return model;
        }

        public async Task<NonVatDocumentDto> EditNonVatDocumentAsync(NonVatDocumentDto nonVatDocumentDto)
        {
            var document = await NonVatDocumentRepo.GetByIdAsync(nonVatDocumentDto.Id);
            if (document is null)
            {

                nonVatDocumentDto.ErrorMassages.Add($"Грешка, не съществува документ с №{nonVatDocumentDto.Id}!");
                return nonVatDocumentDto;
            }
            if (nonVatDocumentDto.PartnerId == 0)
            {
                nonVatDocumentDto.ErrorMassages.Add($"Моля въведете контрагент!");
                return nonVatDocumentDto;
            }

            var companyId = await NonVatDocumentRepo.Context.Companies.OrderBy(c => c.Id).Select(c => c.Id).LastAsync();

            document.CompanyObjectId = await NonVatDocumentRepo.Context.CompanyObjects
                .Where(o => o.GUID == userCompanyTemp.CompanyObjectGUID)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();
            document.PartnerId = nonVatDocumentDto.PartnerId;
            document.CompanyId = companyId;
            document.WriterEmployeeId = await employeeService.GetOrSetEmployeeIdByNameAsync(nonVatDocumentDto.WriterEmployee);
            document.RecipientEmployeeId = await partnerEmployeeService.GetOrSetEmployeeIdByNameAsync(nonVatDocumentDto.RecipientEmployee, nonVatDocumentDto.PartnerId);
            document.CreatedDate = SetDate(nonVatDocumentDto.CreatedDate, nonVatDocumentDto);
            document.VatReasonDate = SetDate(nonVatDocumentDto.VatReasonDate, nonVatDocumentDto);
            document.PaymentTypeId = nonVatDocumentDto.PaymentTypeId;
            document.BankAccountId = await GetBankAccountIdIfRequare(nonVatDocumentDto);
            document.Type = (Data.CompanyData.Models.Enums.NonVatDocumentTypes)nonVatDocumentDto.Type;
            document.FreeText = nonVatDocumentDto.FreeText;
            document.Description = nonVatDocumentDto.Description;

            await ProcessingEditSales(nonVatDocumentDto);

            document.SubTottal = nonVatDocumentDto.SubTottal;
            document.Vat = nonVatDocumentDto.Vat;
            document.Tottal = nonVatDocumentDto.Tottal;

            if (nonVatDocumentDto.HasErrors)
            {
                return nonVatDocumentDto;
            }
            NonVatDocumentRepo.Update(document);
            await NonVatDocumentRepo.SaveChangesAsync();
            return nonVatDocumentDto;
        }

        private async Task ProcessingEditSales(NonVatDocumentDto nonVatDocumentDto)
        {
            nonVatDocumentDto.Vat = 0M;
            var context = NonVatDocumentRepo.Context;
            var sales = await context.Sales.Where(s => s.NonVatDocumentId == nonVatDocumentDto.Id).ToListAsync();

            foreach (var sale in sales)
            {
                var newSale = nonVatDocumentDto.Sales.Where(x => x.SaleId == sale.Id).FirstOrDefault();
                if (newSale is null)
                {
                    if (sale.ProductId != null && sale.ProductId != 0)
                    {
                        var product = await context.Products.FindAsync(sale.ProductId);
                        product.Quantity += sale.Quantity;
                        context.Products.Update(product);
                    }
                    else if (sale.FreeProductId != null && sale.FreeProductId != 0)
                    {
                        var product = await context.FreeProducts.FindAsync(sale.FreeProductId);
                        context.FreeProducts.Remove(product);
                    }
                    context.Sales.Remove(sale);
                }
                else
                {
                    if (sale.ProductId != null && sale.ProductId != 0)
                    {
                        if (sale.ProductId == newSale.ProductId)
                        {
                            if (Decimal.Compare(sale.Quantity, newSale.Quantity) != 0 || Decimal.Compare(sale.Total, newSale.TottalPrice) != 0)
                            {
                                var product = await context.Products.FindAsync(sale.ProductId);
                                product.Quantity += sale.Quantity;
                                context.Products.Update(product);
                                context.Sales.Remove(sale);
                                newSale.SaleId = 0;
                            }
                            else
                            {
                                nonVatDocumentDto.SubTottal += sale.Total;
                                nonVatDocumentDto.Vat += sale.Vat;
                                nonVatDocumentDto.Tottal += sale.TottalWithVat;
                            }
                        }
                        else
                        {
                            var product = await context.Products.FindAsync(sale.ProductId);
                            product.Quantity += sale.Quantity;
                            context.Products.Update(product);
                            context.Sales.Remove(sale);
                            newSale.SaleId = 0;
                        }
                    }
                    else if (sale.FreeProductId != null && sale.FreeProductId != 0)
                    {
                        if (sale.FreeProductId == newSale.FreeProductID)
                        {
                            var freeProduct = await context.FreeProducts.FindAsync(sale.FreeProductId);
                            var vatType = await context.VatTypes.FindAsync(newSale.VatTypeId);

                            freeProduct.Name = newSale.Name;
                            freeProduct.QuantityType = newSale.ProductType;
                            freeProduct.Quantity = newSale.Quantity;
                            freeProduct.Price = newSale.Price;
                            freeProduct.VatTypeId = newSale.VatTypeId;
                            context.FreeProducts.Update(freeProduct);


                            var tottal = newSale.Quantity * newSale.Price;
                            nonVatDocumentDto.SubTottal += tottal;
                            sale.Total = tottal;

                            var vat = tottal * (vatType.Percantage / 100);
                            nonVatDocumentDto.Vat += vat;
                            sale.Vat = vat;

                            var tottalWithVat = tottal + vat;
                            nonVatDocumentDto.Tottal += tottalWithVat;
                            sale.TottalWithVat = tottalWithVat;

                            context.Sales.Update(sale);
                        }
                        else
                        {
                            context.Sales.Remove(sale);
                            newSale.SaleId = 0;
                        }
                    }
                }
            }

            await AddNewSalesOnEdit(nonVatDocumentDto);
        }
        private async Task AddNewSalesOnEdit(NonVatDocumentDto nonVatDocumentDto)
        {

            var context = NonVatDocumentRepo.Context;
            foreach (var sale in nonVatDocumentDto.Sales.Where(s => s.SaleId == 0))
            {
                if (sale.IsProduct && sale.ProductId != 0)
                {
                    var product = await context.Products.Include(p => p.VatType).Where(p => p.Id == sale.ProductId).FirstOrDefaultAsync();
                    if (product is null)
                    {
                        nonVatDocumentDto.ErrorMassages.Add($"Липсва продукт{sale.Name}!");
                        continue;
                    }
                    else
                    {
                        var newSale = new Sales();
                        newSale.ProductId = sale.ProductId;
                        newSale.Quantity = sale.Quantity;
                        newSale.UnitPrice = product.Price;

                        var tottal = sale.Quantity * product.Price;
                        nonVatDocumentDto.SubTottal += tottal;
                        newSale.Total = tottal;

                        var vat = tottal * (product.VatType.Percantage / 100);
                        nonVatDocumentDto.Vat += vat;
                        newSale.Vat = vat;

                        var tottalWithVat = tottal + vat;
                        nonVatDocumentDto.Tottal += tottalWithVat;
                        newSale.TottalWithVat = tottalWithVat;

                        newSale.NonVatDocumentId = nonVatDocumentDto.Id;
                        product.Quantity -= sale.Quantity;

                        await context.Sales.AddAsync(newSale);
                        context.Products.Update(product);
                    }
                }
                else if (!sale.IsProduct)
                {
                    var vatType = await context.VatTypes.FindAsync(sale.VatTypeId);
                    if (String.IsNullOrEmpty(sale.Name) || String.IsNullOrEmpty(sale.ProductType))
                    {
                        nonVatDocumentDto.ErrorMassages.Add("Има грешка в продукти!");
                        continue;
                    }
                    if (vatType is null)
                    {
                        nonVatDocumentDto.ErrorMassages.Add($"Продукт: {sale.Name} има грешно ДДС!");
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
                    nonVatDocumentDto.SubTottal += tottal;
                    newSale.Total = tottal;

                    var vat = tottal * (vatType.Percantage / 100);
                    nonVatDocumentDto.Vat += vat;
                    newSale.Vat = vat;

                    var tottalWithVat = tottal + vat;
                    nonVatDocumentDto.Tottal += tottalWithVat;
                    newSale.TottalWithVat = tottalWithVat;

                    newSale.NonVatDocumentId = nonVatDocumentDto.Id;

                    await context.Sales.AddAsync(newSale);
                }
                else
                {
                    nonVatDocumentDto.ErrorMassages.Add($"Има грешка в продукт {sale.Name}, {sale.ProductType}, {sale.Quantity}, {sale.Price}, {sale.TottalPrice}");
                }
            }
        }
        public async Task<NonVatDocumentDto> CreateNonVatDocumentAsync(NonVatDocumentDto nonVatDocumentDto)
        {
            var documentId = await GetNewNonVatDocumentIdAsync(nonVatDocumentDto);
            if (documentId > nonVatDocumentDto.Id)
            {

                nonVatDocumentDto.ErrorMassages.Add($"Има вече издаден документ с №{nonVatDocumentDto.Id}, документа ще бъде издаден с № {new string('0', 10 - documentId.ToString().Length) + documentId.ToString()}");
                nonVatDocumentDto.Id = documentId;
                return nonVatDocumentDto;
            }
            if (nonVatDocumentDto.PartnerId == 0)
            {
                nonVatDocumentDto.ErrorMassages.Add($"Моля въведете контрагент!");
                return nonVatDocumentDto;
            }

            var companyId = await NonVatDocumentRepo.Context.Companies.OrderBy(c => c.Id).Select(c => c.Id).LastAsync();
            var document = new NonVatDocument();
            document.Id = nonVatDocumentDto.Id;
            document.CompanyObjectId = await NonVatDocumentRepo.Context.CompanyObjects
                .Where(o => o.GUID == userCompanyTemp.CompanyObjectGUID)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();
            document.PartnerId = nonVatDocumentDto.PartnerId;
            document.CompanyId = companyId;
            document.WriterEmployeeId = await employeeService.GetOrSetEmployeeIdByNameAsync(nonVatDocumentDto.WriterEmployee);
            document.RecipientEmployeeId = await partnerEmployeeService.GetOrSetEmployeeIdByNameAsync(nonVatDocumentDto.RecipientEmployee, nonVatDocumentDto.PartnerId);
            document.CreatedDate = SetDate(nonVatDocumentDto.CreatedDate, nonVatDocumentDto);
            document.VatReasonDate = SetDate(nonVatDocumentDto.VatReasonDate, nonVatDocumentDto);
            document.PaymentTypeId = nonVatDocumentDto.PaymentTypeId;
            document.BankAccountId = await GetBankAccountIdIfRequare(nonVatDocumentDto);
            document.Type = (Data.CompanyData.Models.Enums.NonVatDocumentTypes)nonVatDocumentDto.Type;
            document.FreeText = nonVatDocumentDto.FreeText;
            document.Description = nonVatDocumentDto.Description;

            await ProcessingSales(nonVatDocumentDto);

            document.SubTottal = nonVatDocumentDto.SubTottal;
            document.Vat = nonVatDocumentDto.Vat;
            document.Tottal = nonVatDocumentDto.Tottal;

            if (nonVatDocumentDto.HasErrors)
            {
                return nonVatDocumentDto;
            }
            await NonVatDocumentRepo.AddAsync(document);
            await NonVatDocumentRepo.SaveChangesAsync();
            return nonVatDocumentDto;

        }
        private async Task ProcessingSales(NonVatDocumentDto nonVatDocumentDto)
        {
            nonVatDocumentDto.Vat = 0M;
            if (nonVatDocumentDto.Sales.Count == 0)
            {
                nonVatDocumentDto.ErrorMassages.Add("Няма въведени артикули");
                return;
            }

            var context = NonVatDocumentRepo.Context;
            foreach (var sale in nonVatDocumentDto.Sales)
            {
                if (sale.IsProduct && sale.ProductId != 0)
                {
                    var product = await context.Products.Include(p => p.VatType).Where(p => p.Id == sale.ProductId).FirstOrDefaultAsync();
                    if (product is null)
                    {
                        nonVatDocumentDto.ErrorMassages.Add($"Липсва продукт{sale.Name}!");
                        continue;
                    }
                    else
                    {
                        var newSale = new Sales();
                        newSale.ProductId = sale.ProductId;
                        newSale.Quantity = sale.Quantity;
                        newSale.UnitPrice = product.Price;

                        var tottal = sale.Quantity * product.Price;
                        nonVatDocumentDto.SubTottal += tottal;
                        newSale.Total = tottal;

                        var vat = tottal * (product.VatType.Percantage / 100);
                        nonVatDocumentDto.Vat += vat;
                        newSale.Vat = vat;

                        var tottalWithVat = tottal + vat;
                        nonVatDocumentDto.Tottal += tottalWithVat;
                        newSale.TottalWithVat = tottalWithVat;

                        newSale.NonVatDocumentId = nonVatDocumentDto.Id;
                        product.Quantity -= sale.Quantity;

                        await context.Sales.AddAsync(newSale);
                        context.Products.Update(product);
                    }
                }
                else if (!sale.IsProduct)
                {
                    var vatType = await context.VatTypes.FindAsync(sale.VatTypeId);
                    if (String.IsNullOrEmpty(sale.Name) || String.IsNullOrEmpty(sale.ProductType))
                    {
                        nonVatDocumentDto.ErrorMassages.Add("Има грешка в продукти!");
                        continue;
                    }
                    if (vatType is null)
                    {
                        nonVatDocumentDto.ErrorMassages.Add($"Продукт: {sale.Name} има грешно ДДС!");
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
                    nonVatDocumentDto.SubTottal += tottal;
                    newSale.Total = tottal;

                    var vat = tottal * (vatType.Percantage / 100);
                    nonVatDocumentDto.Vat += vat;
                    newSale.Vat = vat;

                    var tottalWithVat = tottal + vat;
                    nonVatDocumentDto.Tottal += tottalWithVat;
                    newSale.TottalWithVat = tottalWithVat;

                    newSale.NonVatDocumentId = nonVatDocumentDto.Id;

                    await context.Sales.AddAsync(newSale);
                }
                else
                {
                    nonVatDocumentDto.ErrorMassages.Add($"Има грешка в продукт {sale.Name}, {sale.ProductType}, {sale.Quantity}, {sale.Price}, {sale.TottalPrice}");
                }
            }
        }
        private async Task<int?> GetBankAccountIdIfRequare(NonVatDocumentDto nonVatDocumentDto)
        {
            var paymentType = await NonVatDocumentRepo.Context.PaymentTypes.Where(pt => pt.Id == nonVatDocumentDto.PaymentTypeId).FirstOrDefaultAsync();
            if (paymentType.RequireBankAccount)
            {
                return nonVatDocumentDto.BankAccountId;
            }
            return null;
        }
        private async Task<long> GetNewNonVatDocumentIdAsync(NonVatDocumentDto nonVatDocumentDto)
        {
            var companyObject = await NonVatDocumentRepo.Context.CompanyObjects
                .Where(o => o.GUID == userCompanyTemp.CompanyObjectGUID)
                .FirstOrDefaultAsync();
            var document = await NonVatDocumentRepo.AllAsNoTracking().OrderBy(nv => nv.Id).LastOrDefaultAsync();
            long documentId = 0;
            if (document != null)
            {
                documentId = document.Id;
            }

            return documentId + 1;
        }

        private DateTime SetDate(string date, NonVatDocumentDto nonVatDocumentDto)
        {
            DateTime parsedDate;
            var isValidDate = DateTime.TryParse(date, new CultureInfo("bg-BG"), DateTimeStyles.None, out parsedDate);
            if (isValidDate)
            {
                return parsedDate;
            }
            nonVatDocumentDto.ErrorMassages.Add("Формата на датата е грешен!");
            return parsedDate;
        }
    }
}
