namespace ThAmCo.Catalogue.Services.Review
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class FakeReviewService : IReviewService
    {

        private readonly IList<ProductReviewModel> productReviewData;

        public FakeReviewService(IList<ProductReviewModel> productStockData)
        {
            this.productReviewData = productStockData;
        }

        public FakeReviewService() :
            this(
                new List<ProductReviewModel>()
                {
                    new ProductReviewModel()
                    {
                        ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                        Date = DateTime.UtcNow,
                        Description = "Test Review of product",
                        Name = "Test"
                    },
                    new ProductReviewModel()
                    {
                        ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                        Date = DateTime.UtcNow.AddDays(-7),
                        Description = "Test Review 2 of product",
                        Name = "Test 2"
                    }
                }
            )
        {
        }

        public IEnumerable<ProductReviewModel> GetProductReviews(Guid id) => this.productReviewData.Where(p => p.ProductId.Equals(id));

        public IEnumerable<ProductReviewModel> GetProductReviews(ProductModel product) => this.productReviewData.Where(p => p.ProductId.Equals(product.Id));

    }
}
