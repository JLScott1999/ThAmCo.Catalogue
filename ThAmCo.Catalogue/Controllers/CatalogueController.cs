namespace ThAmCo.Catalogue.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ThAmCo.Catalogue.Models;
    using ThAmCo.Catalogue.Services.Order;
    using ThAmCo.Catalogue.Services.Product;
    using ThAmCo.Catalogue.Services.Review;
    using ThAmCo.Catalogue.Services.StockManagement;
    using ThAmCo.Catalogue.ViewModels;

    public class CatalogueController : Controller
    {

        private readonly IProductService productService;
        private readonly IStockManagementService stockService;
        private readonly IReviewService reviewService;
        private readonly IOrderService orderService;

        public CatalogueController(
            IProductService productService,
            IStockManagementService stockManagementService,
            IReviewService reviewService,
            IOrderService orderService
        )
        {
            this.productService = productService;
            this.stockService = stockManagementService;
            this.reviewService = reviewService;
            this.orderService = orderService;
        }

        [Route("Products")]
        [Route("/")]
        public async Task<IActionResult> Products()
        {
            IEnumerable<ProductModel> productsList = this.productService.GetProducts();
            try
            {
                return this.View(productsList
                .GroupJoin(await this.stockService.GetProductsStockAsync(),
                    pm => pm.Id,
                    psm => psm.Id,
                    (pm, psm) => new ProductViewModel()
                        {
                            Id = pm.Id,
                            Name = pm.Name,
                            Description = pm.Description,
                            Price = pm.Price,
                            EAN = pm.EAN,
                            BrandName = pm.BrandName,
                            Stock = psm?.FirstOrDefault()?.Stock
                        }
                    )
                );
            }
            catch (Exception)
            {
                return this.View(productsList
                    .Select((pm, psm) => new ProductViewModel()
                    {
                        Id = pm.Id,
                        Name = pm.Name,
                        Description = pm.Description,
                        Price = pm.Price,
                        EAN = pm.EAN,
                        BrandName = pm.BrandName,
                        Stock = null
                    })
               );
            }
        }

        [Route("Product/{id}")]
        public async Task<IActionResult> Product(Guid id)
        {
            ProductModel product = this.productService.GetProduct(id);
            IEnumerable<ProductReviewModel> productReviews = null;
            ProductStockModel productStock = null;
            IEnumerable<ProductOrderModel> orderModel = null;
            if (product != null)
            {
                try
                {
                    productReviews = await this.reviewService.GetProductReviewsAsync(id);
                }
                catch (Exception e)
                {
                    // Review Service failure
                    Debug.WriteLine(e);
                }

                try
                {
                    productStock = await this.stockService.GetProductStockAsync(id);
                }
                catch (Exception e)
                {
                    // Stock Service failure
                    Debug.WriteLine(e);
                }

                try
                {
                    orderModel = await this.orderService.HasOrderedAsync(id);
                }
                catch (Exception e)
                {
                    // Order Service failure
                    Debug.WriteLine(e);
                }

                return this.View(
                    new ProductViewModel()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        EAN = product.EAN,
                        BrandName = product.BrandName,
                        Stock = productStock?.Stock,
                        Reviews = productReviews,
                        LastOrdered = orderModel?.FirstOrDefault()?.OrderDate
                    }
                );
            }
            else
            {
                return this.RedirectToAction("Products");
            }
        }

        [Route("Search")]
        public async Task<IActionResult> Search([FromQuery(Name = "q")] string query)
        {
            IEnumerable<ProductModel> productsList = this.productService.SearchProducts(query);
            try
            {
                return this.View("Products", productsList
                    .GroupJoin(await this.stockService.GetProductsStockAsync(),
                    pm => pm.Id,
                    psm => psm.Id,
                    (pm, psm) => new ProductViewModel()
                        {
                            Id = pm.Id,
                            Name = pm.Name,
                            Description = pm.Description,
                            Price = pm.Price,
                            EAN = pm.EAN,
                            BrandName = pm.BrandName,
                            Stock = psm?.FirstOrDefault()?.Stock
                        }
                    )
                );
            }
            catch (Exception)
            {
                return this.View("Products", productsList
                    .Select((pm, psm) => new ProductViewModel()
                    {
                        Id = pm.Id,
                        Name = pm.Name,
                        Description = pm.Description,
                        Price = pm.Price,
                        EAN = pm.EAN,
                        BrandName = pm.BrandName,
                        Stock = null
                    })
               );
            }
        }

    }
}
