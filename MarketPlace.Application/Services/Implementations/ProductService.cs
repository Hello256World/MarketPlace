using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.DataLayer.Entities.Products;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region constructor

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
        private readonly IGenericRepository<ProductSelectedCategory> _productSelectedCategoryRepository;

        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategoryRepository, IGenericRepository<ProductSelectedCategory> productSelectedCategoryRepository)
        {
            _productSelectedCategoryRepository = productSelectedCategoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region product

        public async Task<FilterProductDTO> FilterProduct(FilterProductDTO filter)
        {
            var query = _productRepository.GetQuery().Where(x => !x.IsDelete);

            #region state

            switch (filter.FilterProductState)
            {
                case FilterProductState.All:
                    break;
                case FilterProductState.UnderProgress:
                    query = query.Where(x => x.ProductAcceptanceState == ProductAcceptanceState.UnderProgress);
                    break;
                case FilterProductState.Accepted:
                    query = query.Where(x => x.ProductAcceptanceState == ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.Rejected:
                    query = query.Where(x => x.ProductAcceptanceState == ProductAcceptanceState.Rejected);
                    break;
                case FilterProductState.Active:
                    query = query.Where(x => x.IsActive);
                    break;
                case FilterProductState.NotActive:
                    query = query.Where(x => !x.IsActive);
                    break;
            }

            #endregion

            #region filter

            if (!string.IsNullOrEmpty(filter.ProductTitle))
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.ProductTitle}%"));

            if (filter.SellerId != null && filter.SellerId.Value != 0)
                query = query.Where(x => x.SellerId == filter.SellerId.Value);

            #endregion

            #region paging

            var pager = Pager.Builder(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetProducts(allEntities);
        }

        #endregion

        #region product categories

        public async Task<List<ProductCategory>> GetAllProductCategory(long? parentId)
        {
            if (parentId != null || parentId != 0)
            {
                return await _productCategoryRepository.GetQuery().Where(x => x.IsActive && !x.IsDelete && x.ParentId == parentId).ToListAsync();
            }

            return await _productCategoryRepository.GetQuery().Where(x => x.IsActive && !x.IsDelete).ToListAsync();
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            if (_productRepository != null) await _productRepository.DisposeAsync();
            if (_productCategoryRepository != null) await _productCategoryRepository.DisposeAsync();
            if (_productSelectedCategoryRepository != null) await _productSelectedCategoryRepository.DisposeAsync();
        }

        #endregion

    }
}
