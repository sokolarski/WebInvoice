using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class ViewDocumentsController : Controller
    {
        private readonly ISearchVatDocumentService searchVatDocumentService;

        public ViewDocumentsController(ISearchVatDocumentService searchVatDocumentService)
        {
            this.searchVatDocumentService = searchVatDocumentService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetVatDocuments(int? pageNumber, int? itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate)
        {
            var model = await searchVatDocumentService.GetPaginatedVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10,documentId,partnerName,type,startDate,endDate);

            this.ViewBag.documentId = documentId;
            this.ViewBag.partnerName = partnerName;
            this.ViewBag.type = type;
            this.ViewBag.startDate = startDate;
            this.ViewBag.endDate = endDate;
            this.ViewBag.itemPerPage = itemPerPage;
            return View(model);
        }
    }
}
