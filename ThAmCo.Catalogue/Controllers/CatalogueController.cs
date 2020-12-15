namespace ThAmCo.Catalogue.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public IActionResult Products()
        {
            return this.View(this.productService.GetProducts()
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

        [Route("Product/{id}")]
        public IActionResult Product(Guid id)
        {
            ProductModel product = this.productService.GetProduct(id);
            if (product != null)
            {
                IEnumerable<ProductReviewModel> productReviews = this.reviewService.GetProductReviews(id);
                ProductStockModel productStock = this.stockService.GetProductStock(id);
                IEnumerable<ProductOrderModel> orderModel = this.orderService.HasOrdered(id);
                return this.View(
                    new ProductViewModel()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Stock = productStock.Stock,
                        Reviews = productReviews,
                        LastOrdered = orderModel.FirstOrDefault()?.DateTime
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
            return this.View("Products", this.productService.SearchProducts(query)
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

    }
}
