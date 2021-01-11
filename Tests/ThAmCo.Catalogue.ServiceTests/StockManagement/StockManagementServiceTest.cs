namespace ThAmCo.Catalogue.ServiceTests.StockManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using Moq.Protected;
    using ThAmCo.Catalogue.Models;
    using ThAmCo.Catalogue.Services.StockManagement;
    using Xunit;

    public class StockManagementServiceTest
    {

        [Fact]
        public async Task GetProductsStock_ReturnValue()
        {
            List<ProductStockModel> jsonObject = new List<ProductStockModel>()
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
            };
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                          "SendAsync",
                          ItExpr.IsAny<HttpRequestMessage>(),
                          ItExpr.IsAny<CancellationToken>()
                       )
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(JsonSerializer.Serialize(jsonObject), System.Text.Encoding.UTF8, "application/json")
                       })
                       .Verifiable();
            HttpClient httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://thamcostock.service/"),
            };
            StockManagementService service = new StockManagementService(httpClient);

            IEnumerable<ProductStockModel> result = await service.GetProductsStockAsync();

            Assert.Equal(2, result.Count());
            ProductStockModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.Id);
            Assert.Equal(10, firstValue.Stock);
            ProductStockModel secondValue = result.ElementAt(1);
            Assert.Equal(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"), secondValue.Id);
            Assert.Equal(0, secondValue.Stock);

            Uri expectedUri = new Uri("http://thamcostock.service/product/stock");
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.AtMostOnce(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  &&
                  req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetProductsStock_NotFound()
        {
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                          "SendAsync",
                          ItExpr.IsAny<HttpRequestMessage>(),
                          ItExpr.IsAny<CancellationToken>()
                       )
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.NotFound
                       })
                       .Verifiable();

            HttpClient httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://thamcostock.service/"),
            };
            StockManagementService service = new StockManagementService(httpClient);

            IEnumerable<ProductStockModel> result = await service.GetProductsStockAsync();

            Assert.Empty(result);

            Uri expectedUri = new Uri("http://thamcostock.service/product/stock");
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.AtMostOnce(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  &&
                  req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetProductsStock_Json_EmptyObject()
        {
            List<ProductStockModel> jsonObject = new List<ProductStockModel>()
            {
                new ProductStockModel(),
                new ProductStockModel()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Stock = 0
                }
            };

            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                          "SendAsync",
                          ItExpr.IsAny<HttpRequestMessage>(),
                          ItExpr.IsAny<CancellationToken>()
                       )
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(JsonSerializer.Serialize(jsonObject), System.Text.Encoding.UTF8, "application/json")
                       })
                       .Verifiable();
            HttpClient httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://thamcostock.service/")
            };
            StockManagementService service = new StockManagementService(httpClient);

            IEnumerable<ProductStockModel> result = await service.GetProductsStockAsync();

            Assert.Single(result);
            ProductStockModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"), firstValue.Id);
            Assert.Equal(0, firstValue.Stock);

            Uri expectedUri = new Uri("http://thamcostock.service/product/stock");
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.AtMostOnce(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  &&
                  req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetProductsStock_Json_InvalidObject()
        {
            List<object> jsonObject = new List<object>()
            {
                new {
                    productId = 1,
                    D = 2020,
                    Name = "Test"
                },
                new ProductStockModel()
                {
                    Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                    Stock = 0
                }
            };

            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                          "SendAsync",
                          ItExpr.IsAny<HttpRequestMessage>(),
                          ItExpr.IsAny<CancellationToken>()
                       )
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(JsonSerializer.Serialize(jsonObject), System.Text.Encoding.UTF8, "application/json")
                       })
                       .Verifiable();
            HttpClient httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://thamcostock.service/")
            };
            StockManagementService service = new StockManagementService(httpClient);

            IEnumerable<ProductStockModel> result = await service.GetProductsStockAsync();

            Assert.Single(result);
            ProductStockModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"), firstValue.Id);
            Assert.Equal(0, firstValue.Stock);

            Uri expectedUri = new Uri("http://thamcostock.service/product/stock");
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.AtMostOnce(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  &&
                  req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetProductsStock_Json_Invalid()
        {
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                          "SendAsync",
                          ItExpr.IsAny<HttpRequestMessage>(),
                          ItExpr.IsAny<CancellationToken>()
                       )
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent("Not json", System.Text.Encoding.UTF8, "application/json")
                       })
                       .Verifiable();
            HttpClient httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://thamcostock.service/"),
            };
            StockManagementService service = new StockManagementService(httpClient);

            IEnumerable<ProductStockModel> result = await service.GetProductsStockAsync();

            Assert.Empty(result);

            Uri expectedUri = new Uri("http://thamcostock.service/product/stock");
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.AtMostOnce(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  &&
                  req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }

    }
}
