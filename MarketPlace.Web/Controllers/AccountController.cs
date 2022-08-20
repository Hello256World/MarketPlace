using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketPlace.Web.Controllers
{
    public class AccountController : SiteBaseController
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

        #region register

        [HttpGet("register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(register);
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.RegisterUser(register);
                switch (res)
                {
                    case RegisterUserResult.UserExist:
                        TempData[ErrorMessage] = "ایمیل وارد شده تکراری می باشد";
                        break;
                    case RegisterUserResult.Success:
                        TempData[SuccessMessage] = "ثبت نام شما با موفقیت انجام شد";
                        TempData[InfoMessage] = "کد فعالسازی برای ایمیل شما ارسال شد";
                        return RedirectToAction("ActivateEmail", "Account", new { email = register.Email });
                }
            }

            return View(register);
        }

        #endregion

        #region activate account

        [HttpGet("activate-email/{email}")]
        public IActionResult ActivateEmail(string email)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            var activateDto = new ActivateEmailDTO { Email = email };

            return View(activateDto);
        }

        [HttpPost("activate-email/{email}")]
        public async Task<IActionResult> ActivateEmail(ActivateEmailDTO activate)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(activate.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(activate);
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.ActivateEmail(activate);
                if (res)
                {
                    TempData[SuccessMessage] = "اکانت شما با موفقیت فعال شد";
                    return RedirectToAction("Login");
                }
                   
                TempData[WarningMessage] = "کد وارد شده معتبر نمی باشد";
            }

            return View(activate);
        }

        #endregion

        #region login

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO login, string? returnUrl)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(login);
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.LoginUser(login);
                switch (res)
                {
                    case LoginUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case LoginUserResult.NotActive:
                        TempData[WarningMessage] = "اکانت مورد نظر فعال نمی باشد";
                        break;
                    case LoginUserResult.Success:

                        var user = await _userService.GetUserByEmail(login.Email);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.FirstName),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email)
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe,
                        };

                        await HttpContext.SignInAsync(principal, properties);

                        TempData[SuccessMessage] = "ورود شما با موفقیت انجام شد";
                        if (returnUrl != null)
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                }
            }

            return View(login);
        }


        #endregion

        #region forgot password

        [HttpGet("forgot-pass")]
        public IActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("forgot-pass"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(forgot);
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.RecoverPassword(forgot);

                switch (res)
                {
                    case ForgotPasswordResult.NotFound:
                        TempData[ErrorMessage] = "ایمیل وارد شده یافت نشد";
                        break;
                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = "رمز عبور جدید با موفقیت برای ایمیل شما ارسال شد";
                        TempData[InfoMessage] = "بعد از وارد شدن به حساب کاربری رمز خود را عوض کنید";
                        return RedirectToAction("Login");
                }
            }

            return View(forgot);
        }

        #endregion

        #region log out

        [HttpGet("log-out")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        #endregion
    }
}
