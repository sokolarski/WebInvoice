using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Models;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class ViewNonVatDocumentController : Controller
    {
        private readonly IViewNonVatDocumentService viewDocumentService;

        public ViewNonVatDocumentController(IViewNonVatDocumentService viewDocumentService)
        {
            this.viewDocumentService = viewDocumentService;
        }
        public async Task<IActionResult> ViewNonVatDocument(long id)
        {
            var model = await viewDocumentService.GetDocumetnById(id);
            if (model is null)
            {
                return View("NotFoundItem");
            }
            return View(model);
        }

        public async Task<IActionResult> ExportPdfOriginal(long id)
        {
            var model = await viewDocumentService.GetDocumetnById(id);
            var txtHtml = await this.RenderViewAsync("PdfDocument", model);
            var documentName = $"{model.Company.Name}-{model.Id}.pdf";
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

        public async Task<IActionResult> ExportPdfCopy(long id)
        {
            var model = await viewDocumentService.GetDocumetnById(id);

            model.Grif = "КОПИЕ";

            var txtHtml = await this.RenderViewAsync("PdfDocument", model);
            var documentName = $"{model.Company.Name}-{model.Id}.pdf";

            var stream = new MemoryStream();

            HtmlToPdf converter = new HtmlToPdf(1100);
            PdfDocument doc = converter.ConvertHtmlString(txtHtml);
            doc.Save(stream);
            doc.Close();

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", documentName);
        }

        public async Task<IActionResult> PdfDocument(long id)
        {
            var model = await viewDocumentService.GetDocumetnById(id);

            return View(model);
        }
    }
}
