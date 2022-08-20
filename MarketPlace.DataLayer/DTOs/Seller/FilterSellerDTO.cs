using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Seller
{
    public class FilterSellerDTO : BasePaging
    {
        #region properties

        public long? UserId { get; set; }

        public string? StoreName { get; set; }

        public string? Phone { get; set; }

        public string? Mobile { get; set; }

        public string? Address { get; set; }

        public FilterSellerState State { get; set; }

        public List<MarketPlace.DataLayer.Entities.Store.Seller> Sellers { get; set; }

        #endregion

        #region methods

        public FilterSellerDTO SetSellers(List<MarketPlace.DataLayer.Entities.Store.Seller> sellers)
        {
            this.Sellers = sellers;
            return this;
        }

        public FilterSellerDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.PageCount = paging.PageCount;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.EndPage = paging.EndPage;
            this.StartPage = paging.StartPage;
            this.SkipEntity = paging.SkipEntity;
            this.TakeEntity = paging.TakeEntity;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            return this;
        }

        #endregion
    }

    public enum FilterSellerState
    {
        [Display(Name ="همه")]
        All,
        [Display(Name = "درحال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
