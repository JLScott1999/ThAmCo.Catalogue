namespace ThAmCo.Catalogue.Services.Review
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class ReviewService : IReviewService
    {

        private readonly HttpClient httpClient;

        public ReviewService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductReviewModel>> GetProductReviewsAsync(Guid id)
        {
            try
            {
                var response = this.httpClient.GetAsync("product/" + id.ToString()).Result;
                response.EnsureSuccessStatusCode();

                if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    IEnumerable<ProductReviewModel> reviewList = await JsonSerializer.DeserializeAsync<IEnumerable<ProductReviewModel>>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            IgnoreNullValues = true,
                            PropertyNameCaseInsensitive = true
                        }
                    );
                    return reviewList.Where(x => !(x == null || x.ProductId.Equals(Guid.Empty) || string.IsNullOrEmpty(x.Name) || string.IsNullOrEmpty(x.Description) || x.Date.Equals(DateTime.MinValue)));
                }
            }
            catch (Exception)
            {
                return new List<ProductReviewModel>();
            }
            return new List<ProductReviewModel>();
        }

    }
}
