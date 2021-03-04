using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;

namespace WebInvoice.Services
{
    public interface ISearchVatDocumentService
    {
        Task<PaginatedList<DocumentShortView>> GetPaginatedVatDocumentAsync(int page, int itemPerPage);
        Task<PaginatedList<DocumentShortView>> GetPaginatedVatDocumentByCriteriaAsync(int page, int itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate, string objGuid);
    }
}