using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Dto.ViewDocument;

namespace WebInvoice.Services
{
    public interface IViewVatDocumentService
    {
        Task<DocumentView> GetDocumetnById(long id);
    }
}