namespace ThAmCo.Catalogue.ServiceTests.Order
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
    using ThAmCo.Catalogue.Services.Order;
    using Xunit;

    public class OrderServiceTest
    {

        [Fact]
        public async Task HasOrdered_ReturnValue()
        {
            DateTime currentDateTime = DateTime.UtcNow;
            List<ProductOrderModel> jsonObject = new List<ProductOrderModel>()
            {
                new ProductOrderModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    OrderDate = currentDateTime
                },
                new ProductOrderModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    OrderDate = currentDateTime.AddDays(-7),
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
                BaseAddress = new Uri("http://thamcoorder.service/"),
            };
            OrderService service = new OrderService(httpClient);

            IEnumerable<ProductOrderModel> result = await service.HasOrderedAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Equal(2, result.Count());
            ProductOrderModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.ProductId);
            Assert.Equal(currentDateTime, firstValue.OrderDate);
            ProductOrderModel secondValue = result.ElementAt(1);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), secondValue.ProductId);
            Assert.Equal(currentDateTime.AddDays(-7), secondValue.OrderDate);
            Assert.True(firstValue.OrderDate > secondValue.OrderDate);

            Uri expectedUri = new Uri("http://thamcoorder.service/product/ordered/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
        public async Task HasOrdered_NotFound()
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
                BaseAddress = new Uri("http://thamcoorder.service/"),
            };
            OrderService service = new OrderService(httpClient);

            IEnumerable<ProductOrderModel> result = await service.HasOrderedAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Empty(result);
            Uri expectedUri = new Uri("http://thamcoorder.service/product/ordered/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
        public async Task HasOrdered_Json_EmptyObject()
        {
            DateTime currentDateTime = DateTime.UtcNow;
            List<ProductOrderModel> jsonObject = new List<ProductOrderModel>()
            {
                new ProductOrderModel() {
                },
                new ProductOrderModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    OrderDate = currentDateTime.AddDays(-7),
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
                BaseAddress = new Uri("http://thamcoorder.service/"),
            };
            OrderService service = new OrderService(httpClient);

            IEnumerable<ProductOrderModel> result = await service.HasOrderedAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Single(result);
            ProductOrderModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.ProductId);
            Assert.Equal(currentDateTime.AddDays(-7), firstValue.OrderDate);

            Uri expectedUri = new Uri("http://thamcoorder.service/product/ordered/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
        public async Task HasOrdered_Json_InvalidObject()
        {
            DateTime currentDateTime = DateTime.UtcNow;
            List<object> jsonObject = new List<object>()
            {
                new {
                    Id = 1,
                    D = 2020,
                    Name = "Test"
                },
                new ProductOrderModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    OrderDate = currentDateTime.AddDays(-7),
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
                BaseAddress = new Uri("http://thamcoorder.service/"),
            };
            OrderService service = new OrderService(httpClient);

            IEnumerable<ProductOrderModel> result = await service.HasOrderedAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Single(result);
            ProductOrderModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.ProductId);
            Assert.Equal(currentDateTime.AddDays(-7), firstValue.OrderDate);

            Uri expectedUri = new Uri("http://thamcoorder.service/product/ordered/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
        public async Task HasOrdered_Json_Invalid()
        {
            DateTime currentDateTime = DateTime.UtcNow;

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
                BaseAddress = new Uri("http://thamcoorder.service/"),
            };
            OrderService service = new OrderService(httpClient);

            IEnumerable<ProductOrderModel> result = await service.HasOrderedAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Empty(result);

            Uri expectedUri = new Uri("http://thamcoorder.service/product/ordered/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
