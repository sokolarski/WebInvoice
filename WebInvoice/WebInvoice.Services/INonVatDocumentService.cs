using System.Threading.Tasks;
using WebInvoice.dto;
using WebInvoice.Dto.Document;

namespace WebInvoice.Services
{
    public interface INonVatDocumentService
    {
        Task<NonVatDocumentDto> CreateNonVatDocumentAsync(NonVatDocumentDto nonVatDocumentDto);
        Task<NonVatDocumentDto> EditNonVatDocumentAsync(NonVatDocumentDto NonVatDocumentDto);
        Task<NonVatDocumentDto> PrepareEditNonVatDocumentModelAsync(long id);
        Task<NonVatDocumentDto> PrepareNonVatDocumentModelAsync(NonVatDocumentTypes nonVatDocumentType);
    }
}