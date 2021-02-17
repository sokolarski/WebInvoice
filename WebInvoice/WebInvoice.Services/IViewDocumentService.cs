using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Dto.ViewDocument;

namespace WebInvoice.Services
{
    public interface IViewDocumentService
    {
        Task<DocumentView> GetDocumetnById(long id);
    }
}