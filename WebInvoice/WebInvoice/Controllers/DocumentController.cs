﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.Document;
using WebInvoice.Dto.Product;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly ICompanyDeletableEntityRepository<Company> companyRepository;

        public DocumentController(ICompanyDeletableEntityRepository<Company> companyRepository)
        {
            this.companyRepository = companyRepository;
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
            var list = new List<ProductShortDto>();
            list.Add(new ProductShortDto() {Name="test",  ProductId=1, Price=1.2m, ProductType="br", Quantity=5, TottalPrice=6.0m, IsProduct=true, AvailableQuantity=2 });
            list.Add(new ProductShortDto() { ProductId = 2, Price = 6.2m, ProductType = "butilka", Quantity = 1, TottalPrice = 6.2m ,IsProduct=false });
            model.Sales.AddRange(list);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VatDocumentDto vatDocumentDto)
        {
            return View(vatDocumentDto);
        }
    }
}