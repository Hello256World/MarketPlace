using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    public class HomeController : SellerBaseController
    {
        #region index

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
