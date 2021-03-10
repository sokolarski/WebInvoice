using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;
using WebInvoice.Services;
using WebInvoice.Services.Reports;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class ReportDocumentsController : Controller
    {
        private readonly IReportVatDocumentsService reportVatDocumentService;
        private readonly IReportNonVatDocumentsService reportNonVatDocumentService;

        public ReportDocumentsController(IReportVatDocumentsService reportVatDocumentService, IReportNonVatDocumentsService reportNonVatDocumentService)
        {
            this.reportVatDocumentService = reportVatDocumentService;
            this.reportNonVatDocumentService = reportNonVatDocumentService;
        }
      

        public async Task<IActionResult> GetReportByCriteria(int? pageNumber, long? documentId, string partnerName, string type, string startDate, string endDate, string objectGuid)
        {
            Report model;
            int? itemPerPage = 20;
            if (type == "invoice" || type == "credit" || type == "debit" || type == "vatDocument")
            {
                model = await reportVatDocumentService.GetPaginatedVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10, documentId, partnerName, type, startDate, endDate, objectGuid);
            }
            else if (type == "proformInvoice" || type == "protocol" || type == "stock" || type == "nonVatDocument")
            {
                model = await reportNonVatDocumentService.GetPaginatedNonVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10, documentId, partnerName, type, startDate, endDate, objectGuid);
            }
            else
            {
                model = await reportVatDocumentService.GetPaginatedVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10, documentId, partnerName, type, startDate, endDate, objectGuid);
            }
            this.ViewBag.documentId = documentId;
            this.ViewBag.partnerName = partnerName;
            this.ViewBag.type = type;
            this.ViewBag.objGuid = objectGuid;
            this.ViewBag.startDate = startDate;
            this.ViewBag.endDate = endDate;
            this.ViewBag.itemPerPage = itemPerPage;
            return View(model);
        }

        public async Task<IActionResult> GetReportPdfCriteria(int? pageNumber, long? documentId, string partnerName, string type, string startDate, string endDate, string objectGuid)
        {
            ReportExport model;
            int? itemPerPage = 20;
            if (type == "invoice" || type == "credit" || type == "debit" || type == "vatDocument")
            {
                model = await reportVatDocumentService.ExportVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10, documentId, partnerName, type, startDate, endDate, objectGuid);
            }
            else if (type == "proformInvoice" || type == "protocol" || type == "stock" || type == "nonVatDocument")
            {
                model = await reportNonVatDocumentService.ExportNonVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10, documentId, partnerName, type, startDate, endDate, objectGuid);
            }
            else
            {
                model = await reportVatDocumentService.ExportVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10, documentId, partnerName, type, startDate, endDate, objectGuid);

            }

            var txtHtml = await this.RenderViewAsync("GetReportPdfCriteria", model);
            var documentName = $"Report.pdf";
            var stream = new MemoryStream();

            HtmlToPdf converter = new HtmlToPdf(1100);
            converter.Options.MarginTop = 15;
            converter.Options.MarginBottom = 30;
            PdfDocument doc = converter.ConvertHtmlString(txtHtml);
            doc.Save(stream);
            doc.Close();

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", documentName);
        }
    }
}
