using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Paging
{
    public class BasePaging
    {
        public BasePaging()
        {
            HowManyShowPageAfterAndBefore = 5;
            TakeEntity = 2;
            PageId = 1;
        }

        public int PageId { get; set; }

        public int PageCount { get; set; }

        public int AllEntitiesCount { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int TakeEntity { get; set; }

        public int SkipEntity { get; set; }

        public int HowManyShowPageAfterAndBefore { get; set; }

        public BasePaging GetBasePaging()
        {
            return this;
        }
    }
}
