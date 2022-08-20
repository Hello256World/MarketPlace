using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Account;
using MarketPlace.Web.PresentationExtension;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region constructor

        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;

        public AccountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region change password

        [HttpGet("change-pass")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("change-pass"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO change)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(change.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(change);
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.ChangePassword(change,User.GetUserId());
                switch (res)
                {
                    case ChangePasswordResult.WrongCurrentPass:
                        TempData[ErrorMessage] = "رمز فعلی وارد شده اشتباه می باشد";
                        ModelState.AddModelError("CurrentPassword", "رمز وارد شده اشتباه می باشد");
                        break;
                    case ChangePasswordResult.DuplicatPassword:
                        TempData[WarningMessage] = "لطفا از کلمه‌ی عبور جدیدی استفاده کنید";
                        break;
                    case ChangePasswordResult.Success:
                        TempData[SuccessMessage] = "رمز شما با موفقیت تغییر پیدا کرد";
                        TempData[InfoMessage] = "لطفا مجدد وارد سایت شوید";
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("Login", "Account", new {area = ""});
                }
            }

            return View(change);
        }

        #endregion

        #region edit profile

        [HttpGet("edit-profile")]
        public async Task<IActionResult> EditProfile()
        {
            var userProfile = await _userService.GetUsreProfile(User.GetUserId());
            if (userProfile == null) return NotFound();

            return View(userProfile);
        }

        [HttpPost("edit-profile"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileDTO profile, IFormFile? avatarImage) 
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(profile.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(profile);
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.EditUserProfile(profile, User.GetUserId(),avatarImage);
                switch (res)
                {
                    case EditProfileResult.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case EditProfileResult.Success:
                        TempData[SuccessMessage] = $"جناب {profile.FirstName} {profile.LastName} ، اطلاعات شما با موفقیت ویرایش شد";
                        return RedirectToAction("EditProfile");
                }
            }

            return View(profile);
        }

        #endregion
    }
}
