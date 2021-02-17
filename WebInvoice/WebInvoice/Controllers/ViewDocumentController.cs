using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    public class ViewDocumentController : Controller
    {
        private readonly IViewDocumentService viewDocumentService;

        public ViewDocumentController(IViewDocumentService viewDocumentService)
        {
            this.viewDocumentService = viewDocumentService;
        }
        public async Task<IActionResult> Index(long id)
        {
            var model = await viewDocumentService.GetDocumetnById(5000000011);
            return View();
        }

    }
}
