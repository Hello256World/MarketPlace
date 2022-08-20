using System.ComponentModel.DataAnnotations;

namespace MarketPlace.DataLayer.DTOs.Contacts
{
    public class AnswerTicketDTO
    {
        public long Id { get; set; }

        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }
    }

    public enum AnswerTicketResult
    {
        NotForUser,
        NotFound,
        Success
    }
}
