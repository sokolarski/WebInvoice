using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.ViewDocument;
using Slovom;

namespace WebInvoice.Services
{
    public class ViewVatDocumentService : IViewVatDocumentService
    {
        private readonly ICompanyDeletableEntityRepository<VatDocument> vatDocumentRepo;
        private readonly ICompanySettingsService companySettingsService;
        private readonly IPartnerService partnerService;
        private readonly IPaymentTypeService paymentTypeService;
        private readonly IBankAccountService bankAccountService;

        public ViewVatDocumentService(
            ICompanyDeletableEntityRepository<VatDocument> vatDocumentRepo,
            ICompanySettingsService companySettingsService,
            IPartnerService partnerService,
            IPaymentTypeService paymentTypeService,
            IBankAccountService bankAccountService)
        {
            this.vatDocumentRepo = vatDocumentRepo;
            this.companySettingsService = companySettingsService;
            this.partnerService = partnerService;
            this.paymentTypeService = paymentTypeService;
            this.bankAccountService = bankAccountService;
        }

        public async Task<DocumentView> GetDocumetnById(long id)
        {
            var document = await vatDocumentRepo.Context.VatDocuments
                .Include(vt => vt.WriterEmployee)
                .Include(vt => vt.RecipientEmployee)
                .Include(vt => vt.Sales)
                .Include(vt => vt.CompanyObject)
                .Where(vt => vt.Id == id)
                .FirstOrDefaultAsync();

            if (document is null)
            {
                return null;
            }

            var documentView = new DocumentView()
            {
                Id = document.Id,
                Type = SetType(document.Type.ToString()),
                CreatedDate = document.CreatedDate.ToString("dd.MM.yyyy"),
                VatReasonDate = document.VatReasonDate.ToString("dd.MM.yyyy"),
                SubTottal = document.SubTottal,
                Vat = document.Vat ?? 0,
                Tottal = document.Tottal,
                Description = document.Description,
                FreeText = document.FreeText,
                ObjectCity = document.CompanyObject.City,
                ObjectName = document.CompanyObject.Name,
                WriterEmployee = document.WriterEmployee?.FullName,
                RecipientEmployee = document.RecipientEmployee?.FullName,
                Grif="ОРИГИНАЛ",

            };
            SetTottalSlovom(documentView);

            documentView.Company = await companySettingsService.GetCompanyInfoById(document.CompanyId);
            documentView.Partner = await partnerService.GetPartnerById(document.PartnerId);
            documentView.PaymentType = await paymentTypeService.GetById(document.PaymentTypeId);
            if (document.BankAccountId != null && documentView.PaymentType.RequiredBankAccount)
            {
                documentView.BankAccount = await bankAccountService.GetById(document.BankAccountId ?? 0);
            }

            var tottalByVat = new List<TottalByVat>();

            foreach (var sale in document.Sales)
            {
                if (sale.ProductId != null)
                {
                    var product = await vatDocumentRepo.Context.Products
                        .Where(p => p.Id == sale.ProductId)
                        .Select(p => new ProductRow()
                        {
                            IsProduct = true,
                            Name = p.Name,
                            ProductType = p.QuantityType.Type,
                            VatTypeId = p.VatTypeId,
                            VatTypeName = p.VatType.Name,
                            VatTypePercentage = p.VatType.Percantage,
                            Quantity = sale.Quantity,
                            Price = sale.UnitPrice,
                            TottalPrice = sale.Total,
                        })
                        .FirstOrDefaultAsync();
                    documentView.Products.Add(product);

                    var vat = tottalByVat.Where(x => x.Id == product.VatTypeId).FirstOrDefault();
                    if (vat is null)
                    {
                        vat = new TottalByVat()
                        {
                            Id = product.VatTypeId,
                            Name = product.VatTypeName,
                            Percentage = product.VatTypePercentage,
                            Base = sale.Total,
                            Vat = sale.Vat ?? 0,
                            Tottal = sale.TottalWithVat,
                        };

                        tottalByVat.Add(vat);
                    }
                    else
                    {
                        vat.Base += sale.Total;
                        vat.Vat += sale.Vat ?? 0;
                        vat.Tottal += sale.TottalWithVat;
                    }
                }
                else if (sale.FreeProductId != null)
                {
                    var product = await vatDocumentRepo.Context.FreeProducts
                        .Where(p => p.Id == sale.FreeProductId)
                        .Select(p => new ProductRow()
                        {
                            IsProduct = false,
                            Name = p.Name,
                            ProductType = p.QuantityType,
                            VatTypeId = p.VatTypeId,
                            VatTypeName = p.VatType.Name,
                            VatTypePercentage = p.VatType.Percantage,
                            Quantity = sale.Quantity,
                            Price = sale.UnitPrice,
                            TottalPrice = sale.Total,
                        })
                        .FirstOrDefaultAsync();
                    documentView.Products.Add(product);

                    var vat = tottalByVat.Where(x => x.Id == product.VatTypeId).FirstOrDefault();
                    if (vat is null)
                    {
                        vat = new TottalByVat()
                        {
                            Id = product.VatTypeId,
                            Name = product.VatTypeName,
                            Percentage = product.VatTypePercentage,
                            Base = sale.Total,
                            Vat = sale.Vat ?? 0,
                            Tottal = sale.TottalWithVat,
                        };
                        tottalByVat.Add(vat);
                    }
                    else
                    {
                        vat.Base += sale.Total;
                        vat.Vat += sale.Vat ?? 0;
                        vat.Tottal += sale.TottalWithVat;
                    }
                }
            }
            documentView.TottalByVats = tottalByVat;
            return documentView;
        }

        private void SetTottalSlovom(DocumentView documentView)
        {
            var TottalString = documentView.Tottal.ToString("F2").Split('.');
            var words = new BgNumberSpeller().Spell(long.Parse(TottalString[0]));
            var result = $"{words} лева и {TottalString[1]} ст.";
            documentView.TottalSlovom = result;
        }

        private string SetType(string type)
        {
            if (type == "Invoice")
            {
                return "Фактура";
            }
            else if (type == "Credit")
            {
                return "Кредитно известие";
            }
            else if (type == "Debit")
            {
                return "Дебитно известие";
            }
            return null;
        }
    }
}
