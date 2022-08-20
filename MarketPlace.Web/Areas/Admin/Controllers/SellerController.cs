using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Common;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.Admin.Controllers
{
    public class SellerController : AdminBaseController
    {
        #region constructor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region seller requests

        [HttpGet("seller-requests")]
        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            filter.TakeEntity = 5;

            return View(await _sellerService.FilterSeller(filter));
        }

        #endregion

        #region accept seller request

        [Route("AcceptSellerRequest")]
        public async Task<IActionResult> AcceptSellerRequest(long sellerId)
        {
            var res = await _sellerService.AcceptSellerRequest(sellerId);
            if (res)
            {
                return JsonResponseState.SendResult(
                    JsonResponseType.Success,
                    "درخواست فروشندگی تایید شد"
                    );
            }

            return JsonResponseState.SendResult(
                JsonResponseType.Danger,
                "درخواست فروشندگی تایید نشد"
                );
        }

        #endregion

        #region reject seller request

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSellerRequest(RejectItemDTO reject)
        {
            var res = await _sellerService.RejectSellerRequest(reject);

            if (res)
            {
                return JsonResponseState.SendResult(JsonResponseType.Success,
                    "درخواست فروشندگی با موفقیت رد شد"
                    ,reject);
            }

            return JsonResponseState.SendResult(JsonResponseType.Danger, "اطلاعاتی با این مشخصات یافت نشد");
        }


        #endregion
    }
}
