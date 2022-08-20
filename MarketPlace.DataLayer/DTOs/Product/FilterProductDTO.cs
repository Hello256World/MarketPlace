using MarketPlace.DataLayer.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Product
{
    public class FilterProductDTO : BasePaging
    {
        #region properties

        public long? SellerId { get; set; }

        public string ProductTitle { get; set; }

        public FilterProductState FilterProductState { get; set; }

        public List<MarketPlace.DataLayer.Entities.Products.Product> Products { get; set; }

        #endregion

        #region method

        public FilterProductDTO SetProducts(List<MarketPlace.DataLayer.Entities.Products.Product> products)
        {
            this.Products = products;

            return this;
        }

        public FilterProductDTO SetPaging(BasePaging paging)
        {
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.PageCount = paging.PageCount;
            this.PageId = paging.PageId;
            this.SkipEntity = paging.SkipEntity;
            this.TakeEntity = paging.TakeEntity;

            return this;
        }

        #endregion
    }

    public enum FilterProductState
    {
        All,
        UnderProgress,
        Accepted,
        Rejected,
        Active,
        NotActive
    }
}
