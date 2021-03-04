using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.ViewDocument;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class ViewDocumentsController : Controller
    {
        private readonly ISearchVatDocumentService searchVatDocumentService;
        private readonly ISearchNonVatDocumentService searchNonVatDocumentService;

        public ViewDocumentsController(ISearchVatDocumentService searchVatDocumentService, ISearchNonVatDocumentService searchNonVatDocumentService)
        {
            this.searchVatDocumentService = searchVatDocumentService;
            this.searchNonVatDocumentService = searchNonVatDocumentService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetDocumentsByCriteria(int? pageNumber, int? itemPerPage, long? documentId, string partnerName, string type, string startDate, string endDate, string objectGuid)
        {
            PaginatedList<DocumentShortView> model;

            if (type == "invoice" || type == "credit" || type == "debit" || type == "vatDocument")
            {
               model = await searchVatDocumentService.GetPaginatedVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10, documentId, partnerName, type, startDate, endDate, objectGuid);
            }
            else if (type == "proformInvoice" || type == "protocol" || type == "stock" || type == "nonVatDocument")
            {
                model = await searchNonVatDocumentService.GetPaginatedNonVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10, documentId, partnerName, type, startDate, endDate, objectGuid);
            }
            else
            {
                model = await searchVatDocumentService.GetPaginatedVatDocumentByCriteriaAsync(pageNumber ?? 1, itemPerPage ?? 10, documentId, partnerName, type, startDate, endDate, objectGuid);
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
    }
}
