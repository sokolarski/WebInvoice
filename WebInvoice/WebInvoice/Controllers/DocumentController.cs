using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.Document;
using WebInvoice.Dto.Product;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly ICompanyDeletableEntityRepository<Company> companyRepository;
        private readonly IVatTypeService vatTypeService;

        public DocumentController(ICompanyDeletableEntityRepository<Company> companyRepository, IVatTypeService vatTypeService)
        {
            this.companyRepository = companyRepository;
            this.vatTypeService = vatTypeService;
        }
        public IActionResult Index()
        {
            var name = companyRepository.All().FirstOrDefault();
            return Json(name);
        }

        public async Task<IActionResult> Create()
        {
            var model = new VatDocumentDto();
            model.PartnerId = 7;
            var list = new List<ProductDocumentDto>();
            list.Add(new ProductDocumentDto() { Name = "test", ProductId = 1, Price = 1.2m, ProductType = "br", Quantity = 5, TottalPrice = 6.0m, IsProduct = true, AvailableQuantity = 2, VatTypeId = 1 });
            list.Add(new ProductDocumentDto() { Name = "fr", ProductId = 2, Price = 1.8m, ProductType = "br", Quantity = 2, TottalPrice = 6.0m, IsProduct = true, AvailableQuantity = 2, VatTypeId = 1 });
            list.Add(new ProductDocumentDto() { Name="as", Price = 6.2m, ProductType = "butilka", Quantity = 1, TottalPrice = 6.2m ,IsProduct=false ,VatTypeId=2 });
            model.Sales.AddRange(list);
            model.CreatedDate = DateTime.Now.ToString("dd.MM.yyyy");
            var vatTypes = vatTypeService.GetAll();
            this.ViewBag.VatTypes = JsonConvert.SerializeObject(vatTypes);
            this.ViewBag.SalesJson= JsonConvert.SerializeObject(list);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VatDocumentDto vatDocumentDto)
        {
            var vatTypes = vatTypeService.GetAll();
            this.ViewBag.VatTypes = JsonConvert.SerializeObject(vatTypes);
            this.ViewBag.SalesJson = JsonConvert.SerializeObject(vatDocumentDto.Sales);
            
            return View(vatDocumentDto);
        }
    }
}
