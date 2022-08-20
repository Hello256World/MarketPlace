using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.DataLayer.Entities.Products;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        #region product

        Task<FilterProductDTO> FilterProduct(FilterProductDTO filter);

        #endregion

        #region product categories

        Task<List<ProductCategory>> GetAllProductCategory(long? parentId);

        #endregion
    }
}
