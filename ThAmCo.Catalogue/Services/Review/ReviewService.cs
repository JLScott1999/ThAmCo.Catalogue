namespace ThAmCo.Catalogue.Services.Review
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using ThAmCo.Catalogue.Models;

    public class ReviewService : IReviewService
    {

        private readonly HttpClient httpClient;

        public ReviewService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public IEnumerable<ProductReviewModel> GetProductReviews(Guid id) => throw new NotImplementedException();

        public IEnumerable<ProductReviewModel> GetProductReviews(ProductModel product) => throw new NotImplementedException();

    }
}
