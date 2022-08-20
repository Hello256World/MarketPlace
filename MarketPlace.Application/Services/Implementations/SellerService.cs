using MarketPlace.Application.Extensions;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Common;
using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Store;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementations
{
    public class SellerService : ISellerService
    {
        #region constructor

        private readonly IGenericRepository<Seller> _sellerRepository;
        private readonly IGenericRepository<User> _userRepository;

        public SellerService(IGenericRepository<Seller> sellerRepository, IGenericRepository<User> userRepository)
        {
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region seller

        public async Task<RequestSellerResult> AddRequestSeller(RequestSellerDTO request, long userId)
        {
            var user = await _userRepository.GetEntityById(userId);

            if (user.IsBlock) return RequestSellerResult.NotPermision;

            var hasUnderProgress = await _sellerRepository.GetQuery().AnyAsync(x => x.UserId == userId && x.StoreAcceptanceState == StoreAcceptanceState.UnderProgress);

            if (hasUnderProgress) return RequestSellerResult.HasUnderProgress;

            var newSeller = new Seller
            {
                Address = request.Address.Sanitize(),
                Phone = request.Phone.Sanitize(),
                StoreName = request.StoreName.Sanitize(),
                StoreAcceptanceState = StoreAcceptanceState.UnderProgress,
                UserId = userId,
            };

            await _sellerRepository.AddEntity(newSeller);
            await _sellerRepository.SaveChanges();

            return RequestSellerResult.Success;
        }

        public async Task<FilterSellerDTO> FilterSeller(FilterSellerDTO filter)
        {
            var query = _sellerRepository.GetQuery().Include(x => x.User).Where(x => !x.IsDelete);

            #region state

            switch (filter.State)
            {
                case FilterSellerState.All:
                    break;
                case FilterSellerState.UnderProgress:
                    query = query.Where(x => x.StoreAcceptanceState == StoreAcceptanceState.UnderProgress);
                    break;
                case FilterSellerState.Accepted:
                    query = query.Where(x => x.StoreAcceptanceState == StoreAcceptanceState.Accepted);
                    break;
                case FilterSellerState.Rejected:
                    query = query.Where(x => x.StoreAcceptanceState == StoreAcceptanceState.Rejected);
                    break;
            }

            #endregion

            #region filter

            if (filter.UserId != null && filter.UserId.Value != 0)
                query = query.Where(x => x.UserId == filter.UserId);

            if (filter.StoreName != null)
                query = query.Where(x => EF.Functions.Like(x.StoreName, $"%{filter.StoreName}%"));

            if (filter.Phone != null)
                query = query.Where(x => EF.Functions.Like(x.Phone, $"%{filter.Phone}%"));

            if (filter.Address != null)
                query = query.Where(x => EF.Functions.Like(x.Address, $"%{filter.Address}%"));

            #endregion

            #region paging

            var pager = Pager.Builder(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntites = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetSellers(allEntites);
        }

        public async Task<EditeRequestSellerDTO> GetRequestSellerForEdite(long sellerId, long userId)
        {
            var seller = await _sellerRepository.GetEntityById(sellerId);

            if (seller == null || seller.UserId != userId) return null;

            return new EditeRequestSellerDTO
            {
                SellerId = sellerId,
                Address = seller.Address,
                Phone = seller.Phone,
                StoreName = seller.StoreName,
            };
        }

        public async Task<EditeRequestSellerResult> EditeSellerRequest(EditeRequestSellerDTO request, long userId)
        {
            var seller = await _sellerRepository.GetEntityById(request.SellerId);

            if (seller == null || seller.UserId != userId || seller.Id != request.SellerId) return EditeRequestSellerResult.NotFound;

            seller.StoreName = request.StoreName;
            seller.Address = request.Address;
            seller.Phone = request.Phone;
            seller.StoreAcceptanceState = StoreAcceptanceState.UnderProgress;
            _sellerRepository.EditeEntity(seller);
            await _sellerRepository.SaveChanges();

            return EditeRequestSellerResult.Success;
        }

        public async Task<bool> AcceptSellerRequest(long sellerId)
        {
            var seller = await _sellerRepository.GetEntityById(sellerId);

            if (seller != null)
            {
                seller.StoreAcceptanceState = StoreAcceptanceState.Accepted;
                seller.StoreAcceptanceDescription = "درخواست فروشندگی شما با موفقیت تایید شد";
                _sellerRepository.EditeEntity(seller);
                await _sellerRepository.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<bool> RejectSellerRequest(RejectItemDTO reject)
        {
            var seller = await _sellerRepository.GetEntityById(reject.Id);

            if (seller != null)
            {
                seller.StoreAcceptanceState = StoreAcceptanceState.Rejected;
                seller.StoreAcceptanceDescription = reject.RejectMessage.Sanitize();
                _sellerRepository.EditeEntity(seller);
                await _sellerRepository.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<Seller> GetSellerForProductFilter(long userId)
        {
            return await _sellerRepository.GetQuery().OrderByDescending(x => x.CreateDate)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.StoreAcceptanceState == StoreAcceptanceState.Accepted);
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {

            if (_sellerRepository != null) await _sellerRepository.DisposeAsync();
            if (_userRepository != null) await _userRepository.DisposeAsync();
        }

        #endregion
    }
}
