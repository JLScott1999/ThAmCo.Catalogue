namespace ThAmCo.Catalogue.RepositoryTests.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EntityFrameworkCoreMock;
    using Microsoft.EntityFrameworkCore;
    using ThAmCo.Catalogue.Data;
    using ThAmCo.Catalogue.Models;
    using ThAmCo.Catalogue.Repositories.Product;
    using Xunit;

    public class ProductRepositoryTest
    {

        [Fact]
        public void Get_All_ReturnValue()
        {
            var productList = new List<ProductData>()
            {
                new ProductData()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc",
                    EAN = "TestEAN",
                    BrandName = "TestBrand",
                    Price = 10.99
                },
                new ProductData()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Name = "TestTwo",
                    Description = "TestTwoDesc",
                    EAN = "TestTwoEAN",
                    BrandName = "TestTwoBrand",
                    Price = 5.45
                }
            };

            var dbContextOptions = new DbContextOptions<DataContext>();
            var dbContextMock = new DbContextMock<DataContext>(dbContextOptions);
            var productDbSetMock = dbContextMock.CreateDbSetMock(x => x.ProductData, productList);

            var repository = new ProductRepository(dbContextMock.Object);
            var result = repository.Get();

            Assert.Equal(2, result.Count());
            ProductModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.Id);
            Assert.Equal("Test", firstValue.Name);
            Assert.Equal("TestDesc", firstValue.Description);
            Assert.Equal("TestEAN", firstValue.EAN);
            Assert.Equal("TestBrand", firstValue.BrandName);
            Assert.Equal(10.99, firstValue.Price);

            ProductModel secondValue = result.ElementAt(1);
            Assert.Equal(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"), secondValue.Id);
            Assert.Equal("TestTwo", secondValue.Name);
            Assert.Equal("TestTwoDesc", secondValue.Description);
            Assert.Equal("TestTwoEAN", secondValue.EAN);
            Assert.Equal("TestTwoBrand", secondValue.BrandName);
            Assert.Equal(5.45, secondValue.Price);

        }

        [Fact]
        public void Get_Single_ReturnValue()
        {
            var productList = new List<ProductData>()
            {
                new ProductData()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc",
                    EAN = "TestEAN",
                    BrandName = "TestBrand",
                    Price = 10.99

                },
                new ProductData()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Name = "TestTwo",
                    Description = "TestTwoDesc",
                    EAN = "TestTwoEAN",
                    BrandName = "TestTwoBrand",
                    Price = 5.45

                }
            };

            var dbContextOptions = new DbContextOptions<DataContext>();
            var dbContextMock = new DbContextMock<DataContext>(dbContextOptions);
            var productDbSetMock = dbContextMock.CreateDbSetMock(x => x.ProductData, productList);
            var repository = new ProductRepository(dbContextMock.Object);

            var result = repository.Get(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"));

            Assert.Equal(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"), result.Id);
            Assert.Equal("TestTwo", result.Name);
            Assert.Equal("TestTwoDesc", result.Description);
            Assert.Equal("TestTwoEAN", result.EAN);
            Assert.Equal("TestTwoBrand", result.BrandName);
            Assert.Equal(5.45, result.Price);
        }

    }
}
