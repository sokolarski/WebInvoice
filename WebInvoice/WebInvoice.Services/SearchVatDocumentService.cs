using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.ViewDocument;

namespace WebInvoice.Services
{
    public class SearchVatDocumentService : ISearchVatDocumentService
    {
        private readonly ICompanyDeletableEntityRepository<VatDocument> vatDocumentRepo;
        private readonly IPartnerService partnerService;

        public SearchVatDocumentService(ICompanyDeletableEntityRepository<VatDocument> vatDocumentRepo , IPartnerService partnerService)
        {
            this.vatDocumentRepo = vatDocumentRepo;
            this.partnerService = partnerService;
        }

        public async Task<PaginatedList<DocumentShortView>> GetPaginatedVatDocumentAsync(int page, int itemPerPage)
        {

            var query = vatDocumentRepo.AllAsNoTracking().OrderByDescending(e => e.CreatedDate).Select(e => new DocumentShortView()
            {
                Id = e.Id,
                PartnerName = e.Partner.Name,
                CreatedDate = e.CreatedDate.ToString("dd.MM.yyyy"),
                DocumentType = e.Type.ToString(),
                Base = e.SubTottal,
                Vat = e.Vat ?? 0,
                Tottal = e.Tottal,

            });
            var result = await PaginatedList<DocumentShortView>.CreateAsync(query, page, itemPerPage);
            foreach (var item in result)
            {
                item.DocumentType = SetType(item.DocumentType);
            }
            return result;
        }

        public async Task<PaginatedList<DocumentShortView>> GetPaginatedVatDocumentByCriteriaAsync(int page, int itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate)
        {
            var query = vatDocumentRepo.AllAsNoTracking();
            if (documentId != null)
            {
               query =  query.Where(e => e.Id == documentId);
            }
            else 
            {
                if (!string.IsNullOrEmpty(type))
                {
                    if (type == "invoice")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.VatDocumentTypes.Invoice);
                    }
                    else if (type == "credit")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.VatDocumentTypes.Credit);

                    }
                    else if (type == "debit")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.VatDocumentTypes.Debit);

                    }
                }

                if (!string.IsNullOrEmpty(partnerName))
                {
                    var partnerId =await partnerService.GetPartnerByName(partnerName);
                    if (partnerId != null)
                    {
                        query = query.Where(e => e.PartnerId == partnerId.Id);
                    }
                    else
                    {
                        query = query.Where(e => e.PartnerId == 0);
                    }
                    
                }

                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime date;
                    var isDate = DateTime.TryParse(startDate, new CultureInfo("bg-BG"), DateTimeStyles.None , out date);
                    if (isDate)
                    {
                        query = query.Where(e => e.CreatedDate >= date);
                    }
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime date;
                    var isDate = DateTime.TryParse(endDate, new CultureInfo("bg-BG"), DateTimeStyles.None, out date);
                    if (isDate)
                    {
                        query = query.Where(e => e.CreatedDate <= date);
                    }
                }
            }




            var newQuery = query.OrderByDescending(e => e.CreatedDate).Select(e => new DocumentShortView()
            {
                Id = e.Id,
                PartnerName = e.Partner.Name,
                CreatedDate = e.CreatedDate.ToString("dd.MM.yyyy"),
                DocumentType = e.Type.ToString(),
                Base = e.SubTottal,
                Vat = e.Vat ?? 0,
                Tottal = e.Tottal,

            });
            var result = await PaginatedList<DocumentShortView>.CreateAsync(newQuery, page, itemPerPage);
            foreach (var item in result)
            {
                item.DocumentType = SetType(item.DocumentType);
            }
            return result;
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
