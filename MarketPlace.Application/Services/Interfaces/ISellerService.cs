using MarketPlace.DataLayer.DTOs.Common;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.DataLayer.Entities.Store;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface ISellerService : IAsyncDisposable
    {
        #region seller

        Task<RequestSellerResult> AddRequestSeller(RequestSellerDTO request, long userId);
        Task<FilterSellerDTO> FilterSeller(FilterSellerDTO filter);
        Task<EditeRequestSellerDTO> GetRequestSellerForEdite(long sellerId,long userId);
        Task<EditeRequestSellerResult> EditeSellerRequest(EditeRequestSellerDTO request, long userId);
        Task<bool> AcceptSellerRequest(long sellerId);
        Task<bool> RejectSellerRequest(RejectItemDTO reject);
        Task<Seller> GetSellerForProductFilter(long userId);

        #endregion
    }
}
