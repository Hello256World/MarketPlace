using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Contacts;
using MarketPlace.Web.PresentationExtension;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region constructor

        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;

        public TicketController(IContactService contactService, ICaptchaValidator captchaValidator)
        {
            _contactService = contactService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region tickets list

        [HttpGet("tickets")]
        public async Task<IActionResult> Index(FilterTicketDTO filter)
        {
            filter.UserId = User.GetUserId();
            filter.OrderBy = FilterTicketOrder.CreateDate_DES;
            filter.FilterTicketState = FilterTicketState.NotDeleted;

            return View(await _contactService.FilterTickets(filter));
        }

        #endregion

        #region add ticket

        [HttpGet("add-ticket")]
        public IActionResult AddTicket()
        {
            return View();
        }

        [HttpPost("add-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketDTO ticket)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(ticket.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(ticket);
            }

            if (ModelState.IsValid)
            {
                var res = await _contactService.AddUserTicket(ticket, User.GetUserId());
                switch (res)
                {
                    case TicketResult.Error:
                        TempData[ErrorMessage] = "عملیات با شکست مواجه شده است";
                        break;
                    case TicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ارسال شد";
                        TempData[InfoMessage] = "جواب شما متعاقباًارسال خواهد شد";
                        return RedirectToAction("Index");
                }
            }

            return View();
        }

        #endregion

        #region ticket detail

        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> TicketDatail(long ticketId)
        {
            var ticket = await _contactService.GetTicketDetail(ticketId, User.GetUserId());
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        #endregion

        #region answer ticket

        [HttpPost("answer-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerTicket(AnswerTicketDTO answer)
        {
            if (string.IsNullOrEmpty(answer.Text))
            {
                TempData[WarningMessage] = "پیام خود را وارد کنید";
            }
            if (ModelState.IsValid)
            {
                var res = await _contactService.AddAnswerTicket(answer, User.GetUserId());
                switch (res)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "عدم دسترسی";
                        TempData[WarningMessage] = "در صورت تکرار دسترسی شما از سایت قطع می شود";
                        return RedirectToAction("Index", "Ticket");
                    case AnswerTicketResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                        return RedirectToAction("Index", "Ticket");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "اطلاعات شما با موفقبت ثبت شد";
                        break;
                }
            }

            return RedirectToAction("TicketDatail", "Ticket", new { area = "User", ticketId = answer.Id });
        }

        #endregion
    }
}
