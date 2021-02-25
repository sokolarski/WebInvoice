using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;

namespace WebInvoice.Services
{
    public interface IViewNonVatDocumentService
    {
        Task<DocumentView> GetDocumetnById(long id);
    }
}