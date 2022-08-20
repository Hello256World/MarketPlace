using MarketPlace.Application.Utils;
using MarketPlace.DataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.EntityExtensions
{
    public static class BannerExtension
    {
        public static string GetBannerImage(this SiteBanner siteBanner)
        {
            return PathExtensions.BannerOrigin + siteBanner.ImageName;
        }
    }
}
