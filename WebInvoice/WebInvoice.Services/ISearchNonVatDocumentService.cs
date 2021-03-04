using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;

namespace WebInvoice.Services
{
    public interface ISearchNonVatDocumentService
    {
        Task<PaginatedList<DocumentShortView>> GetPaginatedNonVatDocumentAsync(int page, int itemPerPage);
        Task<PaginatedList<DocumentShortView>> GetPaginatedNonVatDocumentByCriteriaAsync(int page, int itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate, string objGuid);
    }
}