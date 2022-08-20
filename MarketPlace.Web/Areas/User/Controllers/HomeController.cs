using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
        #region user panel

        public IActionResult Dashboard()
        {
            return View();
        }

        #endregion
    }
}
