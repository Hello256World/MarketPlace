using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Contacts;
using MarketPlace.Web.PresentationExtension;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MarketPlace.DataLayer.Entities.Site;

namespace MarketPlace.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region constructor

        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly ISiteService _siteService;

        public HomeController(IContactService contactService, ICaptchaValidator captchaValidator, ISiteService siteService)
        {
            _contactService = contactService;
            _captchaValidator = captchaValidator;
            _siteService = siteService;
        }

        #endregion

        #region index

        public async Task<IActionResult> Index()
        {
            ViewBag.banners = await _siteService.GetSiteBanners(new List<BannerPlacement>
            {
                BannerPlacement.Home_1,
                BannerPlacement.Home_2,
                BannerPlacement.Home_3
            });

            return View();
        }

        #endregion

        #region contact us

        [HttpGet("contact-us")]
        public async Task<IActionResult> ContactUs()
        {
            ViewBag.siteSetting = await _siteService.GetDeafultSiteSetting();

            return View();
        }

        [HttpPost("contact-us"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsDTO create)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(create.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(create);
            }

            if (ModelState.IsValid)
            {
                await _contactService.CreateContactUs(create,User.GetUserId(),HttpContext.GetUserIp());
                TempData[SuccessMessage] = "پیام شما با موفقیت ثبت شد";
                return RedirectToAction("ContactUs");
            }

            return View(create);
        }

        #endregion

        #region about us

        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs()
		{
            var siteSettings = await _siteService.GetDeafultSiteSetting();

            return View(siteSettings);
		}

        #endregion
    }
}