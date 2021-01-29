using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Dto.Product;
using WebInvoice.Dto.QuantityType;
using WebInvoice.Dto.VatType;

namespace WebInvoice.Services
{
    public class ProductService : IProductService
    {
        private readonly ICompanyDeletableEntityRepository<Product> productRepositoy;

        public ProductService(ICompanyDeletableEntityRepository<Product> productRepository)
        {
            this.productRepositoy = productRepository;
        }

        public async Task<ICollection<ProductDto>> GetAllProduct()
        {
            var products = await productRepositoy.AllAsNoTracking().Select(e => new ProductDto()
            {
                Name = e.Name,
                Id = e.Id,
                Description = e.Description,
                Barcode = e.Barcode,
                Price = e.Price,
                BasePrice = e.BasePrice,
                Quantity = e.Quantity,
                PriceWithVat = e.Price * ((e.VatType.Percantage / 100) + 1),
                VatTypeId = e.VatTypeId,
                VatType = new VatTypeView() { Name = e.VatType.Name + "-" + e.VatType.Percantage.ToString("F2") + "%", Id = e.VatType.Id },
                QuantityTypeId = e.QuantityTypeId,
                QuantityType = new QuantityTypeShortView { Id = e.QuantityType.Id, Type = e.QuantityType.Type },
            }).ToListAsync();
            return products;
        }
        public async Task<ProductDto> GetById(int id)
        {
            var product = await productRepositoy.AllAsNoTracking().Where(e => e.Id == id).Select(e => new ProductDto()
            {
                Name = e.Name,
                Id = e.Id,
                Description = e.Description,
                Barcode = e.Barcode,
                Price = e.Price,
                BasePrice = e.BasePrice,
                PriceWithVat = e.Price * ((e.VatType.Percantage / 100) + 1),
                Quantity = e.Quantity,
                VatTypeId = e.VatTypeId,
                VatType = new VatTypeView() { Name = e.VatType.Name + "-" + e.VatType.Percantage.ToString("F2") + "%", Id = e.VatType.Id },
                QuantityTypeId = e.QuantityTypeId,
                QuantityType = new QuantityTypeShortView { Id = e.QuantityType.Id, Type = e.QuantityType.Type },
            }).FirstOrDefaultAsync();

            return product;
        }

        public async Task<PaginatedList<ProductDto>> GetPaginatedProductsAsync(int page)
        {
            int itemPerPage = 10;
            var query = productRepositoy.AllAsNoTracking().OrderBy(e => e.Name).Select(e => new ProductDto()
            {
                Name = e.Name,
                Id = e.Id,
                Description = e.Description,
                Barcode = e.Barcode,
                Price = e.Price,
                BasePrice = e.BasePrice,
                Quantity = e.Quantity,
                PriceWithVat = e.Price * ((e.VatType.Percantage / 100) + 1),
                VatTypeId = e.VatTypeId,
                VatType = new VatTypeView() { Name = e.VatType.Name + "-" + e.VatType.Percantage.ToString("F2") + "%", Id = e.VatType.Id },
                QuantityTypeId = e.QuantityTypeId,
                QuantityType = new QuantityTypeShortView { Id = e.QuantityType.Id, Type = e.QuantityType.Type },
            });
            var result = await PaginatedList<ProductDto>.CreateAsync(query, page, itemPerPage);
            return result;
        }

        public async Task<IEnumerable<ProductDto>> FindProductAsync(string name)
        {
            var result = await productRepositoy.AllAsNoTracking().OrderBy(p => p.Name).Where(p => p.Name.Contains(name) == true).Select(e => new ProductDto()
            {
                Name = e.Name,
                Id = e.Id,
                Description = e.Description,
                Barcode = e.Barcode,
                Price = e.Price,
                BasePrice = e.BasePrice,
                Quantity = e.Quantity,
                PriceWithVat = e.Price * ((e.VatType.Percantage / 100) + 1),
                VatTypeId = e.VatTypeId,
                VatType = new VatTypeView() { Name = e.VatType.Name + "-" + e.VatType.Percantage.ToString("F2") + "%", Id = e.VatType.Id },
                QuantityTypeId = e.QuantityTypeId,
                QuantityType = new QuantityTypeShortView { Id = e.QuantityType.Id, Type = e.QuantityType.Type },
            }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ProductFindDto>> FindProductDataListAsync(string name)
        {
            var result = await productRepositoy.AllAsNoTracking().OrderBy(p => p.Name).Where(p => p.Name.Contains(name) == true).Select(e => new ProductFindDto()
            {
                Name = e.Name,
                Id = e.Id,
            }).ToListAsync();

            return result;
        }

        public async Task<ProductShortDto> GetProductByNameAsync(string name)
        {
            var result = await productRepositoy.AllAsNoTracking().Where(p => p.Name == name).Select(e => new ProductShortDto()
            {
                ProductId = e.Id,
                Name = e.Name,
                ProductType = e.QuantityType.Type,
                AvailableQuantity = e.Quantity,
                Price = e.Price,
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task Create(ProductDto productDto)
        {
            var company = await productRepositoy.Context.Companies.OrderBy(c => c.Id).LastOrDefaultAsync();
            var product = new Product()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Barcode = productDto.Barcode,
                Price = productDto.Price,
                BasePrice = productDto.BasePrice,
                Quantity = productDto.Quantity,
                VatTypeId = productDto.VatTypeId,
                QuantityTypeId = productDto.QuantityTypeId,
                CompanyId = company.Id,

            };

            await productRepositoy.AddAsync(product);
            await productRepositoy.SaveChangesAsync();
        }

        public async Task Edit(ProductDto productDto)
        {
            var product = productRepositoy.All().Where(e => e.Id == productDto.Id).FirstOrDefault();
            if (productDto.Id != 0 && product != null)
            {
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Barcode = productDto.Barcode;
                product.Price = productDto.Price;
                product.BasePrice = productDto.BasePrice;
                product.Quantity = productDto.Quantity;
                product.VatTypeId = productDto.VatTypeId;
                product.QuantityTypeId = productDto.QuantityTypeId;

                productRepositoy.Update(product);
                await productRepositoy.SaveChangesAsync();
            }
        }

        public async Task<decimal?> AddQuantity(int productId, decimal quantity)
        {
            var product = await productRepositoy.All().Where(p => p.Id == productId).FirstOrDefaultAsync();
            decimal? updatedQuantity = null;
            if (product != null)
            {
                product.Quantity += quantity;

                productRepositoy.Update(product);
                updatedQuantity = product.Quantity;
                await productRepositoy.SaveChangesAsync();
            }
            return updatedQuantity;
        }

    }
}
