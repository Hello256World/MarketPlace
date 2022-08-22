using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.DataLayer.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        #region product

        Task<FilterProductDTO> FilterProduct(FilterProductDTO filter);

        Task<CreateProductResult> CreateProduct(CreateProductDTO create, IFormFile productImage, long sellerId);

        #endregion

        #region product categories

        Task<List<ProductCategory>> GetAllProductCategoryByParentId(long? parentId);
        Task<List<ProductCategory>> GetActiveProductCategory();

        #endregion
    }
}
