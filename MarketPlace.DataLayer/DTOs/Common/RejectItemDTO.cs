using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Common
{
    public class RejectItemDTO
    {
        public long Id { get; set; }

        [Display(Name = "عدم تایید اطلاعات")]
        [Required(ErrorMessage = "متن عدم تایید را وارد کنید")]
        public string RejectMessage { get; set; }
    }
}
