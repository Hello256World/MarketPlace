using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Utils
{
    public static class PathExtensions
    {
        #region default images

        public static string DefaultAvatar = "/assets/img/defaults/avatar.jpg";

        #endregion

        #region uploader

        public static string UploadImage = "/assets/upload/image/";
        public static string UploadImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/upload/image/") ;

        #endregion

        #region product image

        public static string ProductImageOrigin = "/assets/content/images/Product/Origin/";
        public static string ProductImageOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/content/images/Product/Origin/");

        public static string ProductImageThumb = "/assets/content/images/Product/Thumb/";
        public static string ProductImageThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/content/images/Product/Thumb/");

        #endregion

        #region user avatar

        public static string UserAvatarOrigin = "/assets/content/images/UserAvatar/Origin/";
        public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/content/images/UserAvatar/Origin/");

        public static string UserAvatarThumb = "/assets/content/images/UserAvatar/Thumb/";
        public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/content/images/UserAvatar/Thumb/");

        #endregion

        #region home slider

        public static string SliderOrigin = "/assets/img/slider/";

        #endregion

        #region site banner

        public static string BannerOrigin = "/assets/img/bg/";

        #endregion
    }
}
