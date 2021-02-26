using System;
using System.Collections.Generic;
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

        public SearchVatDocumentService(ICompanyDeletableEntityRepository<VatDocument> vatDocumentRepo)
        {
            this.vatDocumentRepo = vatDocumentRepo;
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
