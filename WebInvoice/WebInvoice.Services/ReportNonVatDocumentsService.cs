using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.ViewDocument;
using WebInvoice.Services.Reports;

namespace WebInvoice.Services
{
    public class ReportNonVatDocumentsService : IReportNonVatDocumentsService
    {
        private readonly ICompanyDeletableEntityRepository<NonVatDocument> nonVatDocumentRepo;
        private readonly IPartnerService partnerService;

        public ReportNonVatDocumentsService(ICompanyDeletableEntityRepository<NonVatDocument> nonVatDocumentRepo,
            IPartnerService partnerService)
        {
            this.nonVatDocumentRepo = nonVatDocumentRepo;
            this.partnerService = partnerService;
        }

        public async Task<PaginatedList<DocumentShortView>> GetPaginatedNonVatDocumentAsync(int page, int itemPerPage)
        {

            var query = nonVatDocumentRepo.AllAsNoTracking().OrderByDescending(e => e.CreatedDate).Select(e => new DocumentShortView()
            {
                Id = e.Id,
                PartnerName = e.Partner.Name,
                CreatedDate = e.CreatedDate.ToString("dd.MM.yyyy"),
                DocumentType = e.Type.ToString(),
                Base = e.SubTottal,
                Vat = e.Vat ?? 0,
                Tottal = e.Tottal,
                IsVatDocument = false,

            });
            var result = await PaginatedList<DocumentShortView>.CreateAsync(query, page, itemPerPage);
            foreach (var item in result)
            {
                item.DocumentType = SetType(item.DocumentType);
            }
            return result;
        }

        public async Task<Report> GetPaginatedNonVatDocumentByCriteriaAsync(int page, int itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate, string objGuid)
        {
            var report = new Report();
            var query = nonVatDocumentRepo.AllAsNoTracking();

            if (documentId != null)
            {
                query = query.Where(e => e.Id == documentId);
            }
            else
            {
                if (!string.IsNullOrEmpty(type))
                {
                    if (type == "proformInvoice")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.NonVatDocumentTypes.ProformaInvoice);
                    }
                    else if (type == "protocol")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.NonVatDocumentTypes.Protocol);

                    }
                    else if (type == "stock")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.NonVatDocumentTypes.Stock);

                    }
                }

                if (!string.IsNullOrEmpty(partnerName))
                {
                    var partnerId = await partnerService.GetPartnerByName(partnerName);
                    if (partnerId != null)
                    {
                        query = query.Where(e => e.PartnerId == partnerId.Id);
                    }
                    else
                    {
                        query = query.Where(e => e.PartnerId == 0);
                    }

                }

                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime date;
                    var isDate = DateTime.TryParse(startDate, new CultureInfo("bg-BG"), DateTimeStyles.None, out date);
                    if (isDate)
                    {
                        query = query.Where(e => e.CreatedDate >= date);
                    }
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime date;
                    var isDate = DateTime.TryParse(endDate, new CultureInfo("bg-BG"), DateTimeStyles.None, out date);
                    if (isDate)
                    {
                        query = query.Where(e => e.CreatedDate <= date);
                    }
                }

                if (!string.IsNullOrEmpty(objGuid))
                {
                    query = query.Where(e => e.CompanyObject.GUID == objGuid);
                }
            }




            var newQuery = query.OrderByDescending(e => e.CreatedDate).ThenByDescending(e => e.Id).Select(e => new DocumentShortView()
            {
                Id = e.Id,
                PartnerName = e.Partner.Name,
                CreatedDate = e.CreatedDate.ToString("dd.MM.yyyy"),
                DocumentType = e.Type.ToString(),
                Base = e.SubTottal,
                Vat = e.Vat ?? 0,
                Tottal = e.Tottal,
                IsVatDocument = false,

            });
            var result = await PaginatedList<DocumentShortView>.CreateAsync(newQuery, page, itemPerPage);
            foreach (var item in result)
            {
                item.DocumentType = SetType(item.DocumentType);
            }
            report.Documents = result;
            return report;
        }
        private string SetType(string type)
        {
            if (type == "ProformaInvoice")
            {
                return "Проформа фактура";
            }
            else if (type == "Stock")
            {
                return "Стокова разписка";
            }
            else if (type == "Protocol")
            {
                return "Протокол";
            }
            return null;
        }
    }
}
