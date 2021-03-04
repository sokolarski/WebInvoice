using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;
using WebInvoice.Services.Reports;

namespace WebInvoice.Services
{
    public interface IReportVatDocumentsService
    {
        Task<Report> GetPaginatedVatDocumentByCriteriaAsync(int page, int itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate, string objGuid);
        Task<ReportExport> ExportVatDocumentByCriteriaAsync(int page, int itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate, string objGuid);
    }
}