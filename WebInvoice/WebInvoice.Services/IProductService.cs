using System.Collections.Generic;
using System.Threading.Tasks;
using WebInvoice.Dto.Product;

namespace WebInvoice.Services
{
    public interface IProductService
    {
        Task Create(ProductDto productDto);
        Task Edit(ProductDto productDto);
        Task<ICollection<ProductDto>> GetAllProduct();
        Task<ProductDto> GetById(int id);
        Task<IEnumerable<ProductDto>> FindProductAsync(string name);
        Task<PaginatedList<ProductDto>> GetPaginatedProductsAsync(int page);
        Task<decimal?> AddQuantity(int productId, decimal quantity);
    }
}