namespace ThAmCo.Catalogue.ServiceTests.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ThAmCo.Catalogue.Models;
    using ThAmCo.Catalogue.Repositories.Product;
    using ThAmCo.Catalogue.Services.Product;
    using Xunit;

    public class ProductServiceTest
    {

        [Fact]
        public void GetProductsTest()
        {
            IProductRepository productRepository = new FakeProductRepository(new List<ProductModel>()
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
            ProductService service = new ProductService(productRepository);
            var result = service.GetProducts();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetProductTest()
        {
            IProductRepository productRepository = new FakeProductRepository(new List<ProductModel>()
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
            ProductService service = new ProductService(productRepository);
            var result = service.GetProduct(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), result.Id);
            Assert.Equal("Test", result.Name);
            Assert.Equal("TestDesc", result.Description);
        }

        [Fact]
        public void SearchProductTest()
        {
            IProductRepository productRepository = new FakeProductRepository(new List<ProductModel>()
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
            ProductService service = new ProductService(productRepository);
            var result = service.SearchProducts("Test");
            Assert.Equal(2, result.Count());
        }

    }
}
