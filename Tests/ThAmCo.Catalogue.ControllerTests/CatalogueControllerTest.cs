namespace ThAmCo.Catalogue.ControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using ThAmCo.Catalogue.Controllers;
    using ThAmCo.Catalogue.Models;
    using ThAmCo.Catalogue.Services.Product;
    using ThAmCo.Catalogue.Services.StockManagement;
    using ThAmCo.Catalogue.ViewModels;
    using Xunit;

    public class CatalogueControllerTest
    {

        [Fact]
        public void ProductsReturnValueTest()
        {
            IProductService productService = new FakeProductService(new List<ProductModel>()
            {
                new ProductModel()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc"
                },
                new ProductModel()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Name = "TestTwo",
                    Description = "TestTwoDesc"
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
            CatalogueController controller = new CatalogueController(productService, stockService);
            IActionResult result = controller.Products();

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            IEnumerable<ProductViewModel> model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

    }
}
