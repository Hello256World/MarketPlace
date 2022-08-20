﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Paging
{
    public class Pager
    {
        public static BasePaging Builder(int pageId, int allEntitiesCount, int take, int howManyShowPageAfterAndBefore)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)take));

            return new BasePaging
            {
                AllEntitiesCount = allEntitiesCount,
                PageId = pageId,
                HowManyShowPageAfterAndBefore = howManyShowPageAfterAndBefore,
                TakeEntity = take,
                PageCount = pageCount,
                SkipEntity = (pageId - 1) * take,
                StartPage = pageId - howManyShowPageAfterAndBefore <= 0 ? 1 : pageId - howManyShowPageAfterAndBefore,
                EndPage = pageId + howManyShowPageAfterAndBefore > pageCount ? pageCount : pageId + howManyShowPageAfterAndBefore,
            };
        }
    }
}