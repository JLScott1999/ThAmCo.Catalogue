namespace ThAmCo.Catalogue.RepositoryTests.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using EntityFrameworkCoreMock;
    using Microsoft.EntityFrameworkCore;
    using Moq;
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
                    Description = "TestDesc"
                },
                new ProductData()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Name = "TestTwo",
                    Description = "TestTwoDesc"
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
            ProductModel secondValue = result.ElementAt(1);
            Assert.Equal(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"), secondValue.Id);
            Assert.Equal("TestTwo", secondValue.Name);
            Assert.Equal("TestTwoDesc", secondValue.Description);
        }

        [Fact]
        public void Get__Single_ReturnValue()
        {
            var productList = new List<ProductData>()
            {
                new ProductData()
                {
                    Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Name = "Test",
                    Description = "TestDesc"
                },
                new ProductData()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Name = "TestTwo",
                    Description = "TestTwoDesc"
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
        }

    }
}
