using System.Threading.Tasks;
using WebInvoice.dto;
using WebInvoice.Dto.Document;

namespace WebInvoice.Services
{
    public interface IVatDocumentService
    {
        Task<VatDocumentDto> PrepareVatDocumentModelAsync(VatDocumentTypes vatDocumentType);
        Task<VatDocumentDto> CreateVatDocumentAsync(VatDocumentDto vatDocumentDto);
        Task<VatDocumentDto> PrepareEditVatDocumentModelAsync(long id);
        Task<VatDocumentDto> EditVatDocumentAsync(VatDocumentDto vatDocumentDto);
    }
}