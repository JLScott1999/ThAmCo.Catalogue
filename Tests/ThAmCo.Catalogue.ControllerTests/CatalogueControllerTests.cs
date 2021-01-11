namespace ThAmCo.Catalogue.ControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using ThAmCo.Catalogue.Controllers;
    using ThAmCo.Catalogue.Models;
    using ThAmCo.Catalogue.Services.Order;
    using ThAmCo.Catalogue.Services.Product;
    using ThAmCo.Catalogue.Services.Review;
    using ThAmCo.Catalogue.Services.StockManagement;
    using ThAmCo.Catalogue.ViewModels;
    using Xunit;

    public class CatalogueControllerTests
    {

        [Fact]
        public async Task Products_ReturnValue()
        {
            IProductService productService = new FakeProductService(new List<ProductModel>()
            {
                new ProductModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc",
                    EAN = "TestEAN",
                    BrandName = "TestBrand",
                    Price = 10.99
                },
                new ProductModel()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Name = "TestTwo",
                    Description = "TestTwoDesc",
                    EAN = "TestTwoEAN",
                    BrandName = "TestTwoBrand",
                    Price = 5.45
                }
            });
            IStockManagementService stockService = new FakeStockManagementService(new List<ProductStockModel>()
            {
                new ProductStockModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Stock = 10
                },
                new ProductStockModel()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Stock = 0
                }
            });
            CatalogueController controller = new CatalogueController(productService, stockService, null, null);
            IActionResult result = await controller.Products();

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            IEnumerable<ProductViewModel> model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());

            ProductViewModel firstValue = model.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.Id);
            Assert.Equal("Test", firstValue.Name);
            Assert.Equal("TestDesc", firstValue.Description);
            Assert.Equal(10, firstValue.Stock);
            Assert.Equal("In Stock", firstValue.StockStatus);
            Assert.Equal("TestEAN", firstValue.EAN);
            Assert.Equal("TestBrand", firstValue.BrandName);
            Assert.Equal(10.99, firstValue.Price);

            ProductViewModel secondValue = model.ElementAt(1);
            Assert.Equal(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"), secondValue.Id);
            Assert.Equal("TestTwo", secondValue.Name);
            Assert.Equal("TestTwoDesc", secondValue.Description);
            Assert.Equal(0, secondValue.Stock);
            Assert.Equal("Out of Stock", secondValue.StockStatus);
            Assert.Equal("TestTwoEAN", secondValue.EAN);
            Assert.Equal("TestTwoBrand", secondValue.BrandName);
            Assert.Equal(5.45, secondValue.Price);
        }

        [Fact]
        public async Task Products_ProductsService_Exception()
        {
            Mock<IProductService> productService = new Mock<IProductService>();
            productService.Setup(x => x.GetProducts()).Throws(new Exception());
            IStockManagementService stockService = new FakeStockManagementService(new List<ProductStockModel>()
            {
                new ProductStockModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Stock = 10
                },
                new ProductStockModel()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Stock = 0
                }
            });
            CatalogueController controller = new CatalogueController(productService.Object, stockService, null, null);
            Exception viewResult = await Assert.ThrowsAsync<Exception>(async () => await controller.Products());
        }

        [Fact]
        public async Task Products_StockManagementService_Exception()
        {
            IProductService productService = new FakeProductService(new List<ProductModel>()
            {
                new ProductModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc",
                    EAN = "TestEAN",
                    BrandName = "TestBrand",
                    Price = 10.99
                },
                new ProductModel()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Name = "TestTwo",
                    Description = "TestTwoDesc",
                    EAN = "TestTwoEAN",
                    BrandName = "TestTwoBrand",
                    Price = 5.45
                }
            });
            Mock<IStockManagementService> stockService = new Mock<IStockManagementService>();
            stockService.Setup(x => x.GetProductsStockAsync()).Throws(new Exception());
            CatalogueController controller = new CatalogueController(productService, stockService.Object, null, null);
            IActionResult result = await controller.Products();
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            IEnumerable<ProductViewModel> model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());

            ProductViewModel firstValue = model.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.Id);
            Assert.Equal("Test", firstValue.Name);
            Assert.Equal("TestDesc", firstValue.Description);
            Assert.Null(firstValue.Stock);
            Assert.Equal("Unknown", firstValue.StockStatus);
            Assert.Equal("TestEAN", firstValue.EAN);
            Assert.Equal("TestBrand", firstValue.BrandName);
            Assert.Equal(10.99, firstValue.Price);

            ProductViewModel secondValue = model.ElementAt(1);
            Assert.Equal(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"), secondValue.Id);
            Assert.Equal("TestTwo", secondValue.Name);
            Assert.Equal("TestTwoDesc", secondValue.Description);
            Assert.Null(secondValue.Stock);
            Assert.Equal("Unknown", secondValue.StockStatus);
            Assert.Equal("TestTwoEAN", secondValue.EAN);
            Assert.Equal("TestTwoBrand", secondValue.BrandName);
            Assert.Equal(5.45, secondValue.Price);
        }

        [Fact]
        public async Task Product_ReturnValue()
        {
            IProductService productService = new FakeProductService(new List<ProductModel>()
            {
                new ProductModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc",
                    EAN = "TestEAN",
                    BrandName = "TestBrand",
                    Price = 10.99
                }
            });
            IStockManagementService stockService = new FakeStockManagementService(new List<ProductStockModel>()
            {
                new ProductStockModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Stock = 10
                }
            });
            IReviewService reviewService = new FakeReviewService(new List<ProductReviewModel>()
            {
                new ProductReviewModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Date = DateTime.UtcNow,
                    Name = "Test",
                    Description = "Test Review"
                }
            });
            IOrderService orderService = new FakeOrderService(new List<ProductOrderModel>()
            {
                new ProductOrderModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    OrderDate = DateTime.Parse("2020-12-12 12:12")
                }
            });
            CatalogueController controller = new CatalogueController(productService, stockService, reviewService, orderService);
            IActionResult result = await controller.Product(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            ProductViewModel model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.ViewData.Model);
            Assert.Equal("Test", model.Name);
            Assert.Equal("TestDesc", model.Description);
            Assert.Equal(10, model.Stock);
            Assert.Equal("In Stock", model.StockStatus);
            Assert.Equal("TestEAN", model.EAN);
            Assert.Equal("TestBrand", model.BrandName);
            Assert.Equal(10.99, model.Price);
            Assert.Single(model.Reviews);
            Assert.Equal(DateTime.Parse("2020-12-12 12:12"), model.LastOrdered);
        }

        [Fact]
        public async Task Product_NotFound()
        {
            IProductService productService = new FakeProductService(new List<ProductModel>()
            {
                new ProductModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc",
                    EAN = "TestEAN",
                    BrandName = "TestBrand",
                    Price = 10.99
                }
            });
            IStockManagementService stockService = new FakeStockManagementService(new List<ProductStockModel>()
            {
                new ProductStockModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Stock = 10
                }
            });
            IReviewService reviewService = new FakeReviewService(new List<ProductReviewModel>()
            {
                new ProductReviewModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Date = DateTime.UtcNow,
                    Name = "Test",
                    Description = "Test Review"
                }
            });
            IOrderService orderService = new FakeOrderService(new List<ProductOrderModel>()
            {
                new ProductOrderModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    OrderDate = DateTime.Parse("2020-12-12 12:12")
                }
            });
            CatalogueController controller = new CatalogueController(productService, stockService, reviewService, orderService);
            IActionResult result = await controller.Product(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"));

            RedirectToActionResult viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", viewResult.ActionName);
        }

        [Fact]
        public async Task Product_ProductsService_Exception()
        {
            Mock<IProductService> productService = new Mock<IProductService>();
            productService.Setup(x => x.GetProduct(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"))).Throws(new Exception());
            IStockManagementService stockService = new FakeStockManagementService(new List<ProductStockModel>()
            {
                new ProductStockModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Stock = 10
                }
            });
            IReviewService reviewService = new FakeReviewService(new List<ProductReviewModel>()
            {
                new ProductReviewModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Date = DateTime.UtcNow,
                    Name = "Test",
                    Description = "Test Review"
                }
            });
            IOrderService orderService = new FakeOrderService(new List<ProductOrderModel>()
            {
                new ProductOrderModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    OrderDate = DateTime.Parse("2020-12-12 12:12")
                }
            });
            CatalogueController controller = new CatalogueController(productService.Object, stockService, reviewService, orderService);
            Exception viewResult = await Assert.ThrowsAsync<Exception>(async () => await controller.Product(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99")));
        }

        [Fact]
        public async Task Product_StockManagementService_Exception()
        {
            IProductService productService = new FakeProductService(new List<ProductModel>()
            {
                new ProductModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc",
                    EAN = "TestEAN",
                    BrandName = "TestBrand",
                    Price = 10.99
                }
            });
            Mock<IStockManagementService> stockService = new Mock<IStockManagementService>();
            stockService.Setup(x => x.GetProductStockAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"))).Throws(new Exception());
            IReviewService reviewService = new FakeReviewService(new List<ProductReviewModel>()
            {
                new ProductReviewModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Date = DateTime.UtcNow,
                    Name = "Test",
                    Description = "Test Review"
                }
            });
            IOrderService orderService = new FakeOrderService(new List<ProductOrderModel>()
            {
                new ProductOrderModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    OrderDate = DateTime.Parse("2020-12-12 12:12")
                }
            });
            CatalogueController controller = new CatalogueController(productService, stockService.Object, reviewService, orderService);
            IActionResult result = await controller.Product(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            ProductViewModel model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.ViewData.Model);
            Assert.Equal("Test", model.Name);
            Assert.Equal("TestDesc", model.Description);
            Assert.Null(model.Stock);
            Assert.Equal("Unknown", model.StockStatus);
            Assert.Equal("TestEAN", model.EAN);
            Assert.Equal("TestBrand", model.BrandName);
            Assert.Equal(10.99, model.Price);
            Assert.Single(model.Reviews);
            Assert.Equal(DateTime.Parse("2020-12-12 12:12"), model.LastOrdered);
        }

        [Fact]
        public async Task Product_ReviewService_Exception()
        {
            IProductService productService = new FakeProductService(new List<ProductModel>()
            {
                new ProductModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc",
                    EAN = "TestEAN",
                    BrandName = "TestBrand",
                    Price = 10.99
                }
            });
            IStockManagementService stockService = new FakeStockManagementService(new List<ProductStockModel>()
            {
                new ProductStockModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Stock = 10
                }
            });
            Mock<IReviewService> reviewService = new Mock<IReviewService>();
            reviewService.Setup(x => x.GetProductReviewsAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"))).Throws(new Exception());
            IOrderService orderService = new FakeOrderService(new List<ProductOrderModel>()
            {
                new ProductOrderModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    OrderDate = DateTime.Parse("2020-12-12 12:12")
                }
            });
            CatalogueController controller = new CatalogueController(productService, stockService, reviewService.Object, orderService);
            IActionResult result = await controller.Product(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            ProductViewModel model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.ViewData.Model);
            Assert.Equal("Test", model.Name);
            Assert.Equal("TestDesc", model.Description);
            Assert.Equal(10, model.Stock);
            Assert.Equal("In Stock", model.StockStatus);
            Assert.Equal("TestEAN", model.EAN);
            Assert.Equal("TestBrand", model.BrandName);
            Assert.Equal(10.99, model.Price);
            Assert.Null(model.Reviews);
            Assert.Equal(DateTime.Parse("2020-12-12 12:12"), model.LastOrdered);
        }

        [Fact]
        public async Task Product_OrderService_Exception()
        {
            IProductService productService = new FakeProductService(new List<ProductModel>()
            {
                new ProductModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc",
                    EAN = "TestEAN",
                    BrandName = "TestBrand",
                    Price = 10.99
                }
            });
            IStockManagementService stockService = new FakeStockManagementService(new List<ProductStockModel>()
            {
                new ProductStockModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Stock = 10
                }
            });
            IReviewService reviewService = new FakeReviewService(new List<ProductReviewModel>()
            {
                new ProductReviewModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Date = DateTime.UtcNow,
                    Name = "Test",
                    Description = "Test Review"
                }
            });
            Mock<IOrderService> orderService = new Mock<IOrderService>();
            orderService.Setup(x => x.HasOrderedAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"))).Throws(new Exception());
            CatalogueController controller = new CatalogueController(productService, stockService, reviewService, orderService.Object);
            IActionResult result = await controller.Product(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            ProductViewModel model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.ViewData.Model);
            Assert.Equal("Test", model.Name);
            Assert.Equal("TestDesc", model.Description);
            Assert.Equal(10, model.Stock);
            Assert.Equal("In Stock", model.StockStatus);
            Assert.Equal("TestEAN", model.EAN);
            Assert.Equal("TestBrand", model.BrandName);
            Assert.Equal(10.99, model.Price);
            Assert.Single(model.Reviews);
            Assert.Null(model.LastOrdered);
        }

    }
}
