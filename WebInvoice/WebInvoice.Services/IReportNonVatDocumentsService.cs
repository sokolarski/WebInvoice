using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;
using WebInvoice.Services.Reports;

namespace WebInvoice.Services
{
    public interface IReportNonVatDocumentsService
    {
        Task<PaginatedList<DocumentShortView>> GetPaginatedNonVatDocumentAsync(int page, int itemPerPage);
        Task<Report> GetPaginatedNonVatDocumentByCriteriaAsync(int page, int itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate, string objGuid);
    }
}