using MarketPlace.DataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Entities.Site
{
    public class SiteSetting : BaseEntity
    {
        #region properties

        [Display(Name = "تلفن همراه")]
        public string? Mobile { get; set; }

        [Display(Name = "تلفن")]
        public string? Phone { get; set; }

        [Display(Name = "آدرس ایمیل")]
        public string? Email { get; set; }

        [Display(Name = "متن فوتر")]
        public string? FooterText { get; set; }

        [Display(Name = "متن کپی رایت")]
        public string? CopyRight { get; set; }

        [Display(Name = "آدرس نقشه")]
        public string? MapScript { get; set; }

        [Display(Name = "درباره ما")]
        public string? AboutUs { get; set; }

        [Display(Name = "آدرس")]
        public string? Address { get; set; }

        [Display(Name = "اصلی هست / نیست")]
        public bool IsDefault { get; set; }

        #endregion
    }
}
