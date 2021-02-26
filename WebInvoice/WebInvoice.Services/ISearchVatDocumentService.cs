using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;

namespace WebInvoice.Services
{
    public interface ISearchVatDocumentService
    {
        Task<PaginatedList<DocumentShortView>> GetPaginatedVatDocumentAsync(int page, int itemPerPage);
    }
}