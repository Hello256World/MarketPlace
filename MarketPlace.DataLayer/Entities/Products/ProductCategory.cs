﻿using MarketPlace.DataLayer.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.DataLayer.Entities.Products
{
    public class ProductCategory : BaseEntity
    {
        #region properties

        public long? ParentId { get; set; }
        
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }
        
        [Display(Name = "عنوان در URL")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UrlName { get; set; }
        
        [Display(Name = "فعال / غیر فعال")]
        public bool IsActive { get; set; }

        #endregion

        #region relations

        public ProductCategory Parent { get; set; }

        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }

        #endregion
    }
}
