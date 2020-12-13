namespace ThAmCo.Catalogue.Services.Review
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public interface IReviewService : IService
    {

        public IEnumerable<ProductReviewModel> GetProductReviews(Guid id);

        public IEnumerable<ProductReviewModel> GetProductReviews(ProductModel product);

    }
}