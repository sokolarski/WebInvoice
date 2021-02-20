using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    public class ViewVatDocumentController : Controller
    {
        private readonly IViewVatDocumentService viewDocumentService;

        public ViewVatDocumentController(IViewVatDocumentService viewDocumentService)
        {
            this.viewDocumentService = viewDocumentService;
        }
        public async Task<IActionResult> ViewVatDocument(long id)
        {
            var model = await viewDocumentService.GetDocumetnById(id);
            return View(model);
        }

    }
}
