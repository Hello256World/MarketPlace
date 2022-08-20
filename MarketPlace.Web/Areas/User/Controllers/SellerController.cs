using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.Web.PresentationExtension;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class SellerController : UserBaseController
    {
        #region constructor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region request seller panel

        [HttpGet("request-seller-panel")]
        public IActionResult RequestSellerPanel()
        {
            return View();
        }

        [HttpPost("request-seller-panel"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestSellerPanel(RequestSellerDTO request)
        {
            if (ModelState.IsValid)
            {
                var res = await _sellerService.AddRequestSeller(request, User.GetUserId());
                switch (res)
                {
                    case RequestSellerResult.NotPermision:
                        TempData[ErrorMessage] = "شما اجازه دسترسی به درخواست پنل فروشندگی را ندارید";
                        break;
                    case RequestSellerResult.HasUnderProgress:
                        TempData[WarningMessage] = "شما یک درخواست پنل فروشندگی در حال بررسی دارید";
                        TempData[InfoMessage] = "فقط می توان یک درخواست درحال بررسی داشت";
                        break;
                    case RequestSellerResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ثبت شد";
                        return RedirectToAction("SellerRequests", "Seller");
                }
            }

            return View(request);
        }

        #endregion

        #region seller requests

        [HttpGet("seller-requests")]
        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            filter.UserId = User.GetUserId();
            filter.State = FilterSellerState.All;
            filter.TakeEntity = 5;

            var res = await _sellerService.FilterSeller(filter);

            return View(res);
        }

        #endregion

        #region edite request seller

        [HttpGet("edite-request-seller/{sellerId}")]
        public async Task<IActionResult> EditeRequestSeller(long sellerId)
        {
            var sellerDetail = await _sellerService.GetRequestSellerForEdite(sellerId, User.GetUserId());
            if (sellerDetail == null) return NotFound();

            return View(sellerDetail);
        }

        [HttpPost("edite-request-seller/{sellerId}")]
        public async Task<IActionResult> EditeRequestSeller(EditeRequestSellerDTO request)
        {
            if (ModelState.IsValid)
            {
                var res = await _sellerService.EditeSellerRequest(request, User.GetUserId());

                switch (res)
                {
                    case EditeRequestSellerResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                        break;
                    case EditeRequestSellerResult.Success:
                        TempData[SuccessMessage] = "اطلاعات شما با موفقیت ویرایش شد";
                        TempData[InfoMessage] = "فرایند تایید اطلاعات شما از سر گرفته شد";
                        return RedirectToAction("SellerRequests", "Seller", new { area = "User" });
                }
            }

            return View(request);
        }

        #endregion
    }
}
