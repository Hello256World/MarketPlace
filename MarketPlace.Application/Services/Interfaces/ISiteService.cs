using MarketPlace.DataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface ISiteService : IAsyncDisposable
    {
        #region site setting

        Task<SiteSetting> GetDeafultSiteSetting();

        #endregion

        #region slider

        public Task<List<Slider>> GetActiveSlider();

        #endregion

        #region site banner

        Task<List<SiteBanner>> GetSiteBanners(List<BannerPlacement> placement);

        #endregion
    }
}
