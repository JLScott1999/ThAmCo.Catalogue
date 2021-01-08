namespace ThAmCo.Catalogue.Services.Review
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
                    Stream contentStream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<IEnumerable<ProductReviewModel>>(contentStream, new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
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
