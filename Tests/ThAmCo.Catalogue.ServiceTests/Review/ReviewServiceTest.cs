namespace ThAmCo.Catalogue.ServiceTests.Review
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using Moq.Protected;
    using ThAmCo.Catalogue.Models;
    using ThAmCo.Catalogue.Services.Review;
    using Xunit;


    public class ReviewServiceTest
    {

        [Fact]
        public async Task GetProducts_ReturnValue()
        {
            DateTime currentDateTime = DateTime.UtcNow;
            List<ProductReviewModel> jsonObject = new List<ProductReviewModel>()
            {
                new ProductReviewModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Date = currentDateTime,
                    Description = "Test Review of product",
                    Name = "Test"
                },
                new ProductReviewModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Date = currentDateTime.AddDays(-7),
                    Description = "Test Review 2 of product",
                    Name = "Test 2"
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
                BaseAddress = new Uri("http://thamcoreview.service/"),
            };
            ReviewService service = new ReviewService(httpClient);

            IEnumerable<ProductReviewModel> result = await service.GetProductReviewsAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Equal(2, result.Count());
            ProductReviewModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.ProductId);
            Assert.Equal(currentDateTime, firstValue.Date);
            Assert.Equal("Test Review of product", firstValue.Description);
            Assert.Equal("Test", firstValue.Name);
            ProductReviewModel secondValue = result.ElementAt(1);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), secondValue.ProductId);
            Assert.Equal(currentDateTime.AddDays(-7), secondValue.Date);
            Assert.Equal("Test Review 2 of product", secondValue.Description);
            Assert.Equal("Test 2", secondValue.Name);

            Uri expectedUri = new Uri("http://thamcoreview.service/product/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
        public async Task GetProducts_NotFound()
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
                BaseAddress = new Uri("http://thamcoreview.service/"),
            };

            ReviewService service = new ReviewService(httpClient);
            IEnumerable<ProductReviewModel> result = await service.GetProductReviewsAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Empty(result);

            Uri expectedUri = new Uri("http://thamcoreview.service/product/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
        public async Task GetProducts_Json_EmptyObject()
        {
            DateTime currentDateTime = DateTime.UtcNow;
            List<object> jsonObject = new List<object>()
            {
                new ProductReviewModel() {
                },
                new ProductReviewModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Date = currentDateTime.AddDays(-7),
                    Description = "Test Review 2 of product",
                    Name = "Test 2"
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
                BaseAddress = new Uri("http://thamcoreview.service/"),
            };
            ReviewService service = new ReviewService(httpClient);

            IEnumerable<ProductReviewModel> result = await service.GetProductReviewsAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Single(result);
            ProductReviewModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.ProductId);
            Assert.Equal(currentDateTime.AddDays(-7), firstValue.Date);
            Assert.Equal("Test Review 2 of product", firstValue.Description);
            Assert.Equal("Test 2", firstValue.Name);

            Uri expectedUri = new Uri("http://thamcoreview.service/product/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
        public async Task GetProducts_Json_InvalidObject()
        {
            DateTime currentDateTime = DateTime.UtcNow;
            List<object> jsonObject = new List<object>()
            {
                new {
                    Id = 1,
                    D = 2020,
                    Name = "Test"
                },
                new ProductReviewModel()
                {
                    ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                    Date = currentDateTime.AddDays(-7),
                    Description = "Test Review 2 of product",
                    Name = "Test 2"
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
                BaseAddress = new Uri("http://thamcoreview.service/"),
            };
            ReviewService service = new ReviewService(httpClient);

            IEnumerable<ProductReviewModel> result = await service.GetProductReviewsAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Single(result);
            ProductReviewModel firstValue = result.ElementAt(0);
            Assert.Equal(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"), firstValue.ProductId);
            Assert.Equal(currentDateTime.AddDays(-7), firstValue.Date);
            Assert.Equal("Test Review 2 of product", firstValue.Description);
            Assert.Equal("Test 2", firstValue.Name);

            Uri expectedUri = new Uri("http://thamcoreview.service/product/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
        public async Task GetProducts_Json_Invalid()
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
                BaseAddress = new Uri("http://thamcoreview.service/"),
            };
            ReviewService service = new ReviewService(httpClient);

            IEnumerable<ProductReviewModel> result = await service.GetProductReviewsAsync(Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"));

            Assert.Empty(result);

            Uri expectedUri = new Uri("http://thamcoreview.service/product/14D486C4-CEE6-4C26-B274-CC0E300B0B99");
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
