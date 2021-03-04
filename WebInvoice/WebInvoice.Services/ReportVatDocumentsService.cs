using Microsoft.EntityFrameworkCore;
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
    public class ReportVatDocumentsService : IReportVatDocumentsService
    {
        private readonly ICompanyDeletableEntityRepository<VatDocument> vatDocumentRepo;
        private readonly IPartnerService partnerService;
        private readonly IUserCompanyTemp userCompanyTemp;

        public ReportVatDocumentsService(ICompanyDeletableEntityRepository<VatDocument> vatDocumentRepo,
            IPartnerService partnerService,
            IUserCompanyTemp userCompanyTemp)
        {
            this.vatDocumentRepo = vatDocumentRepo;
            this.partnerService = partnerService;
            this.userCompanyTemp = userCompanyTemp;
        }

        public async Task<Report> GetPaginatedVatDocumentByCriteriaAsync(int page, int itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate, string objGuid)
        {
            var query = vatDocumentRepo.AllAsNoTracking();
            var report = new Report();

            if (documentId != null)
            {
                query = query.Where(e => e.Id == documentId);
            }
            else
            {
                if (!string.IsNullOrEmpty(type))
                {
                    if (type == "invoice")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.VatDocumentTypes.Invoice);
                    }
                    else if (type == "credit")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.VatDocumentTypes.Credit);

                    }
                    else if (type == "debit")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.VatDocumentTypes.Debit);

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

            var sums =await query.GroupBy(x => true).Select(x => new
            {
                Base = x.Sum(y => y.SubTottal),
                Vat = x.Sum(y => y.Vat),
                Tottal = x.Sum(y => y.Tottal)
            }).FirstOrDefaultAsync();

            report.Base = sums.Base;
            report.Vat = sums.Vat ?? 0;
            report.Tottal = sums.Tottal;


            var newQuery = query.OrderByDescending(e => e.CreatedDate).ThenByDescending(e => e.Id).Select(e => new DocumentShortView()
            {
                Id = e.Id,
                PartnerName = e.Partner.Name,
                CreatedDate = e.CreatedDate.ToString("dd.MM.yyyy"),
                DocumentType = e.Type.ToString(),
                Base = e.SubTottal,
                Vat = e.Vat ?? 0,
                Tottal = e.Tottal,
                IsVatDocument = true,
            });
            var result = await PaginatedList<DocumentShortView>.CreateAsync(newQuery, page, itemPerPage);
            foreach (var item in result)
            {
                item.DocumentType = SetType(item.DocumentType);
            }

            report.Documents = result;
            return report;
        }


        public async Task<ReportExport> ExportVatDocumentByCriteriaAsync(int page, int itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate, string objGuid)
        {
            var query = vatDocumentRepo.AllAsNoTracking();
            var report = new ReportExport();
            

            if (documentId != null)
            {
                query = query.Where(e => e.Id == documentId);
                report.DocumentsTypes = "document";
            }
            else
            {
                if (!string.IsNullOrEmpty(type))
                {
                    report.DocumentsTypes = SetType(type);
                    if (type == "invoice")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.VatDocumentTypes.Invoice);
                        report.DocumentsTypes = "Фактури";
                    }
                    else if (type == "credit")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.VatDocumentTypes.Credit);
                        report.DocumentsTypes = "Кредитни известия";
                    }
                    else if (type == "debit")
                    {
                        query = query.Where(e => e.Type == Data.CompanyData.Models.Enums.VatDocumentTypes.Debit);
                        report.DocumentsTypes = "Дебитни известия";
                    }
                    else
                    {
                        report.DocumentsTypes = "Данъчни документи";
                    }
                    
                }
                else
                {
                    report.DocumentsTypes = "Данъчни документи";
                }

                if (!string.IsNullOrEmpty(partnerName))
                {
                    report.CompanyName = partnerName;
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
                    report.Objects = userCompanyTemp.CurrentCompanyAppObjects.Where(o => o.GUID == objGuid).FirstOrDefault()?.ObjectName;
                }
            }

            


            var result =await query.OrderByDescending(e => e.CreatedDate).ThenByDescending(e => e.Id).Select(e =>new
            {
                Id = e.Id,
                PartnerName = e.Partner.Name,
                CreatedDate = e.CreatedDate,
                DocumentType = e.Type.ToString(),
                Base = e.SubTottal,
                Vat = e.Vat ?? 0,
                Tottal = e.Tottal,
            }).ToListAsync();

       
            if (string.IsNullOrEmpty(startDate))
            {
                report.StartDate = result.OrderBy(e => e.CreatedDate).FirstOrDefault()?.CreatedDate.ToString("dd.MM.yyyy");
            }
            else
            {
                report.StartDate = startDate;
            }

            if (string.IsNullOrEmpty(endDate))
            {
                report.EndDate = DateTime.Now.ToString("dd.MM.yyyy");
            }
            else
            {
                report.EndDate = endDate;
            }

            report.Documents = result.Select(e => new DocumentShortView()
            {
                Id = e.Id,
                PartnerName = e.PartnerName,
                CreatedDate = e.CreatedDate.ToString("dd.MM.yyyy"),
                DocumentType = SetType(e.DocumentType.ToString()),
                Base = e.Base,
                Vat = e.Vat,
                Tottal = e.Tottal,
                IsVatDocument = true,
            });
            return report;
        }
        private string SetType(string type)
        {
            if (type == "Invoice")
            {
                return "Фактура";
            }
            else if (type == "Credit")
            {
                return "Кредитно известие";
            }
            else if (type == "Debit")
            {
                return "Дебитно известие";
            }
            return null;
        }
    }
}
