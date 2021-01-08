namespace ThAmCo.Catalogue.Controllers
{
    using System;
    using System.Collections.Generic;
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
        public IActionResult Products()
        {
            IEnumerable<ProductModel> productsList = this.productService.GetProducts();
            try
            {
                return this.View(productsList
                .Join(this.stockService.GetProductsStock(),
                    pm => pm.Id,
                    psm => psm.Id,
                    (pm, psm) => new ProductViewModel()
                        {
                            Id = pm.Id,
                            Name = pm.Name,
                            Description = pm.Description,
                            Stock = psm.Stock
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
                catch (Exception)
                {
                    // Review Service failure
                }

                try
                {
                    productStock = this.stockService.GetProductStock(id);
                }
                catch (Exception)
                {
                    // Stock Service failure
                }

                try
                {
                    orderModel = this.orderService.HasOrdered(id);
                }
                catch (Exception)
                {
                    // Order Service failure
                }

                return this.View(
                    new ProductViewModel()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Stock = productStock?.Stock,
                        Reviews = productReviews,
                        LastOrdered = orderModel?.FirstOrDefault()?.DateTime
                    }
                );
            }
            else
            {
                return this.RedirectToAction("Products");
            }
        }

        [Route("Search")]
        public IActionResult Search([FromQuery(Name = "q")] string query)
        {
            IEnumerable<ProductModel> productsList = this.productService.SearchProducts(query);
            try
            {
                return this.View("Products", productsList
                    .Join(this.stockService.GetProductsStock(),
                    pm => pm.Id,
                    psm => psm.Id,
                    (pm, psm) => new ProductViewModel()
                        {
                            Id = pm.Id,
                            Name = pm.Name,
                            Description = pm.Description,
                            Stock = psm.Stock
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
                        Stock = null
                    })
               );
            }
        }

    }
}
