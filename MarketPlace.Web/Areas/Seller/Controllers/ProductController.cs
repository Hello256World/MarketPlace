using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.Web.PresentationExtension;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    public class ProductController : SellerBaseController
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly ISellerService _sellerService;

        public ProductController(IProductService productService, ISellerService sellerService)
        {
            _productService = productService;
            _sellerService = sellerService;
        }

        #endregion

        #region products list

        [HttpGet("products")]
        public async Task<IActionResult> Products(FilterProductDTO filter)
        {
            var seller = await _sellerService.GetSellerForProductFilter(User.GetUserId());
            filter.SellerId = seller.Id;

            return View(await _productService.FilterProduct(filter));
        }

        #endregion

        #region create product

        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.productCategory = await _productService.GetAllProductCategory(null);

            return View();
        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDTO product)
        {
            if (ModelState.IsValid)
            {
                // todo : create product
            }
            ViewBag.productCategory = await _productService.GetAllProductCategory(null);
            return View(product);
        }

        #endregion
    }
}
