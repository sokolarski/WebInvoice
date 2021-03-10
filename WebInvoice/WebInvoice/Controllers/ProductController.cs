using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Dto.Product;
using WebInvoice.Services;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IVatTypeService vatTypeService;
        private readonly IQuantityTypeService quantityTypeService;

        public ProductController(IProductService productService, IVatTypeService vatTypeService, IQuantityTypeService quantityTypeService)
        {
            this.productService = productService;
            this.vatTypeService = vatTypeService;
            this.quantityTypeService = quantityTypeService;
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var model = await productService.GetPaginatedProductsAsync(pageNumber ?? 1);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var vatTypes =await vatTypeService.GetAllView();
            this.ViewBag.SelectVatType = vatTypes.Select(vt => new SelectListItem(vt.Name, vt.Id.ToString(), vt.IsActive));
            var quantityTypes = await quantityTypeService.GetAllView();
            this.ViewBag.SelectQuantityType = quantityTypes.Select(qt => new SelectListItem(qt.Type,qt.Id.ToString(),qt.IsActive));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await productService.Create(productDto);
                return RedirectToAction("Index");
            }
            return View(productDto);
        }

        public async Task<IActionResult> Search(string name)
        {

            var result = await productService.FindProductAsync(name);
            return Json(result);

        }

        public async Task<IActionResult> FindProductDataListAjax(string name)
        {
            var result = await productService.FindProductDataListAsync(name);
            return Json(result);
        }

        public async Task<IActionResult> GetProductByNameAjax(string name)
        {
            var result = await productService.GetProductByNameAsync(name);
            return Json(result);
        }

        public async Task<IActionResult> Edit(int productId)
        {
            var model = await productService.GetById(productId);

            var vatTypes =await vatTypeService.GetAllView();
            this.ViewBag.SelectVatType = vatTypes.Select(vt => new SelectListItem(vt.Name, vt.Id.ToString(), vt.Id == model.VatTypeId));
            var quantityTypes = await quantityTypeService.GetAllView();
            this.ViewBag.SelectQuantityType = quantityTypes.Select(qt => new SelectListItem(qt.Type, qt.Id.ToString()));

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await productService.Edit(productDto);
                return RedirectToAction("Index");
            }

            var vatTypes = await vatTypeService.GetAllView();
            this.ViewBag.SelectVatType = vatTypes.Select(vt => new SelectListItem(vt.Name, vt.Id.ToString(), vt.Id == productDto.VatTypeId));
            var quantityTypes = await quantityTypeService.GetAllView();
            this.ViewBag.SelectQuantityType = quantityTypes.Select(qt => new SelectListItem(qt.Type, qt.Id.ToString()));
            
            return View(productDto);
        }

        public async Task<IActionResult> AddQuantity(int productId, decimal quantity)
        {
            return Json(await productService.AddQuantity(productId, quantity));
        }
    }
}
